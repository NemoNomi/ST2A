using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class OverallLeaderboardManager : MonoBehaviour
{
    public TextMeshProUGUI[] teamNameTexts;
    public TextMeshProUGUI[] timeTexts;

    private List<LeaderboardEntry> overallLeaderboardEntries = new List<LeaderboardEntry>();

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

    private int resetClickCount = 0;
    private float resetTimer = 5f;
    private bool isResetTimerRunning = false;

    void Start()
    {
        LoadOverallLeaderboard();
        AddLastGameToOverallLeaderboard();
        UpdateOverallLeaderboardUI();
    }

    public void AddLastGameToOverallLeaderboard()
    {
        string lastTeamName = PlayerPrefs.GetString("LastTeamName", "Unknown Team");
        float totalTime = PlayerPrefs.GetFloat("TotalTime", 0f);

        if (totalTime > 0)
        {
            AddToOverallLeaderboard(lastTeamName, totalTime);
        }
    }

    public void AddToOverallLeaderboard(string teamName, float time)
    {
        overallLeaderboardEntries.Add(new LeaderboardEntry(teamName, time));

        overallLeaderboardEntries.Sort((x, y) => x.time.CompareTo(y.time));

        if (overallLeaderboardEntries.Count > 10)
        {
            overallLeaderboardEntries = overallLeaderboardEntries.GetRange(0, 10);
        }

        SaveOverallLeaderboard();
        UpdateOverallLeaderboardUI();
    }

    private void UpdateOverallLeaderboardUI()
    {
        for (int i = 0; i < teamNameTexts.Length; i++)
        {
            string placeNumber = (i + 1) + ") ";

            if (i < overallLeaderboardEntries.Count)
            {
                teamNameTexts[i].text = placeNumber + overallLeaderboardEntries[i].teamName;
                timeTexts[i].text = FormatTime(overallLeaderboardEntries[i].time);
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

    private void SaveOverallLeaderboard()
    {
        for (int i = 0; i < overallLeaderboardEntries.Count; i++)
        {
            PlayerPrefs.SetString("Overall_TeamName" + i, overallLeaderboardEntries[i].teamName);
            PlayerPrefs.SetFloat("Overall_TeamTime" + i, overallLeaderboardEntries[i].time);
        }
        PlayerPrefs.Save();
    }

    private void LoadOverallLeaderboard()
    {
        overallLeaderboardEntries.Clear();

        for (int i = 0; i < teamNameTexts.Length; i++)
        {
            if (PlayerPrefs.HasKey("Overall_TeamName" + i) && PlayerPrefs.HasKey("Overall_TeamTime" + i))
            {
                string teamName = PlayerPrefs.GetString("Overall_TeamName" + i);
                float time = PlayerPrefs.GetFloat("Overall_TeamTime" + i);
                overallLeaderboardEntries.Add(new LeaderboardEntry(teamName, time));
            }
        }
    }

    public void ResetOverallLeaderboard()
    {
        if (!isResetTimerRunning)
        {
            StartCoroutine(ResetButtonSequence());
        }
        else
        {
            resetClickCount++;
        }

        if (resetClickCount >= 3)
        {
            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.DeleteKey("Overall_TeamName" + i);
                PlayerPrefs.DeleteKey("Overall_TeamTime" + i);
            }

            overallLeaderboardEntries.Clear();
            UpdateOverallLeaderboardUI();

            resetClickCount = 0;
        }
    }

    private IEnumerator ResetButtonSequence()
    {
        resetClickCount = 1;
        isResetTimerRunning = true;

        yield return new WaitForSeconds(resetTimer);

        if (resetClickCount < 3)
        {
            resetClickCount = 0;
        }

        isResetTimerRunning = false;
    }
}
