using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        this.transform.position = Target.position;
    }
}
