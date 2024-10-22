using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResetAllDataButton : MonoBehaviour
{
    public Button resetButton;
    private int resetClickCount = 0;
    private float resetTimer = 5f;
    private bool isResetTimerRunning = false;

    void Start()
    {
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    private void OnResetButtonClicked()
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
            ResetAllData();
            resetClickCount = 0;
            isResetTimerRunning = false;
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

    public void ResetAllData()
    {
        for (int i = 1; i <= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                PlayerPrefs.DeleteKey("Round" + i + "_TeamName" + j);
                PlayerPrefs.DeleteKey("Round" + i + "_TeamTime" + j);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey("Overall_TeamName" + i);
            PlayerPrefs.DeleteKey("Overall_TeamTime" + i);
        }

        PlayerPrefs.DeleteKey("TeamNames");

        PlayerPrefs.DeleteKey("LastTeamName");
        PlayerPrefs.DeleteKey("TotalTime");

        PlayerPrefs.Save();

        Debug.Log("Alle PlayerPrefs-Daten wurden erfolgreich gelöscht.");
    }
}
