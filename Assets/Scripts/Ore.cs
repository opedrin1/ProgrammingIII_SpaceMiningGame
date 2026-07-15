using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Ore : MonoBehaviour, IMineable
{
    public enum OreType
    {
        Iron,
        Platinum,
        Gold,
        Cobalt,
        Chromite
    }

    [SerializeField] private OreType oreType;
    [SerializeField] private float totalAmount = 50f;
    [SerializeField] private float valuePerUnit = 1f;

    private float _remainingAmount;

    public OreType Type => oreType;
    public float ValuePerUnit => valuePerUnit;
    public bool IsDepleted => _remainingAmount <= 0f;

    private void Awake()
    {
        _remainingAmount = totalAmount;
    }

    public float Mine(float amount)
    {
        float extracted = Mathf.Min(amount, _remainingAmount);
        _remainingAmount -= extracted;

        if (IsDepleted)
        {
            Debug.Log($"{oreType} ore depleted — total value earned: {totalAmount * valuePerUnit:F1}");
            Destroy(gameObject);
        }

        return extracted;
    }
}