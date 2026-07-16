using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 3;

    private int _currentHp;

    public int CurrentHp => _currentHp;
    public int MaxHp => maxHp;
    
    public event Action OnDied;

    private void Awake()
    {
        _currentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        if (_currentHp <= 0) return;

        _currentHp -= amount;

        if (_currentHp <= 0)
        {
            OnDied?.Invoke();
            Destroy(gameObject);
        }
    }
}