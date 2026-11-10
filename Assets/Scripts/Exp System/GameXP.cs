using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameXP : MonoBehaviour
{
    [Header("XP UI")]
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Slider XPSlider;

    [Header("XP Settings")]
    [SerializeField, Min(1)] private int startingXPRequired = 100;
    [SerializeField, Min(1f)] private float xpGrowthMultiplier = 1.5f;

    public int XPCount { get; private set; }
    public int TotalXPEarned { get; private set; }
    public int CurrentLevel { get; private set; } = 1;
    public int MaxXP { get; private set; }
    public int BonusXPPercent { get; private set; }

    // Giữ event cũ để các script khác đang lắng nghe không bị ảnh hưởng.
    public event Action<int> OnLevelUp; //event thong bao cho cac he thong khac rang player level up!!

    // Lưu phần XP lẻ (đơn vị 1/100 XP), tránh mất bonus ở Enemy ít XP.
    private int xpRemainderHundredths;

    private void Awake()
    {
        // Awake giúp MaxXP sẵn sàng trước khi Enemy có thể cấp XP.
        MaxXP = Mathf.Max(1, startingXPRequired);
    }

    private void Start()
    {
        UpdateXPUI();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0)
            return;

        // VD: 5 XP với bonus 5% = 5.25 XP.
        // 5 XP được nhận ngay; 0.25 XP giữ để cộng dồn cho các lần sau.
        long scaledXP = (long)amount * (100L + BonusXPPercent)
                      + xpRemainderHundredths;
        long wholeXP = scaledXP / 100L;
        xpRemainderHundredths = (int)(scaledXP % 100L);

        // Giữ public API kiểu int như script gốc.
        int earnedXP = (int)Math.Min(wholeXP, int.MaxValue - (long)TotalXPEarned);
        if (earnedXP <= 0)
            return;

        XPCount += earnedXP;
        TotalXPEarned += earnedXP;

        // Một lần nhận XP có thể tăng nhiều level; phát event cho từng level.
        while (XPCount >= MaxXP)
        {
            XPCount -= MaxXP;
            LevelUp();
        }

        UpdateXPUI();
    }

    // Mỗi lần chọn Bonus XP sẽ tăng thêm 5 điểm phần trăm.
    public void AddXPBonus(int percentagePoints)
    {
        if (percentagePoints <= 0)
            return;

        BonusXPPercent += percentagePoints;
        Debug.Log($"Bonus XP: +{BonusXPPercent}%");
    }

    private void LevelUp()
    {
        CurrentLevel++;

        // Không cho yêu cầu XP trở thành 0.
        MaxXP = Mathf.Max(1, Mathf.RoundToInt(
            MaxXP * Mathf.Max(1f, xpGrowthMultiplier)));

        Debug.Log($"Level Up! Current Level: {CurrentLevel}");
        OnLevelUp?.Invoke(CurrentLevel);
    }

#if UNITY_EDITOR
    // Trong Play Mode: menu ⋮ của component -> DEBUG / Level Up Once.
    [ContextMenu("DEBUG/Level Up Once")]
    private void DebugLevelUpOnce()
    {
        AddXP(Mathf.Max(1, MaxXP - XPCount));
    }

    [ContextMenu("DEBUG/Add 1000 XP")]
    private void DebugAdd1000XP()
    {
        AddXP(1000);
    }
#endif

    private void UpdateXPUI()
    {
        if (XPSlider != null)
        {
            XPSlider.minValue = 0;
            XPSlider.maxValue = MaxXP;
            XPSlider.value = XPCount;
        }

        if (LevelText != null)
        {
            LevelText.text = $"Lv. {CurrentLevel} | XP: {XPCount}/{MaxXP}";
        }
    }


}
