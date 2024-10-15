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

    private float timer = 0f;
    private bool timerRunning = false;

    [Header("Storage Controllers")]
    public StorageController storage1;
    public StorageController storage2;

    [Header("Collectibles Settings")]
    public CollectibleController[] allCollectibles;

    [Header("Timer Settings")]
    public float timerStartDelay = 4f;

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
        StartCoroutine(StartTimerAfterDelay());
    }

    IEnumerator StartTimerAfterDelay()
    {
        yield return new WaitForSeconds(timerStartDelay);
        
        bool allActive = true;
        foreach (CollectibleController collectible in allCollectibles)
        {
            if (!collectible.gameObject.activeInHierarchy)
            {
                allActive = false;
                break;
            }
        }

        if (allActive)
        {
            StartTimer();
        }
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
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
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

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            player.DisableMovement();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public bool IsGameOver()
{
    return !timerRunning;
}

}
