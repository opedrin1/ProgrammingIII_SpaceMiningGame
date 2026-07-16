using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;

    private void Awake()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.CurrentState == GameState.Playing || 
            GameManager.Instance.CurrentState == GameState.Paused)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                GameManager.Instance.TogglePause();
            }
        }
    }

    private void HandleGameStateChanged(GameState state)
    {
        pauseMenuCanvas.SetActive(state == GameState.Paused);
    }

    public void Resume() => GameManager.Instance.TogglePause();
    public void Restart() => GameManager.Instance.RestartGame();
    public void QuitGame() => GameManager.Instance.QuitGame();
}