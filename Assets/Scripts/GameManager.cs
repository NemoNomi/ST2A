using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region UI Elements
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimeText;
    public GameObject gameOverPanel;

    public GameObject countdownCanvas;
    public TextMeshProUGUI countdownText; 
    #endregion

    #region Leaderboard Settings
    public LeaderboardManager leaderboardManager;
    #endregion

    private float timer = 0f;
    private bool timerRunning = false;
    private bool isGameOver = false;

    #region Storage and Collectibles
    public StorageController[] player1Storages;
    public StorageController[] player2Storages;
    public CollectibleController[] allCollectibles;
    #endregion

    public float collectibleStartDelay = 3f;
    public float countdownStartDelay = 1f;
    public float readyDisplayDuration = 1.5f;
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

        StartCoroutine(StartCountdownAndActivateCollectibles());
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    IEnumerator StartCountdownAndActivateCollectibles()
    {
        yield return new WaitForSeconds(countdownStartDelay);

        countdownCanvas.SetActive(true);

        countdownText.text = "Ready?";
        yield return new WaitForSeconds(readyDisplayDuration);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);

        countdownCanvas.SetActive(false);

        foreach (CollectibleController collectible in allCollectibles)
        {
            collectible.gameObject.SetActive(true);
        }

        StartTimer();
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
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
        float roundedTime = Mathf.Round(timer);

        finalTimeText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(roundedTime / 60F), Mathf.FloorToInt(roundedTime % 60F));

        gameOverPanel.SetActive(true);
        isGameOver = true;

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            player.DisableMovement();
        }

        Debug.Log("Final Timer (to be added to Leaderboard): " + roundedTime);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetFloat("RoundTime_" + sceneIndex, roundedTime);

        leaderboardManager.AddToLeaderboard(teamName, roundedTime);
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
