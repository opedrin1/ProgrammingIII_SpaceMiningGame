using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnStartClicked() => GameManager.Instance.StartGame();
    public void OnRestartClicked() => GameManager.Instance.RestartGame();
    public void OnQuitClicked() => GameManager.Instance.QuitGame();
    
}