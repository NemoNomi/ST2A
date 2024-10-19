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

    [Header("First Selected Options")]
    [SerializeField] private GameObject mainMenuFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    [SerializeField] private GameObject audioSettingsFirstSelected;
    [SerializeField] private GameObject controlsSettingsFirstSelected;

    #endregion

    private void Start()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        SetPanelVisibility(mainMenuPanel);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
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
        EventSystem.current.SetSelectedGameObject(settingsMenuFirstSelected);
    }

    private void OpenAudioSettingsMenu()
    {
        SetPanelVisibility(audioSettingsMenuPanel);
        EventSystem.current.SetSelectedGameObject(audioSettingsFirstSelected);
    }

    private void OpenControlsSettingsMenu()
    {
        SetPanelVisibility(controlsSettingsMenuPanel);
        EventSystem.current.SetSelectedGameObject(controlsSettingsFirstSelected);
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
