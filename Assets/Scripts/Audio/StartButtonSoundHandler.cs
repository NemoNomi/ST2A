using UnityEngine;
using UnityEngine.UI;

public class StartButtonSoundHandler : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        if (AudioManager.instance != null && button != null)
        {
            button.onClick.AddListener(() => PlayButtonClickSound());
        }
    }

    private void PlayButtonClickSound()
    {
        AudioManager.instance.PlayUI(AudioManager.instance.UISubmit);
    }
}
