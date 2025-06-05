using UnityEngine;

public class Reset : MonoBehaviour
{
    public void OnClickReset()
    {
        InputManager.Instance.TurnOff = true;
        ResetManager.Instance.OnClickRestart();
    }
}
