using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    public void Setup(UpgradeType type, Action<UpgradeType> onSelected)
    {
        switch (type)
        {
            case UpgradeType.SharpBullet:
                SetContent("SHARP BULLET", "+5 Turret Damage");
                break;
            case UpgradeType.RapidFire:
                SetContent("RAPID FIRE", "+5 Attack Speed (+5%)");
                break;
            case UpgradeType.LongRange:
                SetContent("LONG RANGE", "+5 Turret Attack Range");
                break;
            case UpgradeType.TurretRepair:
                SetContent("TURRET REPAIR", "Heal 20% Max HP of living turrets");
                break;
            case UpgradeType.BonusXp:
                SetContent("BONUS XP", "+5% XP gained from enemies");
                break;
        }

        if (button == null)
        {
            Debug.LogError($"{name}: UpgradeCardUI is missing Button reference.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onSelected?.Invoke(type));
    }

    private void SetContent(string title, string description)
    {
        if (titleText != null)
            titleText.text = title;

        if (descriptionText != null)
            descriptionText.text = description;
    }
}
