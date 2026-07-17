using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Fire Points")]
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    
    [Header("Weapon Settings")]
    [Tooltip("Shots per second.")]
    [SerializeField] private float fireRate = 4f;
    [SerializeField] private GameObject projectilePrefab;

    private float _fireTimer;
    private bool _fireFromRight;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Fire();
            _fireTimer = 0f;
            return;
        }

        if (!Mouse.current.leftButton.isPressed) return;

        _fireTimer += Time.deltaTime;
        float fireInterval = 1f / Mathf.Max(fireRate, 0.01f);

        if (_fireTimer >= fireInterval)
        {
            _fireTimer -= fireInterval;
            Fire();
        }
    }

    private void Fire()
    {
        Transform origin = _fireFromRight ? rightFirePoint : leftFirePoint;
        _fireFromRight = !_fireFromRight;

        if (projectilePrefab == null || origin == null) return;

        Instantiate(projectilePrefab, origin.position, transform.rotation);
    }
}