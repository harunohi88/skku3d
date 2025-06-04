using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_test : MonoBehaviour
{
    int i = 1;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
