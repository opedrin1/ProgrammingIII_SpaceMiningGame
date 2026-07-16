using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnPlayerDied -= HandlePlayerDied;
    }

    private void HandlePlayerDied()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToEndScreen();
        }
        else
        {
            Debug.LogWarning("GameManager.Instance is null.");
        }
    }
}