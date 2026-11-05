
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameXP : MonoBehaviour
{
    [Header("XP UI")]
    [SerializeField] private TextMeshProUGUI XPText;
    [SerializeField] private Slider XPSlider;

    [Header("XP Settings")]
    [SerializeField] private int startingXPRequired = 100;
    [SerializeField] private float xpGrowthMultiplier = 1.5f;

    public int XPCount { get; private set; }
    public int TotalXPEarned { get; private set; }
    public int CurrentLevel { get; private set; } = 1;
    public int MaxXP { get; private set; }

    public event Action<int> OnLevelUp;

    private void Start()
    {
        MaxXP = startingXPRequired;

        UpdateXPUI();
    }

    // Add XP when an enemy dies
    public void AddXP(int amount)
    {
        if (amount <= 0)
            return;

        XPCount += amount;
        TotalXPEarned += amount;

        // Handle multiple level-ups if XP overflows
        while (XPCount >= MaxXP)
        {
            XPCount -= MaxXP;

            LevelUp();
        }

        UpdateXPUI();
    }

    private void LevelUp()
    {
        CurrentLevel++;

        Debug.Log("Level Up! Current Level: " + CurrentLevel);

        // Increase XP required for the next level
        MaxXP = Mathf.RoundToInt(MaxXP * xpGrowthMultiplier);

        OnLevelUp?.Invoke(CurrentLevel);
    }

    private void UpdateXPUI()
    {
        if (XPSlider != null)
        {
            XPSlider.minValue = 0;
            XPSlider.maxValue = MaxXP;
            XPSlider.value = XPCount;
        }

        if (XPText != null)
        {
            XPText.text = "XP Earned: " + XPCount + "/" + MaxXP;
        }
    }
}
