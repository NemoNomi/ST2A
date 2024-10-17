using UnityEngine;

public class ErrorMessageController : MonoBehaviour
{
    #region Fields
    public GameObject errorCanvas;
    public float displayDuration = 2.0f;
    private float timer;
    private bool isDisplayingMessage;
    #endregion

    #region Initialization
    void Start()
    {
        if (errorCanvas != null)
        {
            errorCanvas.SetActive(false);
        }
        isDisplayingMessage = false;
    }
    #endregion

    #region Update Method
    void Update()
    {
        if (isDisplayingMessage)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                HideErrorMessage();
            }
        }
    }
    #endregion

    #region Error Message Handling
    public void ShowErrorMessage()
    {
        if (errorCanvas != null)
        {
            errorCanvas.SetActive(true);
            isDisplayingMessage = true;
            timer = displayDuration;
        }
    }

    void HideErrorMessage()
    {
        if (errorCanvas != null)
        {
            errorCanvas.SetActive(false);
            isDisplayingMessage = false;
        }
    }
    #endregion
}