using TMPro;
using UnityEngine;
using System;
using System.Globalization;

public class LeaderboardRow : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text achievedAtText;
    [SerializeField] private TMP_Text scoreText;

    public void SetData(int rank, LeaderboardEntry entry)
    {
        rankText.text = rank.ToString();

        if (DateTime.TryParse(
            entry.achievedAt,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind,
            out DateTime achievedAt))
        {
            achievedAtText.text = achievedAt.ToLocalTime()
                .ToString("dd/MM/yyyy HH:mm");
        }
        else
        {
            achievedAtText.text = entry.achievedAt;
        }

        scoreText.text = entry.score.ToString("N0");
    }
}