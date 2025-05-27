using UnityEngine;
using System.Collections;

public class EnemyHitEffect : MonoBehaviour
{
    private Renderer _renderer;
    private Color _hitEmissionColor = Color.gray;

    private MaterialPropertyBlock _propertyBlock;
    private Color _originalEmissionColor;
    private readonly int EMISSION_COLOR_ID = Shader.PropertyToID("_EmissionColor");
    private bool _isFlashing;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_propertyBlock);
        _originalEmissionColor = _propertyBlock.GetColor(EMISSION_COLOR_ID);
    }

    public void PlayHitEffect(float duration)
    {
        if (_isFlashing) return;
        StartCoroutine(Flash_Coroutine(duration));

    }

    private IEnumerator Flash_Coroutine(float duration)
    {
        _isFlashing = true;

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(EMISSION_COLOR_ID, _hitEmissionColor);
        _renderer.SetPropertyBlock(_propertyBlock);

        yield return new WaitForSeconds(duration);

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(EMISSION_COLOR_ID, _originalEmissionColor);
        _renderer.SetPropertyBlock(_propertyBlock);

        _isFlashing = false;
    }
}
