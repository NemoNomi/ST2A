using UnityEngine;

public class PlayerCollectibleHandler : MonoBehaviour
{
    private CollectibleController heldCollectible;
    private PlayerIdentifier playerIdentifier;
    public Transform[] storagePoints;

    void Start()
    {
        playerIdentifier = GetComponent<PlayerIdentifier>();
        if (playerIdentifier == null)
        {
            Debug.LogError("PlayerCollectibleHandler: Kein PlayerIdentifier gefunden.");
        }

        if (playerIdentifier.playerNumber == 1)
        {
            GameObject[] storageObjects = GameObject.FindGameObjectsWithTag("Storage_Player1");
            storagePoints = new Transform[storageObjects.Length];
            for (int i = 0; i < storageObjects.Length; i++)
            {
                storagePoints[i] = storageObjects[i].transform;
            }
        }
        else if (playerIdentifier.playerNumber == 2)
        {
            GameObject[] storageObjects = GameObject.FindGameObjectsWithTag("Storage_Player2");
            storagePoints = new Transform[storageObjects.Length];
            for (int i = 0; i < storageObjects.Length; i++)
            {
                storagePoints[i] = storageObjects[i].transform;
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver()) return;

        if (Input.GetKeyDown(GetInteractionKey()))
        {
            if (heldCollectible == null)
            {
                TryPickupCollectible();
            }
            else
            {
                DropCollectible();
            }
        }
    }

    KeyCode GetInteractionKey()
    {
        if (playerIdentifier.playerNumber == 1)
        {
            return KeyCode.Space;
        }
        else
        {
            return KeyCode.RightControl;
        }
    }

   void TryPickupCollectible()
{
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
    foreach (var hitCollider in hitColliders)
    {
        if (hitCollider.CompareTag("Collectible"))
        {
            CollectibleController collectible = hitCollider.GetComponent<CollectibleController>();
            if (collectible != null && !collectible.isCollected)
            {
                foreach (var storagePoint in storagePoints)
                {
                    float distance = Vector3.Distance(collectible.transform.position, storagePoint.position);
                    if (distance < 1.5f)
                    {
                        StorageController storage = storagePoint.GetComponent<StorageController>();
                        if (storage != null)
                        {
                            storage.RemoveCollectible(collectible);
                        }
                    }
                }

                heldCollectible = collectible;
                collectible.Collect(transform);
                break;
            }
        }
    }
}

void DropCollectible()
{
    if (heldCollectible != null)
    {
        Transform closestStorage = null;
        float closestDistance = Mathf.Infinity;

        foreach (var storagePoint in storagePoints)
        {
            float distance = Vector3.Distance(transform.position, storagePoint.position);
            if (distance < 1.5f && distance < closestDistance)
            {
                StorageController storage = storagePoint.GetComponent<StorageController>();
                if (storage != null && !storage.IsFilled)
                {
                    closestStorage = storagePoint;
                    closestDistance = distance;
                }
            }
        }

        if (closestStorage != null)
        {
            heldCollectible.Drop(closestStorage.position);
            StorageController storage = closestStorage.GetComponent<StorageController>();
            if (storage != null)
            {
                storage.AddCollectible(heldCollectible);
            }
        }
        else
        {
            heldCollectible.Drop(transform.position);
        }

        heldCollectible = null;
    }
}


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
