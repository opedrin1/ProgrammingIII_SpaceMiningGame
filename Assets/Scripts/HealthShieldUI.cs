using System.Collections.Generic;
using UnityEngine;

public class HealthShieldUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    
    [Header("HP Row")]
    [SerializeField] private RectTransform hpContainer;
    [SerializeField] private GameObject hpPipPrefab;
    
    [Header("Shield Row")]
    [SerializeField] private RectTransform shieldContainer;
    [SerializeField] private GameObject shieldPipPrefab;

    private readonly List<GameObject> _hpPips = new List<GameObject>();
    private readonly List<GameObject> _shieldPips = new List<GameObject>();

    private void Awake()
    {
        BuildPipRow(hpContainer, hpPipPrefab, playerHealth.MaxHp, _hpPips);
        BuildPipRow(shieldContainer, shieldPipPrefab, playerHealth.MaxShield, _shieldPips);
    }

    private void OnEnable()
    {
        playerHealth.OnHpChanged += UpdateHpDisplay;
        playerHealth.OnShieldChanged += UpdateShieldDisplay;
    }

    private void OnDisable()
    {
        playerHealth.OnHpChanged -= UpdateHpDisplay;
        playerHealth.OnShieldChanged -= UpdateShieldDisplay;
    }

    private void Start()
    {
        UpdateHpDisplay(playerHealth.CurrentHp);
        UpdateShieldDisplay(playerHealth.CurrentShield);
    }

    private void BuildPipRow(RectTransform container, GameObject prefab, int count, List<GameObject> targetList)
    {
        targetList.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject pip = Instantiate(prefab, container);
            targetList.Add(pip);
        }
    }

    private void UpdateHpDisplay(int currentHp)
    {
        SetActiveCount(_hpPips, currentHp);
    }

    private void UpdateShieldDisplay(int currentShield)
    {
        SetActiveCount(_shieldPips, currentShield);
    }

    private void SetActiveCount(List<GameObject> pips, int activeCount)
    {
        for (int i = 0; i < pips.Count; i++)
        {
            pips[i].SetActive(i < activeCount);
        }
    }
}