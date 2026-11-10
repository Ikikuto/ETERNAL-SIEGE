using System.Collections.Generic;
using UnityEngine;

public class LevelUpPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameXP gameXP;
    [SerializeField] private TurretUpgradeManager upgradeManager;
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private UpgradeCardUI[] cards = new UpgradeCardUI[3];

    public bool IsOpen { get; private set; }

    private int pendingChoices;
    private float previousTimeScale = 1f;

    private void Awake()
    {
        if (panelRoot != null && panelRoot != gameObject)
            panelRoot.SetActive(false);
    }

    private void OnEnable()
    {
        if (gameXP == null)
            gameXP = FindFirstObjectByType<GameXP>();

        if (gameXP != null)
            gameXP.OnLevelUp += HandleLevelUp;
        else
            Debug.LogError("LevelUpPanelUI: Assign GameXP in Inspector.");
    }

    private void OnDisable()
    {
        if (gameXP != null)
            gameXP.OnLevelUp -= HandleLevelUp;

        if (IsOpen)
        {
            Time.timeScale = previousTimeScale;
            IsOpen = false;
        }
    }

    private void HandleLevelUp(int newLevel)
    {
        pendingChoices++;

        // Khi tăng nhiều level cùng lúc, xếp hàng đủ số lần chọn.
        if (!IsOpen)
            ShowNextChoice();
    }

    private void ShowNextChoice()
    {
        if (!ReferencesAreValid())
            return;

        if (!IsOpen)
        {
            previousTimeScale = Time.timeScale;
            panelRoot.SetActive(true);
            IsOpen = true;
            Time.timeScale = 0f;
        }

        List<UpgradeType> pool = new()
        {
            UpgradeType.SharpBullet,
            UpgradeType.RapidFire,
            UpgradeType.LongRange,
            UpgradeType.TurretRepair,
            UpgradeType.BonusXp
        };

        // Partial Fisher-Yates: 3 kết quả ngẫu nhiên, không trùng loại.
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(i, pool.Count);
            UpgradeType temp = pool[i];
            pool[i] = pool[randomIndex];
            pool[randomIndex] = temp;
            cards[i].Setup(pool[i], OnCardSelected);
        }
    }

    private void OnCardSelected(UpgradeType selectedType)
    {
        if (!IsOpen || pendingChoices <= 0)
            return;

        upgradeManager.ApplyUpgrade(selectedType);
        pendingChoices--;

        if (pendingChoices > 0)
        {
            ShowNextChoice();
        }
        else
        {
            panelRoot.SetActive(false);
            IsOpen = false;
            Time.timeScale = previousTimeScale;
        }
    }

    private bool ReferencesAreValid()
    {
        if (panelRoot == null || panelRoot == gameObject || upgradeManager == null ||
            cards == null || cards.Length < 3)
        {
            Debug.LogError("LevelUpPanelUI: Check Panel Root, Upgrade Manager, and 3 Cards in Inspector.");
            return false;
        }

        for (int i = 0; i < 3; i++)
        {
            if (cards[i] == null)
            {
                Debug.LogError($"LevelUpPanelUI: Card index {i} is missing.");
                return false;
            }
        }

        return true;
    }

    
}
