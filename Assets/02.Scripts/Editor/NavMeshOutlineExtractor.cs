using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections.Generic;
using System.IO;

public class NavMeshWalkableOutlineExporter : EditorWindow
{
    int textureSize = 1024;
    string saveFileName = "NavMeshWalkableOutline.png";
    float minEdgeLength = 0.3f;

    [MenuItem("Tools/NavMesh/Export Walkable Outline")]
    public static void ShowWindow()
    {
        GetWindow<NavMeshWalkableOutlineExporter>("Walkable NavMesh Outline");
    }

    void OnGUI()
    {
        GUILayout.Label("Walkable NavMesh Outline Exporter", EditorStyles.boldLabel);
        textureSize = EditorGUILayout.IntField("Texture Size", textureSize);
        saveFileName = EditorGUILayout.TextField("Output File Name", saveFileName);
        minEdgeLength = EditorGUILayout.FloatField("Min Edge Length", minEdgeLength);

        if (GUILayout.Button("Export Walkable Outline"))
        {
            ExportOutline();
        }
    }

    void ExportOutline()
    {
        Texture2D tex = GenerateWalkableOutlineTexture(textureSize, textureSize);
        byte[] pngData = tex.EncodeToPNG();

        string folderPath = Application.dataPath + "/Minimap";
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        string filePath = folderPath + "/" + saveFileName;
        File.WriteAllBytes(filePath, pngData);

        Debug.Log("Walkable NavMesh outline saved to: " + filePath);
        AssetDatabase.Refresh();
    }

    Texture2D GenerateWalkableOutlineTexture(int width, int height)
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        var edges = new Dictionary<(Vector3, Vector3), int>();
        var vertices = navMeshData.vertices;
        var indices = navMeshData.indices;

        for (int i = 0; i < indices.Length; i += 3)
        {
            Vector3 a = vertices[indices[i]];
            Vector3 b = vertices[indices[i + 1]];
            Vector3 c = vertices[indices[i + 2]];

            AddEdge(edges, a, b);
            AddEdge(edges, b, c);
            AddEdge(edges, c, a);
        }

        var boundaryEdges = new List<(Vector3, Vector3)>();
        foreach (var edge in edges)
        {
            if (edge.Value == 1)
                boundaryEdges.Add(edge.Key);
        }

        List<List<Vector3>> polylines = BuildPolylines(boundaryEdges);

        if (polylines.Count == 0)
        {
            Debug.LogWarning("No valid outlines found.");
            return new Texture2D(width, height);
        }

        // 스케일 계산
        Vector2 min = GetMin(polylines);
        Vector2 max = GetMax(polylines);
        Vector2 scale = new Vector2(width / (max.x - min.x + 0.01f), height / (max.y - min.y + 0.01f));

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Color clear = new Color(0, 0, 0, 0);
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                tex.SetPixel(x, y, clear);

        // 가장 바깥 폴리라인 찾기
        List<Vector3> outer = null;
        float maxArea = float.MinValue;
        foreach (var line in polylines)
        {
            float area = Mathf.Abs(SignedPolygonArea(line));
            if (area > maxArea)
            {
                maxArea = area;
                outer = line;
            }
        }

        // 내부 채우기
        if (outer != null)
        {
            List<Vector2> projected = new();
            foreach (var p in outer)
                projected.Add(ProjectXZ(p, min, scale));
            DrawPolygonFill(tex, projected, new Color(0.8f, 0.8f, 0.8f, 1f)); // 회색
        }

        // 외곽선 그리기
        foreach (var line in polylines)
        {
            for (int i = 0; i < line.Count; i++)
            {
                Vector2 p1 = ProjectXZ(line[i], min, scale);
                Vector2 p2 = ProjectXZ(line[(i + 1) % line.Count], min, scale);
                DrawLine(tex, (int)p1.x, (int)p1.y, (int)p2.x, (int)p2.y, Color.white);
            }
        }

