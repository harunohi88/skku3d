using UnityEngine;
using System.Collections;

public class CameraManager : BehaviourSingleton<CameraManager>
{
    private Vector3 _originalPosition;
    private Vector3 _shakePosition;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void Shake(float duration, float magnitude)
    {
        _originalPosition = Camera.main.transform.position;
        _shakePosition = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            _shakePosition = new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _shakePosition = Vector3.zero;
        transform.localPosition = _originalPosition;
    }
}
