using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public TextMeshProUGUI[] teamNameTexts;
    public TextMeshProUGUI[] timeTexts;
    
    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string teamName;
        public float time;

        public LeaderboardEntry(string name, float t)
        {
            teamName = name;
            time = t;
        }
    }

    void Start()
    {
        LoadLeaderboard();
    }

    public void AddToLeaderboard(string teamName, float time)
    {
        LeaderboardEntry existingEntry = leaderboardEntries.Find(entry => entry.teamName == teamName);

        if (existingEntry != null)
        {
            if (time < existingEntry.time)
            {
                existingEntry.time = time;
            }
        }
        else
        {
            leaderboardEntries.Add(new LeaderboardEntry(teamName, time));
        }

        leaderboardEntries.Sort((x, y) => x.time.CompareTo(y.time));

        if (leaderboardEntries.Count > 5)
        {
            leaderboardEntries = leaderboardEntries.GetRange(0, 5);
        }

        SaveLeaderboard();

        UpdateLeaderboardUI();
    }

    private void UpdateLeaderboardUI()
    {
        for (int i = 0; i < teamNameTexts.Length; i++)
        {
            if (i < leaderboardEntries.Count)
            {
                teamNameTexts[i].text = leaderboardEntries[i].teamName;
                timeTexts[i].text = FormatTime(leaderboardEntries[i].time);
            }
            else
            {
                teamNameTexts[i].text = "";
                timeTexts[i].text = "";
            }
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void SaveLeaderboard()
    {
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            PlayerPrefs.SetString("TeamName" + i, leaderboardEntries[i].teamName);
            PlayerPrefs.SetFloat("TeamTime" + i, leaderboardEntries[i].time);
        }
    }

    private void LoadLeaderboard()
    {
        leaderboardEntries.Clear();

        for (int i = 0; i < teamNameTexts.Length; i++)
        {
            if (PlayerPrefs.HasKey("TeamName" + i) && PlayerPrefs.HasKey("TeamTime" + i))
            {
                string teamName = PlayerPrefs.GetString("TeamName" + i);
                float time = PlayerPrefs.GetFloat("TeamTime" + i);
                leaderboardEntries.Add(new LeaderboardEntry(teamName, time));
            }
        }

        UpdateLeaderboardUI();
    }

    public void ResetLeaderboard()
    {
        PlayerPrefs.DeleteAll();
        leaderboardEntries.Clear();
        UpdateLeaderboardUI();
    }
}
