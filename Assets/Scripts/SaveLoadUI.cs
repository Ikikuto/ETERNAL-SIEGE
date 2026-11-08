using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private Transform player;
    // [SerializeField] private PlayerHealth playerHealth;
    // [SerializeField] private GameScore gameScore;
    [SerializeField] private GameManager gameManager;
    // [SerializeField] private PlayerInventory playerInventory;
    public void SaveGame()
    {
        // if (saveSystem == null ||
        // player == null || playerHealth == null ||
        // gameScore == null || playerInventory == null)
        // {
        //     Debug.LogWarning("Missing Save references");
        //     return;
        // }

        SaveData data = new()
        {
            playerX = player.position.x,
            playerY = player.position.y,
            // health = playerHealth.CurrentHealth,
            // score = gameScore.Score,
            // coinCount = playerInventory.CoinCount,
            // items = playerInventory.ExportItems()
        };

        saveSystem.SaveGame(data);
    }

    public void LoadGame()
    {
        // if (saveSystem == null || player == null ||
        // playerHealth == null || gameScore == null)
        // {
        //     Debug.LogWarning("Missing Load references");
        //     return;
        // }

        SaveData data = saveSystem.LoadGame();
        if (data == null)
        {
            Debug.LogWarning("No save file found");
            return;
        }

        player.position = new Vector3(data.playerX,
        data.playerY, 0
        );

        // playerHealth.SetHealth(data.health);
        // gameScore.SetScore(data.score);
        // playerInventory.SetCoinCount(data.coinCount);
        // playerInventory.ImportItems(data.items);

        if (data.health > 0 && gameManager != null)
        {
            // gameManager.ResumeGame();
        }

        Debug.Log("Game loaded successfully");
    }

}