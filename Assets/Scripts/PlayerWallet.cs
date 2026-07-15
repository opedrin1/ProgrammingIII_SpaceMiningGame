using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet Instance { get; private set; }
    
    public event Action<float> OnValueChanged;

    public float TotalValue { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddValue(float amount)
    {
        if (amount <= 0f) return;

        TotalValue += amount;
        OnValueChanged?.Invoke(TotalValue);
    }
}