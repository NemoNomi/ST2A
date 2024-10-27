using UnityEngine;
using TMPro;
using System.Collections;

public class TotalTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI textBlock1;
    public TextMeshProUGUI textBlock2;
    public TextMeshProUGUI textBlock3;
    public GameObject finalCanvas;

    public float typewriterSpeed = 0.05f;
    public float delayBeforeFirstBlock = 1f;
    public float delayBetweenBlocks = 1.5f;
    public float delayBeforeCanvasActivation = 1.0f;

    void Start()
{
    float totalTime = 0f;
    string teamName = PlayerPrefs.GetString("TeamName", "Unbekanntes Team");

    for (int i = 1; i <= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
    {
        if (PlayerPrefs.HasKey(teamName + "_RoundTime_" + i))
        {
            float roundTime = PlayerPrefs.GetFloat(teamName + "_RoundTime_" + i);
            totalTime += roundTime;
        }
    }

    int totalMinutes = Mathf.FloorToInt(totalTime / 60F);
    int totalSeconds = Mathf.FloorToInt(totalTime % 60F);
    string totalFormattedTime = string.Format("{0:00}:{1:00}", totalMinutes, totalSeconds);

    PlayerPrefs.SetFloat("TotalTime", totalTime);
    PlayerPrefs.SetString("LastTeamName", teamName);
    PlayerPrefs.Save();

    finalCanvas.SetActive(false);

    StartCoroutine(TypeWriterEffect(teamName, totalFormattedTime));
}


    IEnumerator TypeWriterEffect(string teamName, string totalTime)
    {
        yield return new WaitForSeconds(delayBeforeFirstBlock);

        string text1 = $"Congrats {teamName},";
        yield return StartCoroutine(ShowTextWithTypewriterEffect(textBlock1, text1));

        yield return new WaitForSeconds(delayBetweenBlocks);

        string text2 = $"You needed {totalTime} to defragment the hard drive. The PC now runs faster.";
        yield return StartCoroutine(ShowTextWithTypewriterEffect(textBlock2, text2));

        yield return new WaitForSeconds(delayBetweenBlocks);

        string text3 = "Do you think you are amongst the fastest employees?";
        yield return StartCoroutine(ShowTextWithTypewriterEffect(textBlock3, text3));

        yield return new WaitForSeconds(delayBeforeCanvasActivation);

        finalCanvas.SetActive(true);
    }

    IEnumerator ShowTextWithTypewriterEffect(TextMeshProUGUI textBlock, string textToShow)
    {
        textBlock.text = "";
        for (int i = 0; i < textToShow.Length; i++)
        {
            textBlock.text += textToShow[i];
            yield return new WaitForSeconds(typewriterSpeed);
        }
    }
}
