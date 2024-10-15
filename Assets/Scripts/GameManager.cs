using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText; // TextMeshPro für den Timer
    public TextMeshProUGUI finalTimeText; // TextMeshPro für die finale Zeit
    public GameObject gameOverPanel; // Spielende-Panel

    private float timer = 0f;
    private bool timerRunning = false;
    private bool isGameOver = false; // NEU: Variable für den Spielstatus

    [Header("Storage Controllers")]
    public StorageController storage1;
    public StorageController storage2;

    [Header("Collectibles Settings")]
    public CollectibleController[] allCollectibles; // Liste aller Collectibles

    [Header("Timer Settings")]
    public float collectibleStartDelay = 3f; // Verzögerung, bevor Collectibles erscheinen

    void Awake()
    {
        // Singleton-Pattern
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
        // Setze den Timer von Anfang an auf 00:00
        UpdateTimerUI();

        // Collectibles zunächst deaktivieren
        foreach (CollectibleController collectible in allCollectibles)
        {
            collectible.gameObject.SetActive(false);
        }

        // Timer und Collectibles nach der Verzögerung aktivieren
        StartCoroutine(ActivateCollectiblesAfterDelay());
    }

    IEnumerator ActivateCollectiblesAfterDelay()
    {
        // Warte die festgelegte Zeit
        yield return new WaitForSeconds(collectibleStartDelay);

        // Collectibles aktivieren
        foreach (CollectibleController collectible in allCollectibles)
        {
            collectible.gameObject.SetActive(true);
        }

        // Timer starten
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

    /// <summary>
    /// Überprüft, ob beide Speicher gefüllt sind.
    /// </summary>
    public void CheckGameStatus()
    {
        if (storage1.IsFilled && storage2.IsFilled)
        {
            EndGame();
        }
    }

    /// <summary>
    /// Beendet das Spiel und zeigt die finale Zeit an.
    /// </summary>
    void EndGame()
    {
        StopTimer();
        finalTimeText.text = "Zeit: " + timerText.text;
        gameOverPanel.SetActive(true);
        isGameOver = true; // NEU: Setze das Spiel als beendet

        // Spielerbewegung stoppen
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            player.DisableMovement();
        }
    }

    public bool IsGameOver() // NEU: Getter für den Spielstatus
    {
        return isGameOver;
    }

    // Optional: Neustart des Spiels
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
