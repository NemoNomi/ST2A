using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimeText;
    public GameObject gameOverPanel;

    [Header("Leaderboard Settings")]
    public LeaderboardManager leaderboardManager;

    private float timer = 0f;
    private bool timerRunning = false;
    private bool isGameOver = false;
    [Header("Storage Controllers")]
    public StorageController storage1;
    public StorageController storage2;

    [Header("Collectibles Settings")]
    public CollectibleController[] allCollectibles;

    [Header("Timer Settings")]
    public float collectibleStartDelay = 3f;

    private string teamName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        teamName = PlayerPrefs.GetString("TeamName", "No Team");

        UpdateTimerUI();

        foreach (CollectibleController collectible in allCollectibles)
        {
            collectible.gameObject.SetActive(false);
        }

        StartCoroutine(ActivateCollectiblesAfterDelay());
    }

    IEnumerator ActivateCollectiblesAfterDelay()
    {
        yield return new WaitForSeconds(collectibleStartDelay);

        foreach (CollectibleController collectible in allCollectibles)
        {
            collectible.gameObject.SetActive(true);
        }

        StartTimer();
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        timerRunning = true;
        Debug.Log("Timer gestartet.");
    }

    public void StopTimer()
    {
        timerRunning = false;
        Debug.Log("Timer gestoppt.");
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void CheckGameStatus()
    {
        if (storage1.IsFilled && storage2.IsFilled)
        {
            EndGame();
        }
    }


    void EndGame()
    {
        StopTimer();
        finalTimeText.text = "Zeit: " + timerText.text;
        gameOverPanel.SetActive(true);
        isGameOver = true;

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            player.DisableMovement();
        }

        leaderboardManager.AddToLeaderboard(teamName, timer);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
