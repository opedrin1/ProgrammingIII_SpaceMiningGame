using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Thrust")]
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 4f;
    [SerializeField] private float deceleration = 6f;
    [SerializeField] private float idleDrag = 1.5f;

    [Header("Rotation")]
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private Camera cam;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] private Key dashLeftKey = Key.A;
    [SerializeField] private Key dashRightKey = Key.D;

    private Rigidbody2D _rb;
    private float _currentSpeed;
    private bool _isDashing;
    private float _dashCooldownTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (_dashCooldownTimer > 0f)
            _dashCooldownTimer -= Time.deltaTime;

        if (_isDashing) return;

        if (Keyboard.current[dashLeftKey].wasPressedThisFrame)
            TryStartDash(-transform.right);
        else if (Keyboard.current[dashRightKey].wasPressedThisFrame)
            TryStartDash(transform.right);
    }

    private void FixedUpdate()
    {
        RotateTowardsMouse();

        if (!_isDashing)
        {
            UpdateSpeed();
            _rb.linearVelocity = (Vector2)transform.up * _currentSpeed;
        }
    }

    private void TryStartDash(Vector2 direction)
    {
        if (_dashCooldownTimer > 0f) return;

        StartCoroutine(DashRoutine(direction));
    }

    private IEnumerator DashRoutine(Vector2 direction)
    {
        _isDashing = true;
        _dashCooldownTimer = dashCooldown;

        float duration = dashDistance / Mathf.Max(dashSpeed, 0.01f);
        float elapsed = 0f;
        Vector2 dashVelocity = direction.normalized * dashSpeed;

        while (elapsed < duration)
        {
            _rb.linearVelocity = dashVelocity;
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _isDashing = false;
    }

    private void RotateTowardsMouse()
    {
        Vector3 screenPos = Mouse.current.position.ReadValue();
        screenPos.z = Mathf.Abs(cam.transform.position.z);
        Vector2 mouseWorld = cam.ScreenToWorldPoint(screenPos);

        Vector2 toMouse = mouseWorld - (Vector2)transform.position;
        if (toMouse.sqrMagnitude < 0.0001f) return;
        
        float targetAngle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg - 90f;
        float newAngle = Mathf.MoveTowardsAngle(_rb.rotation, targetAngle, turnSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(newAngle);
    }

    private void UpdateSpeed()
    {
        bool thrusting = Keyboard.current.wKey.isPressed;
        bool braking = Keyboard.current.sKey.isPressed;

        if (thrusting)
            _currentSpeed += acceleration * Time.fixedDeltaTime;
        else if (braking)
            _currentSpeed -= deceleration * Time.fixedDeltaTime;
        else
            _currentSpeed -= idleDrag * Time.fixedDeltaTime;

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxSpeed);
    }
}