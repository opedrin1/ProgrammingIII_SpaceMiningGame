using System;
using System.Collections;
using UnityEngine;

/// Player HP and shield system.
/// hp: permanent damage, no regeneration.
/// shield: regenerates 1 point every _shieldRegenDelay seconds of not being hit.
/// any hit interrupts and restarts that countdown. Hits drain shield first. Once shield is at 0, hits drain HP instead.

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHp = 5;
    
    [Header("Shield")]
    [SerializeField] private int maxShield = 5;
    [SerializeField] private float shieldRegenDelay = 10f;

    private int _currentHp;
    private int _currentShield;
    private Coroutine _shieldRegenCoroutine;

    public int CurrentHp => _currentHp;
    public int CurrentShield => _currentShield;
    public int MaxHp => maxHp;
    public int MaxShield => maxShield;

    // fired whenever HP changes, passing the new value
    public event Action<int> OnHpChanged;

    // fired whenever shield changes (hit or regen), passing the new value
    public event Action<int> OnShieldChanged;

    // fired specifically when a hit consumes a shield point (not on regen)
    public event Action OnShieldHit;

    // fired continuously while regenerating, passing progress from 0 to 1. Resets to 0 on interruption or once a point is gained
    public event Action<float> OnShieldRegenProgress;

    // fired once when HP reaches 0
    public event Action OnPlayerDied;

    private void Awake()
    {
        _currentHp = maxHp;
        _currentShield = maxShield;
    }
    
    public void TakeHit()
    {
        if (_currentHp <= 0) return; // already dead, ignore further hits

        if (_currentShield > 0)
        {
            _currentShield--;
            OnShieldChanged?.Invoke(_currentShield);
            OnShieldHit?.Invoke();
        }
        else
        {
            _currentHp--;
            OnHpChanged?.Invoke(_currentHp);

            if (_currentHp <= 0)
            {
                OnPlayerDied?.Invoke();
                return;
            }
        }

        RestartShieldRegen();
    }

    private void RestartShieldRegen()
    {
        if (_shieldRegenCoroutine != null)
            StopCoroutine(_shieldRegenCoroutine);

        OnShieldRegenProgress?.Invoke(0f);

        if (_currentShield < maxShield)
            _shieldRegenCoroutine = StartCoroutine(ShieldRegenRoutine());
    }

    private IEnumerator ShieldRegenRoutine()
    {
        while (_currentShield < maxShield)
        {
            float elapsed = 0f;
            while (elapsed < shieldRegenDelay)
            {
                elapsed += Time.deltaTime;
                OnShieldRegenProgress?.Invoke(Mathf.Clamp01(elapsed / shieldRegenDelay));
                yield return null;
            }

            _currentShield++;
            OnShieldChanged?.Invoke(_currentShield);
            OnShieldRegenProgress?.Invoke(0f); // reset for the next point, or final idle state if now full
        }

        _shieldRegenCoroutine = null;
    }
}