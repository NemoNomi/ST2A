using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderMainMenu : MonoBehaviour
{
    [Header("Animator")]
    public Animator transition;

    public float transitionTime = 1.2f;

    public void OnPlayButton()
    {
        StartCoroutine(PlayTransitionAndLoadScene());
    }

    private IEnumerator PlayTransitionAndLoadScene()
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
