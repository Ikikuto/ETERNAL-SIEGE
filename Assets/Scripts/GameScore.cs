using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText; // Your score display text

    private int score;
    public int Score
    {
        get { return score; }
    }

    public void SetScore(int value)
    {
        score = Mathf.Max(value, 0);
        UpdateUI();
    }

    private void Start()
    {
        score = 0;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        if (amount <= 0) return;

        SetScore(score + amount);
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}