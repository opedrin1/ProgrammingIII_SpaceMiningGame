using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// spaceship controller
/// rotation: face the mouse cursor
/// thrust: W accelerates toward maxSpeed, S decelerates toward 0.
/// speed is clamped at 0, so the ship can never move backwards.
/// </summary>

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

    private Rigidbody2D _rb;
    private float _currentSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f; // no gravity in space
        if (cam == null) cam = Camera.main;
    }

    private void FixedUpdate()
    {
        RotateTowardsMouse();
        UpdateSpeed();

        _rb.linearVelocity = (Vector2)transform.up * _currentSpeed;
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