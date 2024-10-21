using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.2f;

    public void LoadMainMenu()
    {
        StartCoroutine(PlayTransitionAndLoadMainMenu());
    }

    private IEnumerator PlayTransitionAndLoadMainMenu()
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(0);
    }
}
