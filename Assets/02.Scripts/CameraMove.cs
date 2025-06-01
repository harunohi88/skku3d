
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform Target;

    private void Start()
    {
    }

    private void Update()
    {
        if (GameManager.Instance.IsStart == false) return;
        transform.rotation = Target.rotation;
        this.transform.position = Target.position;
    }
}
