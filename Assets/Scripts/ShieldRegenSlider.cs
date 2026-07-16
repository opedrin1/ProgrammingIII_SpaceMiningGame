using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShieldRegenSlider : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider slider;
    
    [SerializeField] private GameObject visualRoot;

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnShieldRegenProgress += UpdateSlider;
            playerHealth.OnShieldChanged += HandleShieldChanged;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnShieldRegenProgress -= UpdateSlider;
            playerHealth.OnShieldChanged -= HandleShieldChanged;
        }
    }

    private void Start()
    {
        HandleShieldChanged(playerHealth != null ? playerHealth.CurrentShield : 0);
    }

    private void UpdateSlider(float progress)
    {
        slider.value = progress;
    }

    private void HandleShieldChanged(int currentShield)
    {
        bool isFull = playerHealth != null && currentShield >= playerHealth.MaxShield;
        visualRoot.SetActive(!isFull);
    }
}