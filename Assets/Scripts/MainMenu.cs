using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private SaveLoadUI saveLoadUI;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private Button continueButton;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void UpdateContinueButton()
    {
        if (continueButton == null)
        {
            Debug.LogWarning("ContinueButton is missing");
            return;
        }
        continueButton.interactable =
            saveSystem != null &&
            saveSystem.HasSaveFile();
    }

    public void ContinueGame()
    {
        if (saveSystem == null || !saveSystem.HasSaveFile())
        {
            Debug.LogWarning("No save file available.");
            UpdateContinueButton();
            return;
        }
        if (saveLoadUI == null)
        {
            Debug.LogWarning("SaveLoadUI is missing");
            return;
        }
        saveLoadUI.LoadGame();
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