        tex.Apply();
        return tex;
    }

    void AddEdge(Dictionary<(Vector3, Vector3), int> edges, Vector3 a, Vector3 b)
    {
        a = NormalizeVertex(a);
        b = NormalizeVertex(b);

        if (Vector3.Distance(a, b) < minEdgeLength || IsAlmostVertical(a, b))
            return;

        var key = (a, b);
        var reverseKey = (b, a);
        if (edges.ContainsKey(reverseKey))
            edges[reverseKey]++;
        else if (edges.ContainsKey(key))
            edges[key]++;
        else
            edges[key] = 1;
    }

    Vector3 NormalizeVertex(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x * 100f) / 100f, Mathf.Round(v.y * 100f) / 100f, Mathf.Round(v.z * 100f) / 100f);
    }

    bool IsAlmostVertical(Vector3 a, Vector3 b)
    {
        Vector3 dir = (b - a).normalized;
        return Mathf.Abs(dir.x) < 0.05f && Mathf.Abs(dir.z) < 0.05f;
    }

    List<List<Vector3>> BuildPolylines(List<(Vector3, Vector3)> edges)
    {
        var used = new HashSet<int>();
        var polylines = new List<List<Vector3>>();

        for (int i = 0; i < edges.Count; i++)
        {
            if (used.Contains(i)) continue;
            var polyline = new List<Vector3>();
            var current = edges[i].Item1;
            var start = current;
            polyline.Add(current);
            used.Add(i);

            Vector3 next = edges[i].Item2;
            polyline.Add(next);

            bool closed = false;

            while (true)
            {
                bool found = false;
                for (int j = 0; j < edges.Count; j++)
                {
                    if (used.Contains(j)) continue;
                    if (edges[j].Item1 == next)
                    {
                        next = edges[j].Item2;
                        polyline.Add(next);
                        used.Add(j);
                        found = true;
                        break;
                    }
                    else if (edges[j].Item2 == next)
                    {
                        next = edges[j].Item1;
                        polyline.Add(next);
                        used.Add(j);
                        found = true;
                        break;
                    }
                }
                if (!found || next == start)
                {
                    closed = (next == start);
                    break;
                }
            }

            if (polyline.Count > 2)
                polylines.Add(polyline);
        }

        return polylines;
    }

    Vector2 GetMin(List<List<Vector3>> polys)
    {
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        foreach (var poly in polys)
        {
            foreach (var v in poly)
                min = Vector2.Min(min, new Vector2(v.x, v.z));
        }
        return min;
    }

    Vector2 GetMax(List<List<Vector3>> polys)
    {
        Vector2 max = new Vector2(float.MinValue, float.MinValue);
        foreach (var poly in polys)
        {
            foreach (var v in poly)
                max = Vector2.Max(max, new Vector2(v.x, v.z));
        }
        return max;
    }

    Vector2 ProjectXZ(Vector3 pos, Vector2 min, Vector2 scale)
    {
        return new Vector2((pos.x - min.x) * scale.x, (pos.z - min.y) * scale.y);
    }

    float SignedPolygonArea(List<Vector3> poly)
    {
        float area = 0f;
        for (int i = 0; i < poly.Count; i++)
        {
            Vector3 a = poly[i];
            Vector3 b = poly[(i + 1) % poly.Count];
            area += (a.x * b.z - b.x * a.z);
        }
        return area * 0.5f;
    }

    void DrawLine(Texture2D tex, int x0, int y0, int x1, int y1, Color col)
    {
        int dx = Mathf.Abs(x1 - x0), dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if (x0 >= 0 && y0 >= 0 && x0 < tex.width && y0 < tex.height)
                tex.SetPixel(x0, y0, col);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
    }

    void DrawPolygonFill(Texture2D tex, List<Vector2> poly, Color color)
    {
        int minY = (int)poly[0].y, maxY = (int)poly[0].y;
        foreach (var p in poly)
        {
            minY = Mathf.Min(minY, (int)p.y);
            maxY = Mathf.Max(maxY, (int)p.y);
        }

        for (int y = minY; y <= maxY; y++)
        {
            List<int> nodeX = new();
            for (int i = 0; i < poly.Count; i++)
            {
                Vector2 p1 = poly[i];
                Vector2 p2 = poly[(i + 1) % poly.Count];

                if ((p1.y < y && p2.y >= y) || (p2.y < y && p1.y >= y))
                {
                    int x = (int)(p1.x + (y - p1.y) / (p2.y - p1.y) * (p2.x - p1.x));
                    nodeX.Add(x);
                }
            }

            nodeX.Sort();
            for (int i = 0; i < nodeX.Count - 1; i += 2)
            {
                for (int x = nodeX[i]; x < nodeX[i + 1]; x++)
                {
                    if (x >= 0 && x < tex.width && y >= 0 && y < tex.height)
                        tex.SetPixel(x, y, color);
                }
            }
        }
    }
}
