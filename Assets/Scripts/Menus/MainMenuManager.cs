using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject audioSettingsMenuPanel;
    [SerializeField] private GameObject controlsSettingsMenuPanel;
    #endregion

    private void Start()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        SetPanelVisibility(mainMenuPanel);
    }

    private void SetPanelVisibility(GameObject activePanel)
    {
        mainMenuPanel.SetActive(activePanel == mainMenuPanel);
        settingsMenuPanel.SetActive(activePanel == settingsMenuPanel);
        audioSettingsMenuPanel.SetActive(activePanel == audioSettingsMenuPanel);
        controlsSettingsMenuPanel.SetActive(activePanel == controlsSettingsMenuPanel);
    }

    #region Menu Navigation
    private void OpenSettingsMenu()
    {
        mainMenuPanel.SetActive(true);
        SetPanelVisibility(settingsMenuPanel);
    }

    private void OpenAudioSettingsMenu()
    {
        SetPanelVisibility(audioSettingsMenuPanel);
    }

    private void OpenControlsSettingsMenu()
    {
        SetPanelVisibility(controlsSettingsMenuPanel);
    }
    #endregion

    #region UI Button Handlers
    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }

    public void OnAudioSettingsPress()
    {
        OpenAudioSettingsMenu();
    }

    public void OnControlsSettingsPress()
    {
        OpenControlsSettingsMenu();
    }

    public void OnSettingsBackPress()
    {
        ShowMainMenu();
    }

    public void OnAudioSettingsBackPress()
    {
        OpenSettingsMenu();
    }

    public void OnControlsSettingsBackPress()
    {
        OpenSettingsMenu();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
    #endregion
}
