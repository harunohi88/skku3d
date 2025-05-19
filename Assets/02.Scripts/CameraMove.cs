using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform Target;

    private void Start()
    {
        transform.rotation = Target.rotation;
    }

    private void Update()
    {
        this.transform.position = Target.position;
    }
}
