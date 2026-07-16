using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
/// <summary>
/// Temporary debug helper — press the assigned key to simulate the player
/// getting hit, for testing shield/HP behavior without real enemy projectiles yet.
/// Only compiles in the Editor or Development Builds, so it's automatically
/// stripped out of release builds. Safe to delete entirely once real
/// projectiles exist.
/// </summary>
public class DebugHitTester : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Key _hitKey = Key.H;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current[_hitKey].wasPressedThisFrame)
        {
            _playerHealth.TakeHit();
        }
    }
}
#endif