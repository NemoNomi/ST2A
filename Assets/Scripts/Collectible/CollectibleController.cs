using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [Header("Activation Settings")]
    public float activationDelay = 3f;

    [HideInInspector]
    public bool isCollected = false;
    private Transform collector;

    void Start()
    {
        gameObject.SetActive(false);
        Invoke("ActivateCollectible", activationDelay);
    }

    void ActivateCollectible()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (isCollected && collector != null)
        {
            Vector3 newPosition = collector.position;
            newPosition.z = 0f;
            newPosition.y += 1f;
            transform.position = newPosition;
        }
    }

    public void Collect(Transform playerTransform)
    {
        if (!isCollected)
        {
            isCollected = true;
            collector = playerTransform;
            transform.SetParent(collector);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = Color.cyan;
            Debug.Log(gameObject.name + " gesammelt von " + collector.name);
        }
    }

    // Methode zum Absetzen des Collectibles
    public void Drop(Vector3 dropPosition)
    {
        if (isCollected)
        {
            isCollected = false;
            transform.SetParent(null);
            transform.position = dropPosition;
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            Debug.Log(gameObject.name + " abgelegt bei Position: " + dropPosition);
            collector = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
