using UnityEngine;
using System.Collections;

public class CameraManager : BehaviourSingleton<CameraManager>
{
    private Vector3 _originalPosition;
    public Vector3 ShakePosition;

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

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            ShakePosition = (transform.right * x) + (transform.up * y);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        ShakePosition = Vector3.one;

        IsShaking = false;
    }
}
