using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneTrigger : MonoBehaviour
{
    public static CutSceneTrigger Instance;
    public PlayableDirector pd;
    public TimelineAsset[] ta;

    private void Start()
    {
        Instance = this;

        // 컷신 끝났을 때 실행될 이벤트 등록
        pd.stopped += OnCutsceneFinished;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("151456465");
        if (other.CompareTag("Player"))
        {
            Debug.Log("rwerwer");

            InputManager.Instance.TurnOff = true;   // 입력 잠금
            pd.Play(ta[0]);                         // 컷신 재생
            this.gameObject.SetActive(false);       // 트리거 꺼버리기
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        Debug.Log("컷신 끝났음");
        InputManager.Instance.TurnOff = false;      // 입력 다시 허용

    }
}
