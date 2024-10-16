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
    public StorageController[] player1Storages;
    public StorageController[] player2Storages;

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

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        leaderboardManager.SetRoundKey("Round" + sceneIndex);

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
        if (AllStoragesFilled(player1Storages) && AllStoragesFilled(player2Storages))
        {
            EndGame();
        }
    }

    bool AllStoragesFilled(StorageController[] storages)
    {
        foreach (var storage in storages)
        {
            if (!storage.IsFilled)
            {
                return false;
            }
        }
        return true;
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

        Debug.Log("Final Timer (to be added to Leaderboard): " + timer);

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
