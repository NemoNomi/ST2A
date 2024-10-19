using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Typewriter Effect Settings")]
    public TextMeshProUGUI textDisplay;
    public string fullText = "Dein Text hier...";
    public float delay = 0.1f;
    public Button nextSceneButton;

    private string currentText = "";

    [Header("Animator Settings")]
    public Animator transition;
    public float transitionTime = 1.2f;

    [Header("Audio Settings")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    void Start()
    {
        nextSceneButton.gameObject.SetActive(false);
        StartCoroutine(ShowTextWithDelay());
    }

    IEnumerator ShowTextWithDelay()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textDisplay.text = currentText;

            if (AudioManager.instance != null && i % 2 == 0)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.Typewriter);
            }

            yield return new WaitForSeconds(delay);
        }

        nextSceneButton.gameObject.SetActive(true);
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
}