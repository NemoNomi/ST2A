using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Typewriter Effect Settings")]
    public TextMeshProUGUI textDisplay;

    private string part1Text = "Welcome to switch it up!\n\nYou’re Eco and Tech, two dedicated workers on a computer’s hard drive. \n\nYour task? Keep things organized and running smoothly. Over time, as files are stored and deleted, pieces of data get scattered across the drive, slowing the system and the workers down. \n\nYour job is to collect these fragments and put them back in the right place to speed things up again.";

    private string part2Text = "Each of you controls one side of the workspace, but sometimes the fragments that belong on your side are on your partner’s side.\n\nUse the central exchange area to swap fragments, and work together to restore order. \n\nThe faster you are, the better. The most efficient teams will be recognized among all computer workers!\n\nGet ready to team up, think fast, and switch it up!";

    public float delay = 0.05f;

    public Button nextButton;
    public Button nextSceneButton;

    public GameObject additionalCanvas;
    public GameObject secondCanvas;

    private string currentText = "";
    private bool skipTypewriter = false;

    [Header("Animator Settings")]
    public Animator transition;
    public float transitionTime = 1.2f;

    void Start()
    {
        nextButton.gameObject.SetActive(false);
        nextSceneButton.gameObject.SetActive(false);

        additionalCanvas.SetActive(false);
        secondCanvas.SetActive(false);

        StartCoroutine(ShowTextWithDelay(part1Text, () =>
        {
            nextButton.gameObject.SetActive(true);

            additionalCanvas.SetActive(true);
        }));
    }

    IEnumerator ShowTextWithDelay(string textToShow, System.Action onComplete = null)
    {
        currentText = "";
        textDisplay.text = "";

        for (int i = 0; i <= textToShow.Length; i++)
        {
            if (skipTypewriter)
            {
                textDisplay.text = textToShow;
                onComplete?.Invoke();
                yield break;
            }

            currentText = textToShow.Substring(0, i);
            textDisplay.text = currentText;
            yield return new WaitForSeconds(delay);
        }

        onComplete?.Invoke();
    }

    public void OnNextButtonClicked()
    {
        nextButton.gameObject.SetActive(false);
        skipTypewriter = false;

        additionalCanvas.SetActive(false);

        StartCoroutine(ShowTextWithDelay(part2Text, () =>
        {
            nextSceneButton.gameObject.SetActive(true);

            secondCanvas.SetActive(true);
        }));
    }

    public void LoadNextScene()
    {
        StartCoroutine(PlayTransitionAndLoadScene());
    }

    private IEnumerator PlayTransitionAndLoadScene()
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnTextClicked()
    {
        skipTypewriter = true;
    }
}
