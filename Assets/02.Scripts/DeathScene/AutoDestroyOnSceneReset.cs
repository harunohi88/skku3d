using UnityEngine;

public class AutoDestroyOnSceneReset : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
