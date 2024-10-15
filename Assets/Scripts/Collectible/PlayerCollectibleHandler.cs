using UnityEngine;

public class PlayerCollectibleHandler : MonoBehaviour
{
    private CollectibleController heldCollectible;
    private PlayerIdentifier playerIdentifier;
    public Transform storagePoint;

    void Start()
    {
        playerIdentifier = GetComponent<PlayerIdentifier>();
        if (playerIdentifier == null)
        {
            Debug.LogError("PlayerCollectibleHandler: Kein PlayerIdentifier gefunden.");
        }

        if (playerIdentifier.playerNumber == 1)
        {
            GameObject storageObj = GameObject.Find("Storage_Player1");
            if (storageObj != null)
            {
                storagePoint = storageObj.transform;
            }
            else
            {
                Debug.LogError("Storage_Player1 nicht gefunden!");
            }
        }
        else if (playerIdentifier.playerNumber == 2)
        {
            GameObject storageObj = GameObject.Find("Storage_Player2");
            if (storageObj != null)
            {
                storagePoint = storageObj.transform;
            }
            else
            {
                Debug.LogError("Storage_Player2 nicht gefunden!");
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
                    if (Vector3.Distance(collectible.transform.position, storagePoint.position) < 1.5f)
                    {
                        StorageController storage = storagePoint.GetComponent<StorageController>();
                        if (storage != null)
                        {
                            storage.RemoveCollectible();
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
            Vector3 dropPosition = transform.position;

            float distance = Vector3.Distance(transform.position, storagePoint.position);
            if (distance < 1.5f)
            {
                dropPosition = storagePoint.position;
                heldCollectible.Drop(dropPosition);
                StorageController storage = storagePoint.GetComponent<StorageController>();
                if (storage != null)
                {
                    storage.AddCollectible();
                }
            }
            else
            {
                heldCollectible.Drop(dropPosition);
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
