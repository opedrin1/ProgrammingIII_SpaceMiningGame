using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Player HP and shield system.
/// hp: permanent damage, no regeneration.
/// shield: regenerates 1 point every _shieldRegenDelay seconds of not being hit.
/// any hit interrupts and restarts that countdown. hits drain shield first. Once shield is at 0, hits drain HP instead.
/// </summary>
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

    // fired whenever shield changes, passing the new value
    public event Action<int> OnShieldChanged;

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

        if (_currentShield < maxShield)
            _shieldRegenCoroutine = StartCoroutine(ShieldRegenRoutine());
    }

    private IEnumerator ShieldRegenRoutine()
    {
        while (_currentShield < maxShield)
        {
            yield return new WaitForSeconds(shieldRegenDelay);

            _currentShield++;
            OnShieldChanged?.Invoke(_currentShield);
        }

        _shieldRegenCoroutine = null;
    }
}