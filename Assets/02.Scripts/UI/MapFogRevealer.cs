using UnityEngine;
using UnityEngine.AI;

public class MapFogRevealer : MonoBehaviour
{
    public Transform Player;
    public Material DrawMaterial;
    public RenderTexture FogMask;
    public float RevealRadius = 10f;

    private Vector2 _mapMin;
    private Vector2 _mapMax;

    void Start()
    {
        CalculateNavMeshBounds(out _mapMin, out _mapMax);
        Global.Instance.MapMax = _mapMax;
        Global.Instance.MapMin = _mapMin;
        ClearFogMask();
        Player = PlayerManager.Instance.Player?.transform;
    }

    void Update()
    {
        if (Player == null) return;
        Vector3 pos = Player.position;

        float u = Mathf.InverseLerp(_mapMin.x, _mapMax.x, pos.x);
        float v = Mathf.InverseLerp(_mapMin.y, _mapMax.y, pos.z);

        u = Mathf.Clamp01(u);
        v = Mathf.Clamp01(v);

        Vector2 uv = new Vector2(u, v);

        RenderTexture temp = RenderTexture.GetTemporary(FogMask.width, FogMask.height, 0, FogMask.format);

        // Copy current FogMask to temp
        Graphics.Blit(FogMask, temp);

        // Set material properties
        DrawMaterial.SetTexture("_MainTex", temp);
        DrawMaterial.SetVector("_Center", uv);
        DrawMaterial.SetFloat("_Radius", RevealRadius / (_mapMax.x - _mapMin.x));

        // Draw new fog circle using Max blending
        Graphics.Blit(null, FogMask, DrawMaterial);

        RenderTexture.ReleaseTemporary(temp);
    }

    void CalculateNavMeshBounds(out Vector2 min, out Vector2 max)
    {
        var triangulation = NavMesh.CalculateTriangulation();
        var verts = triangulation.vertices;

        if (verts.Length == 0)
        {
            min = Vector2.zero;
            max = Vector2.one;
            return;
        }

        float minX = verts[0].x;
        float maxX = verts[0].x;
        float minZ = verts[0].z;
        float maxZ = verts[0].z;

        foreach (var v in verts)
        {
            minX = Mathf.Min(minX, v.x);
            maxX = Mathf.Max(maxX, v.x);
            minZ = Mathf.Min(minZ, v.z);
            maxZ = Mathf.Max(maxZ, v.z);
        }

        min = new Vector2(minX, minZ);
        max = new Vector2(maxX, maxZ);
    }

    void ClearFogMask()
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = FogMask;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = active;
    }
}