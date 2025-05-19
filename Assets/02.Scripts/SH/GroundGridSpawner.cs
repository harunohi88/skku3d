using UnityEngine;

public class GroundGridSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int row = 10;
    public int col = 10;
    public float spacing = 2f;

    void Start()
    {
        for (int x = 0; x < row; x++)
        {
            for (int z = 0; z < col; z++)
            {
                Vector3 pos = new Vector3(x * spacing, 0, z * spacing);
                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }
    }
}
