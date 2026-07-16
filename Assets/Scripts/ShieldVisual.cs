using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShieldVisual : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite shieldedSprite;
    [SerializeField] private float flashDuration = 0.2f;

    private SpriteRenderer _spriteRenderer;
    private Coroutine _flashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = normalSprite;
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnShieldChanged += HandleShieldChanged;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnShieldChanged -= HandleShieldChanged;
    }

    private void HandleShieldChanged(int newShieldValue)
    {
        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);

        _flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _spriteRenderer.sprite = shieldedSprite;
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.sprite = normalSprite;
        _flashCoroutine = null;
    }
}