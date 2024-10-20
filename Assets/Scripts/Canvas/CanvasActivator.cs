using System.Collections;
using UnityEngine;

public class CanvasActivator : MonoBehaviour
{
    [Header("Canvas to Activate")]
    public GameObject canvasToActivate;

    [Header("Activation Delay (seconds)")]
    public float delay = 5f;

    void Start()
    {
        if (canvasToActivate != null)
        {
            canvasToActivate.SetActive(false);
            StartCoroutine(ActivateCanvasAfterDelay());
        }
    }

    IEnumerator ActivateCanvasAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        canvasToActivate.SetActive(true);
    }
}
