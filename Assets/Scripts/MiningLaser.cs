using UnityEngine;
using UnityEngine.InputSystem;

public class MiningLaser : MonoBehaviour
{
    [Header("Beam Origins")]
    [SerializeField] private Transform leftOrigin;
    [SerializeField] private Transform rightOrigin;

    [Header("Beam Visuals")]
    [SerializeField] private LineRenderer leftBeam;
    [SerializeField] private LineRenderer rightBeam;

    [Header("Beam Settings")]
    [SerializeField] private float maxRange = 10f;
    [SerializeField] private float miningRate = 5f;
    [SerializeField] private LayerMask mineableLayer;
    [SerializeField] private Camera cam;

    private bool _isFiring;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
        SetBeamsActive(false);
    }

    private void Update()
    {
        _isFiring = Mouse.current.rightButton.isPressed;
        SetBeamsActive(_isFiring);

        if (!_isFiring) return;

        Vector2 originMid = (leftOrigin.position + rightOrigin.position) * 0.5f;

        Vector3 screenPos = Mouse.current.position.ReadValue();
        screenPos.z = Mathf.Abs(cam.transform.position.z);
        Vector2 mouseWorld = cam.ScreenToWorldPoint(screenPos);

        Vector2 toMouse = mouseWorld - originMid;
        if (toMouse.sqrMagnitude < 0.0001f) return;

        float castDistance = Mathf.Min(toMouse.magnitude, maxRange);
        Vector2 aimDir = toMouse.normalized;
        Vector2 endPoint = originMid + aimDir * castDistance;

        RaycastHit2D hit = Physics2D.Raycast(originMid, aimDir, castDistance, mineableLayer);
        if (hit.collider != null)
        {
            endPoint = hit.point;

            if (hit.collider.TryGetComponent(out IMineable mineable) && !mineable.IsDepleted)
            {
                float extracted = mineable.Mine(miningRate * Time.deltaTime);
                float valueEarned = extracted * mineable.ValuePerUnit;

                if (valueEarned > 0f && PlayerWallet.Instance != null)
                    PlayerWallet.Instance.AddValue(valueEarned);
            }
        }

        DrawBeam(leftBeam, leftOrigin.position, endPoint);
        DrawBeam(rightBeam, rightOrigin.position, endPoint);
    }

    private void DrawBeam(LineRenderer beam, Vector3 origin, Vector3 target)
    {
        beam.SetPosition(0, origin);
        beam.SetPosition(1, target);
    }

    private void SetBeamsActive(bool active)
    {
        leftBeam.enabled = active;
        rightBeam.enabled = active;
    }
}