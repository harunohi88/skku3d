using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections.Generic;
using System.IO;

public class NavMeshWalkableOutlineExporter : EditorWindow
{
    int textureSize = 1024;
    string saveFileName = "NavMeshWalkableOutline.png";
    float edgeSearchStep = 0.5f;
    float edgeDetectRadius = 1f;
    int maxSteps = 500;

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
        edgeSearchStep = EditorGUILayout.FloatField("Edge Follow Step", edgeSearchStep);
        edgeDetectRadius = EditorGUILayout.FloatField("Edge Detection Radius", edgeDetectRadius);
        maxSteps = EditorGUILayout.IntField("Max Outline Steps", maxSteps);

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
        // 샘플 위치 (중앙)
        Vector3 origin = GameObject.FindGameObjectWithTag("Player").transform.position;
        NavMeshHit startHit;
        if (!NavMesh.SamplePosition(origin, out startHit, 10f, NavMesh.AllAreas))
        {
            Debug.LogError("NavMesh not found near origin.");
            return null;
        }

        List<Vector3> edgePoints = new();
        Vector3 currentPos = startHit.position;
        Vector3 direction = Vector3.right;

        for (int i = 0; i < maxSteps; i++)
        {
            if (NavMesh.FindClosestEdge(currentPos, out NavMeshHit edgeHit, NavMesh.AllAreas))
            {
                edgePoints.Add(edgeHit.position);

                // 진행 방향 변경 (외곽 따라 회전)
                direction = Quaternion.AngleAxis(90, Vector3.up) * edgeHit.normal;
                Vector3 nextPos = edgeHit.position + direction * edgeSearchStep;

                if (NavMesh.SamplePosition(nextPos, out NavMeshHit nextHit, 2f, NavMesh.AllAreas))
                {
                    currentPos = nextHit.position;
                }
                else
                {
                    break; // 막힘
                }
            }
            else
            {
                break;
            }
        }

        // 이미지 생성
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Color clear = new Color(0, 0, 0, 0);
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                tex.SetPixel(x, y, clear);

        if (edgePoints.Count < 2) return tex;

        // 스케일 설정
        Vector2 min = GetMin(edgePoints);
        Vector2 max = GetMax(edgePoints);
        Vector2 scale = new Vector2(width / (max.x - min.x), height / (max.y - min.y));

        for (int i = 0; i < edgePoints.Count - 1; i++)
        {
            Vector2 p1 = ProjectXZ(edgePoints[i], min, scale);
            Vector2 p2 = ProjectXZ(edgePoints[i + 1], min, scale);
            DrawLine(tex, (int)p1.x, (int)p1.y, (int)p2.x, (int)p2.y, Color.green);
        }

        tex.Apply();
        return tex;
    }

    Vector2 ProjectXZ(Vector3 pos, Vector2 min, Vector2 scale)
    {
        return new Vector2((pos.x - min.x) * scale.x, (pos.z - min.y) * scale.y);
    }

    Vector2 GetMin(List<Vector3> verts)
    {
        Vector2 min = new Vector2(verts[0].x, verts[0].z);
        foreach (var v in verts)
            min = Vector2.Min(min, new Vector2(v.x, v.z));
        return min;
    }

    Vector2 GetMax(List<Vector3> verts)
    {
        Vector2 max = new Vector2(verts[0].x, verts[0].z);
        foreach (var v in verts)
            max = Vector2.Max(max, new Vector2(v.x, v.z));
        return max;
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
}
