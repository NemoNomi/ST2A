using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSoundHandler : MonoBehaviour, IPointerEnterHandler
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
        AudioManager.instance.PlayUI(AudioManager.instance.UISelect);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayUI(AudioManager.instance.UIHover);
    }
}
