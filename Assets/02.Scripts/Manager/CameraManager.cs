using UnityEngine;
using System.Collections;

public class CameraManager : BehaviourSingleton<CameraManager>
{
    private Vector3 _originalPosition;
    private Vector3 _shakePosition;

    public bool IsShaking = false;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private Coroutine _cameraShakeCoroutine;
    public void CameraShake(float magnitude, float duration)
    {
        if (_cameraShakeCoroutine != null)
        {
            StopCoroutine(_cameraShakeCoroutine);
        }
        _cameraShakeCoroutine = StartCoroutine(ShakeCoroutine(magnitude, duration));
    }

    private IEnumerator ShakeCoroutine(float magnitude, float duration)
    {
        IsShaking = true;

        float elapsed = 0.0f;

        Vector3 originalPosition = transform.localPosition;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;

        IsShaking = false;
    }
}
