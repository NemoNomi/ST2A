using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Team Name Input")]
    public TMP_InputField teamNameInput;
    public TextMeshProUGUI warningText;
    public Color errorColor = new Color(1, 0.7f, 0.7f);

    private const int minLength = 1;
    private const int maxLength = 15;

    private Color defaultInputFieldColor;
    private Coroutine warningCoroutine;

    [Header("Transition")]
    public Animator transition;
    public float transitionTime = 1.2f;

    void Start()
    {
        defaultInputFieldColor = teamNameInput.image.color;
        teamNameInput.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnInputValueChanged(string input)
    {
        if (input.Length > maxLength)
        {
            ShowWarning($"Maximal {maxLength} Zeichen erlaubt!");

            teamNameInput.text = input.Substring(0, maxLength);

            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
            }
            warningCoroutine = StartCoroutine(HideWarningAfterDelay(2f));
        }
    }

    public void StartGame()
    {
        string teamName = teamNameInput.text;

        if (string.IsNullOrEmpty(teamName))
        {
            ShowWarning("Bitte gib einen Teamnamen ein.");
            return;
        }

        if (teamName.Length > maxLength)
        {
            ShowWarning($"Maximal {maxLength} Zeichen erlaubt.");
            return;
        }

        PlayerPrefs.SetString("TeamName", teamName);

        // Starte den Szenenwechsel mit Animation
        StartCoroutine(PlayTransitionAndLoadScene());
    }

    private void ShowWarning(string message)
    {
        warningText.text = message;
        teamNameInput.image.color = errorColor;
    }

    public void OnInputFieldSelected()
    {
        warningText.text = "";
        teamNameInput.image.color = defaultInputFieldColor;
    }

    private IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningText.text = "";
        teamNameInput.image.color = defaultInputFieldColor;
    }

    private IEnumerator PlayTransitionAndLoadScene()
    {
        // Spielt die Szenenübergangs-Animation ab
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        // Warten, bis die Animation fertig ist
        yield return new WaitForSeconds(transitionTime);

        // Lade die nächste Szene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
