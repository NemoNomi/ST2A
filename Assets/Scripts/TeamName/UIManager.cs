using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_InputField teamNameInput;

    public void StartGame()
    {
        string teamName = teamNameInput.text;

        if (!string.IsNullOrEmpty(teamName))
        {
            PlayerPrefs.SetString("TeamName", teamName);

            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogWarning("Bitte einen Teamnamen eingeben.");
        }
    }
}
