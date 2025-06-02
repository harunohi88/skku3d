using UnityEngine;

public class Boss3DoorOpen : MonoBehaviour
{
    public Transform[] Doors;
    public float openSpeed = 2.5f;
    public float openDistance = 2f;

    private bool isOpening = false;
    private float moved = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = true;
        }
    }

    private void Update()
    {
        if (!isOpening || moved >= openDistance) return;

        float delta = openSpeed * Time.deltaTime;
        moved += delta;

        // Clamp to prevent overshoot
        delta = Mathf.Min(delta, openDistance - moved);

        // 왼쪽 문은 +X 방향으로 이동
        Doors[0].position += new Vector3(0, 0, delta);

        // 오른쪽 문은 -X 방향으로 이동
        Doors[1].position -= new Vector3(0, 0, delta);
    }
}
