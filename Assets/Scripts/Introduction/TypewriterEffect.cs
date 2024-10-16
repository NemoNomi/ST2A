using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string fullText = "Dein Text hier...";
    public float delay = 0.1f;
    public AudioSource typingSound;
    public Button nextSceneButton;

    private string currentText = "";

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

            if (typingSound != null && i % 2 == 0)
            {
                typingSound.pitch = Random.Range(minPitch, maxPitch);
                typingSound.Play();
            }

            yield return new WaitForSeconds(delay);
        }

        nextSceneButton.gameObject.SetActive(true);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
