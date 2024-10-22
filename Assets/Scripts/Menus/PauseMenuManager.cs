using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Pause Menu Canvas")]
    [SerializeField] private GameObject pauseMenuCanvasGO;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject audioMenuPanel;
    [SerializeField] private GameObject controlsMenuPanel;

    [Header("Pause Button")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite defaultSprite;

    [Header("Animator Settings")]
    public Animator transition;
    public float transitionTime = 1.2f;

    private bool isPaused = false;
    private Image pauseButtonImage;

    private void Start()
    {
        InitializeMenus();

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(() => TogglePauseMenu());
            pauseButtonImage = pauseButton.GetComponent<Image>();
        }
    }

    private void Update()
    {
        HandlePauseInput();
    }

    private void InitializeMenus()
    {
        pauseMenuCanvasGO.SetActive(false);
        pauseMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        audioMenuPanel.SetActive(false);
        controlsMenuPanel.SetActive(false);
    }

    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !EventSystem.current.currentSelectedGameObject)
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        OpenPauseMenu();
        AudioManager.instance.PlayUI(AudioManager.instance.PauseMenuOpen);

        if (!audioMenuPanel.activeInHierarchy)
        {
            AudioManager.instance.SetLowpassFilter(500f);
        }

        if (pauseButtonImage != null && pauseSprite != null)
        {
            pauseButtonImage.sprite = pauseSprite;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        CloseAllMenus();
        AudioManager.instance.PlayUI(AudioManager.instance.PauseMenuClose);
        AudioManager.instance.SetLowpassFilter(22000f);

        if (pauseButtonImage != null && defaultSprite != null)
        {
            pauseButtonImage.sprite = defaultSprite;
        }
    }

    private void SetMenuVisibility(bool isVisible)
    {
        pauseMenuCanvasGO.SetActive(isVisible);
    }

    private void OpenPauseMenu()
    {
        SetMenuVisibility(true);
        pauseMenuPanel.SetActive(true);
        settingsMenuPanel.SetActive(false);
        audioMenuPanel.SetActive(false);
        controlsMenuPanel.SetActive(false);
    }

    private void OpenSettingsMenu()
    {
        pauseMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(true);
        audioMenuPanel.SetActive(false);
        controlsMenuPanel.SetActive(false);
    }

    private void OpenAudioMenu()
    {
        settingsMenuPanel.SetActive(false);
        audioMenuPanel.SetActive(true);
        controlsMenuPanel.SetActive(false);

        AudioManager.instance.SetLowpassFilter(22000f);
    }

    private void OpenControlsMenu()
    {
        settingsMenuPanel.SetActive(false);
        controlsMenuPanel.SetActive(true);
    }

    private void CloseAllMenus()
    {
        SetMenuVisibility(false);
        pauseMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        audioMenuPanel.SetActive(false);
        controlsMenuPanel.SetActive(false);
    }

    public void OnResumePress()
    {
        ResumeGame();
    }

    public void OnMainMenuPress()
    {
        Time.timeScale = 1f;
        StartCoroutine(PlayTransitionAndLoadScene(0));
    }

    public void RestartGame()
    {
        StartCoroutine(PlayTransitionAndLoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator PlayTransitionAndLoadScene(int sceneIndex)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }

    public void OnAudioPress()
    {
        OpenAudioMenu();
    }

    public void OnControlsPress()
    {
        OpenControlsMenu();
    }

    public void OnSettingsBackPress()
    {
        settingsMenuPanel.SetActive(false);
        OpenPauseMenu();

        AudioManager.instance.SetLowpassFilter(500f);
    }

    public void OnAudioBackPress()
    {
        audioMenuPanel.SetActive(false);
        OpenSettingsMenu();
        AudioManager.instance.SetLowpassFilter(500f);
    }

    public void OnControlsBackPress()
    {
        controlsMenuPanel.SetActive(false);
        OpenSettingsMenu();
    }
}
