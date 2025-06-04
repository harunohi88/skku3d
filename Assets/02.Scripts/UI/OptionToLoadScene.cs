using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionToLoadScene : MonoBehaviour
{
    public void OnMainButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
