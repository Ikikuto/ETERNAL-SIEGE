using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern for easy global access
    public static GameManager Instance { get; private set; }

    [Header("Core References")]
    [SerializeField] private CrystalCoreHealth crystalCore;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject victoryUI;

    public enum GameState { Playing, Paused, GameOver, Victory }
    public GameState CurrentState { get; private set; } = GameState.Playing;

    private void Awake()
    {
        // Setup Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure UI panels are hidden at start
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (victoryUI != null) victoryUI.SetActive(false);

        Time.timeScale = 1f; // Ensure game is unpaused on start
    }

    private void Update()
    {
        // Example: Check if Crystal Core died
        if (crystalCore != null && crystalCore.IsDead && CurrentState == GameState.Playing)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        Debug.Log("Game Over!");

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        Time.timeScale = 0f; // Freeze the game
    }

    public void Victory()
    {
        CurrentState = GameState.Victory;
        Debug.Log("Victory!");

        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }

        Time.timeScale = 0f; // Freeze the game
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Unfreeze before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}