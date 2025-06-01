using UnityEngine;

public class Merchant : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlayUIAudio(UIAudioType.Tab);
        if (InputManager.Instance._equipmentPanel.activeSelf && InputManager.Instance._inventoryPanel.activeSelf)
        {
            InputManager.Instance._equipmentPanel.SetActive(false);
            InputManager.Instance._upgradeAndShopPanel.SetActive(true);
        }
        else
        {
            InputManager.Instance._upgradeAndShopPanel.SetActive(true);
            InputManager.Instance._inventoryPanel.SetActive(true);
            InputManager.Instance.UpdateBackgroundImage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AudioManager.Instance.PlayUIAudio(UIAudioType.Tab);
        InputManager.Instance._equipmentPanel.SetActive(false);
        InputManager.Instance._upgradeAndShopPanel.SetActive(false);
        InputManager.Instance._inventoryPanel.SetActive(false);
        InputManager.Instance.UpdateBackgroundImage();
    }
}
