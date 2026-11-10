
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]
    private string apiBaseUrl = "http://localhost:9999/api";

    public event Action<LeaderboardResponse> LeaderboardLoaded;
    public event Action<string> LeaderboardFailed;

    // GET: Load leaderboard
    public void LoadLeaderboard()
    {
        StartCoroutine(GetLeaderboard());
    }

    private IEnumerator GetLeaderboard()
    {
        string url = apiBaseUrl + "/leaderboard";

        using UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Leaderboard request failed: " + request.error);
            LeaderboardFailed?.Invoke(request.error);
            yield break;
        }

        string json = request.downloadHandler.text;

        LeaderboardResponse response =
            JsonUtility.FromJson<LeaderboardResponse>(json);

        if (response == null || response.attempts == null)
        {
            LeaderboardFailed?.Invoke("Invalid leaderboard response.");
            yield break;
        }

        foreach (LeaderboardEntry entry in response.attempts)
        {
            Debug.Log(entry.achievedAt + ": " + entry.score);
        }

        LeaderboardLoaded?.Invoke(response);
    }

    // POST: Submit a new score
    private IEnumerator PostScore(int score)
    {
        string url = apiBaseUrl + "/scores";

        ScoreRequest scoreRequest = new()
        {
            achievedAt = DateTime.UtcNow.ToString("o"),
            score = score
        };

        string json = JsonUtility.ToJson(scoreRequest);
        byte[] body = Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request = new(url, "POST");

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Save score failed: " + request.error);
            LeaderboardFailed?.Invoke(request.error);
            yield break;
        }

        Debug.Log(request.downloadHandler.text);
        Debug.Log("Score saved successfully!");
    }

    public void SendScore(int score)
    {
        StartCoroutine(PostScore(score));
    }
}
