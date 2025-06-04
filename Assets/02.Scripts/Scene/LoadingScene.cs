using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LoadingScene : MonoBehaviour, IPointerClickHandler
{

    // - 프로그레스 슬라이더 바
    public Slider ProgressSlider;
    // - 프로그레스 텍스트
    public TextMeshProUGUI ProgressText;
    public TextMeshProUGUI ShowText;
    public bool _isTouchable = false;
    private AsyncOperation _asyncOperation;

    public List<string> TextList;

    private void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
        _isTouchable = false;
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        // 지정된 씬을 비동기로 로드한다.
        _asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        _asyncOperation.allowSceneActivation = false;

        // 로딩이 되는 동안 계속해서 반복문
        while(_asyncOperation.isDone == false)
        {
            ProgressText.text = $"{_asyncOperation.progress * 100f}%";

            int index = Mathf.Min((int)(_asyncOperation.progress * TextList.Count), TextList.Count - 1);
            ShowText.text = TextList[index];

            if (_asyncOperation.progress >= 0.9f)
            {
                ProgressText.text = "아무 곳이나 클릭하십시오.";
                _isTouchable = true;
                break;
            }

            yield return null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isTouchable)
        {
            _isTouchable = false;
            _asyncOperation.allowSceneActivation = true;
        }
    }
}
