using System.Collections;
using UnityEngine;

public class ObjectSlideIn : MonoBehaviour
{
    [Header("Objects from Left to Right")]
    public GameObject[] leftToRightObjects;

    [Header("Objects from Right to Left")]
    public GameObject[] rightToLeftObjects;

    [Header("Movement Settings")]
    public float slideDuration = 1.5f;
    public float offScreenOffset = 1000f;
    public float interObjectDelay = 0.3f;

    private Vector3[] leftToRightStartPositions;
    private Vector3[] rightToLeftStartPositions;

    void Start()
    {
        InitializeStartPositions();
        StartSlideInAnimations();
    }

    void InitializeStartPositions()
    {
        leftToRightStartPositions = new Vector3[leftToRightObjects.Length];
        for (int i = 0; i < leftToRightObjects.Length; i++)
        {
            GameObject obj = leftToRightObjects[i];
            leftToRightStartPositions[i] = obj.transform.position;
            obj.transform.position = new Vector3(-offScreenOffset, obj.transform.position.y, obj.transform.position.z);
        }

        rightToLeftStartPositions = new Vector3[rightToLeftObjects.Length];
        for (int i = 0; i < rightToLeftObjects.Length; i++)
        {
            GameObject obj = rightToLeftObjects[i];
            rightToLeftStartPositions[i] = obj.transform.position;
            obj.transform.position = new Vector3(Screen.width + offScreenOffset, obj.transform.position.y, obj.transform.position.z);
        }
    }

    void StartSlideInAnimations()
    {
        StartCoroutine(SlideObjectsWithDelay(leftToRightObjects, leftToRightStartPositions, slideDuration, interObjectDelay));
        StartCoroutine(SlideObjectsWithDelay(rightToLeftObjects, rightToLeftStartPositions, slideDuration, interObjectDelay));
    }

    IEnumerator SlideObjectsWithDelay(GameObject[] objects, Vector3[] targetPositions, float duration, float delay)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            StartCoroutine(SlideObject(objects[i], targetPositions[i], duration));
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator SlideObject(GameObject obj, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = obj.transform.position;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            yield return null;
        }

        obj.transform.position = targetPosition;
    }
}
