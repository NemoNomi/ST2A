using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Team Name Input")]
    public TMP_InputField teamNameInput;
    public TextMeshProUGUI warningText;
    public Color errorColor = new Color(1, 0.7f, 0.7f);

    private const int maxLength = 10;

    private Color defaultInputFieldColor;
    private Coroutine warningCoroutine;

    [Header("Transition")]
    public Animator transition;
    public float transitionTime = 1.2f;

    void Start()
    {
        defaultInputFieldColor = teamNameInput.image.color;
        teamNameInput.onValueChanged.AddListener(OnInputValueChanged);
        warningText.text = "";
    }

    private void OnInputValueChanged(string input)
    {
        if (input.Length > maxLength)
        {
            ShowWarning($"Maximum {maxLength} characters allowed.");

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
            ShowWarning("Please enter a team name.");
            return;
        }

        if (teamName.Length > maxLength)
        {
            ShowWarning($"Maximum {maxLength} characters allowed.");
            return;
        }

        if (CheckIfTeamNameExists(teamName))
        {
            ShowWarning($"The team name '{teamName}' already exists. Please choose a different name.");
            return;
        }

        SaveTeamName(teamName);

        PlayerPrefs.SetString("TeamName", teamName);
        PlayerPrefs.Save();

        StartCoroutine(PlayTransitionAndLoadScene());
    }

    private bool CheckIfTeamNameExists(string teamName)
    {
        List<string> savedTeamNames = GetSavedTeamNames();

        return savedTeamNames.Contains(teamName);
    }

    private void SaveTeamName(string teamName)
    {
        List<string> savedTeamNames = GetSavedTeamNames();

        if (!savedTeamNames.Contains(teamName))
        {
            savedTeamNames.Add(teamName);
            PlayerPrefs.SetString("TeamNames", string.Join(",", savedTeamNames));
            PlayerPrefs.Save();
        }
    }

    private List<string> GetSavedTeamNames()
    {
        string savedNamesString = PlayerPrefs.GetString("TeamNames", "");
        List<string> savedTeamNames = new List<string>(savedNamesString.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
        return savedTeamNames;
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
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
