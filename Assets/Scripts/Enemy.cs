using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float turnSpeed = 180f;
    
    [Header("Weapons")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private GameObject projectilePrefab;

    private Rigidbody2D _rb;
    private Transform _player;
    private float _fireTimer;
    private bool _fireFromRight;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;
    }
    
    public void ApplyDifficultyMultiplier(float moveSpeedMultiplier, float fireRateMultiplier)
    {
        moveSpeed *= moveSpeedMultiplier;
        fireRate *= fireRateMultiplier;
    }

    private void FixedUpdate()
    {
        if (_player == null) return;

        RotateTowardsPlayer();
        MoveForward();
    }

    private void Update()
    {
        if (_player == null) return;

        _fireTimer += Time.deltaTime;
        float fireInterval = 1f / Mathf.Max(fireRate, 0.01f);

        if (_fireTimer >= fireInterval)
        {
            _fireTimer -= fireInterval;
            Fire();
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector2 toPlayer = (Vector2)_player.position - _rb.position;
        if (toPlayer.sqrMagnitude < 0.0001f) return;

        float targetAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg - 90f;
        float newAngle = Mathf.MoveTowardsAngle(_rb.rotation, targetAngle, turnSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(newAngle);
    }

    private void MoveForward()
    {
        _rb.MovePosition(_rb.position + (Vector2)transform.up * moveSpeed * Time.fixedDeltaTime);
    }

    private void Fire()
    {
        Transform origin = _fireFromRight ? rightFirePoint : leftFirePoint;
        _fireFromRight = !_fireFromRight;

        if (projectilePrefab == null || origin == null) return;

        Instantiate(projectilePrefab, origin.position, transform.rotation);
    }
}