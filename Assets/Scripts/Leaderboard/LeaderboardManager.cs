using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public TextMeshProUGUI[] teamNameTexts;
    public TextMeshProUGUI[] timeTexts;

    [Header("Audio SFX")]
    AudioManager audioManager;

    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    private string currentRoundKey = "Round1";

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

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        LoadLeaderboard();
    }

    public void SetRoundKey(string roundKey)
    {
        currentRoundKey = roundKey;
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

        audioManager.PlaySFX(audioManager.Leaderboard);
    }

    public void UpdateLeaderboardUI()
    {
        for (int i = 0; i < teamNameTexts.Length; i++)
        {
            string placeNumber = (i + 1) + ") ";

            if (i < leaderboardEntries.Count)
            {
                teamNameTexts[i].text = placeNumber + leaderboardEntries[i].teamName;
                timeTexts[i].text = FormatTime(leaderboardEntries[i].time);
            }
            else
            {
                teamNameTexts[i].text = placeNumber;
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
            PlayerPrefs.SetString(currentRoundKey + "_TeamName" + i, leaderboardEntries[i].teamName);
            PlayerPrefs.SetFloat(currentRoundKey + "_TeamTime" + i, leaderboardEntries[i].time);
        }
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        leaderboardEntries.Clear();

        for (int i = 0; i < teamNameTexts.Length; i++)
        {
            if (PlayerPrefs.HasKey(currentRoundKey + "_TeamName" + i) && PlayerPrefs.HasKey(currentRoundKey + "_TeamTime" + i))
            {
                string teamName = PlayerPrefs.GetString(currentRoundKey + "_TeamName" + i);
                float time = PlayerPrefs.GetFloat(currentRoundKey + "_TeamTime" + i);
                leaderboardEntries.Add(new LeaderboardEntry(teamName, time));
            }
        }

        UpdateLeaderboardUI();
    }

    public void ResetLeaderboard()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.DeleteKey(currentRoundKey + "_TeamName" + i);
            PlayerPrefs.DeleteKey(currentRoundKey + "_TeamTime" + i);
        }
        leaderboardEntries.Clear();
        UpdateLeaderboardUI();
    }
}
