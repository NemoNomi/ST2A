using UnityEngine;
using TMPro;

public class TeamNameManager : MonoBehaviour
{
    public TextMeshProUGUI teamNameText;

    void Start()
    {
        string teamName = PlayerPrefs.GetString("TeamName", "Unknown Team");

        teamNameText.text = "Team: " + teamName;
    }
}