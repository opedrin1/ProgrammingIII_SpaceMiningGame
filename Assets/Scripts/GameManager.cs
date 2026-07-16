using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameState 
{
    Start,
    Playing,
    Paused,
    EndGame
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public GameState CurrentState { get; private set; }
    
    // event other scripts can subscribe to when the state changes
    public static event Action<GameState> OnGameStateChanged;

    [Header("Scene Names")]
    public string gameplayScene = "Gameplay";
    public string mainMenuScene = "MainMenu";
    public string endScreenScene = "GameOver";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // set the initial state based on whichever scene is currently loaded
        string activeScene = SceneManager.GetActiveScene().name;
        if (activeScene == mainMenuScene)
            ChangeState(GameState.Start);
        else if (activeScene == endScreenScene)
            ChangeState(GameState.EndGame);
        else
            ChangeState(GameState.Playing);
    }
    
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        
        switch (newState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.EndGame:
                Time.timeScale = 1f;
                break;
        }
        
        OnGameStateChanged?.Invoke(newState);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(gameplayScene);
        ChangeState(GameState.Playing);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(gameplayScene);
        ChangeState(GameState.Playing);
    }

    public void GoToEndScreen()
    {
        SceneManager.LoadScene(endScreenScene);
        ChangeState(GameState.EndGame);
    }

    // Toggles between Playing and Paused
    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (CurrentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}