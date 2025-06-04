using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullscreenToggle;
    public Slider BGMSlider;
    public Slider SFXSlider;
    private List<Resolution> _resolutionList = new List<Resolution>();
    

    private void Start()
    {
        Debug.Log($"Initial Screen Settings - Resolution: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}");
        
        // 해상도 옵션 초기화
        ResolutionDropdown.ClearOptions();
        List<string> options = new List<string>
        {
            "1280 x 720 (HD)",
            "1920 x 1080 (FHD)",
            "2560 x 1440 (QHD)",
            "3840 x 2160 (UHD/4K)"
        };
        ResolutionDropdown.AddOptions(options);

        // 현재 해상도에 맞는 옵션 선택
        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;
        Debug.Log($"Current System Resolution: {currentWidth}x{currentHeight}");
        
        int selectedIndex = 0;

        if (currentWidth == 3840 && currentHeight == 2160)
            selectedIndex = 3;
        else if (currentWidth == 2560 && currentHeight == 1440)
            selectedIndex = 2;
        else if (currentWidth == 1920 && currentHeight == 1080)
            selectedIndex = 1;
        else if (currentWidth == 1280 && currentHeight == 720)
            selectedIndex = 0;

        ResolutionDropdown.value = selectedIndex;
        ResolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

        // 전체화면 토글 초기화
        FullscreenToggle.isOn = Screen.fullScreen;
        FullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        Debug.Log($"Initial Fullscreen Toggle State: {FullscreenToggle.isOn}");
    }

    private void OnResolutionChanged(int index)
    {
        int width = 0;
        int height = 0;

        switch (index)
        {
            case 0: // HD
                width = 1280;
                height = 720;
                break;
            case 1: // FHD
                width = 1920;
                height = 1080;
                break;
            case 2: // QHD
                width = 2560;
                height = 1440;
                break;
            case 3: // UHD/4K
                width = 3840;
                height = 2160;
                break;
        }

        Debug.Log($"Attempting to change resolution to: {width}x{height}");
        Screen.SetResolution(width, height, Screen.fullScreen);
        Debug.Log($"Resolution changed - Current: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}");
    }

    private void OnFullscreenChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log($"Fullscreen mode changed - Current: {Screen.fullScreen}");
    }

    public void OnBGMSliderValueChanged()
    {
        AudioManager.Instance.SetBGMVolume(BGMSlider.value);
    }

    public void OnSFXSliderValueChanged()
    {
        AudioManager.Instance.SetSFXVolume(SFXSlider.value);
    }
}