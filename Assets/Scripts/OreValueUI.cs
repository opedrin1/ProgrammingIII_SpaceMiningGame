using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Displays the player's total ore value. Listens to PlayerWallet, so it
/// automatically stays correct no matter which ore type contributed the value.
/// </summary>
public class OreValueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;

    private void Start()
    {
        if (PlayerWallet.Instance != null)
        {
            PlayerWallet.Instance.OnValueChanged += UpdateDisplay;
            UpdateDisplay(PlayerWallet.Instance.TotalValue);
        }
    }

    private void OnDestroy()
    {
        if (PlayerWallet.Instance != null)
            PlayerWallet.Instance.OnValueChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(float newTotal)
    {
        valueText.text = $"{newTotal:F0}";
    }
}