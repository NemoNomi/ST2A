using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TotalTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI teamNameText;

    void Start()
    {
        float totalTime = 0f;

        string teamName = PlayerPrefs.GetString("TeamName", "Unbekanntes Team");

        for (int i = 1; i <= SceneManager.sceneCountInBuildSettings; i++)
        {
            if (PlayerPrefs.HasKey("RoundTime_" + i))
            {
                float roundTime = PlayerPrefs.GetFloat("RoundTime_" + i);
                totalTime += roundTime;
            }
        }

        int totalMinutes = Mathf.FloorToInt(totalTime / 60F);
        int totalSeconds = Mathf.FloorToInt(totalTime % 60F);

        totalTimeText.text = string.Format("Gesamtzeit: {0:00}:{1:00}", totalMinutes, totalSeconds);
        teamNameText.text = "Team: " + teamName;
    }
}
