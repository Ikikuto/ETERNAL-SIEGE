using UnityEngine;

public class GameSaveController : MonoBehaviour
{
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private Transform playerTransform;
    // [SerializeField] private PlayerHealth playerHealth;
    // [SerializeField] private GameScore gameScore;
    // [SerializeField] private PlayerInventory playerInventory;

    public void SaveGame()
    {
        SaveData data = new()
        {
            playerX = playerTransform.position.x,
            playerY = playerTransform.position.y,
            // health = playerHealth.CurrentHealth,
            // score = gameScore.Score,
            // items = playerInventory.ExportItems()
        };

        saveSystem.SaveGame(data);
    }

    public void LoadGame()
    {
        SaveData data = saveSystem.LoadGame();
        if (data == null)
        {
            Debug.Log("No save file found");
            return;
        }
        playerTransform.position = new Vector3(
        data.playerX,
        data.playerY,
        playerTransform.position.z
        );
        // playerHealth.SetHealth(data.health);
        // gameScore.SetScore(data.score);
        // playerInventory.ImportItems(data.items);
    }

}