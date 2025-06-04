using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform Target;

    private void Start()
    {
        SceneManager.sceneLoaded += LoadCameraPositionOnScene;
    }

    public void LoadCameraPositionOnScene(Scene scene, LoadSceneMode mode)
    {
        Target = GameObject.FindGameObjectWithTag("CameraPosition")?.transform;
    }

    private void Update()
    {
        if (Target != null)
        {
            if (GameManager.Instance.IsStart == false) return;

            transform.rotation = Target.rotation;
            this.transform.position = Target.position + CameraManager.Instance.ShakePosition;
        }
    }
}
