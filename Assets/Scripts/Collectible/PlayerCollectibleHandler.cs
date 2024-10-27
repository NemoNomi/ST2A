using UnityEngine;

public class PlayerCollectibleHandler : MonoBehaviour
{
    private CollectibleController heldCollectible;
    private PlayerIdentifier playerIdentifier;
    public Transform[] storagePoints;
    public ErrorMessageController errorMessageController;

    [Header("Audio SFX")]
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        playerIdentifier = GetComponent<PlayerIdentifier>();

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
    else if (playerIdentifier.playerNumber == 2)
    {
        if (Input.GetKey(KeyCode.RightControl))
        {
            return KeyCode.RightControl;
        }
        else if (Input.GetKey(KeyCode.RightCommand))
        {
            return KeyCode.RightCommand;
        }
        else if (Input.GetKey(KeyCode.RightAlt))
        {
            return KeyCode.RightAlt;
        }
    }

    return KeyCode.None;
}



    void TryPickupCollectible()
    {
        if (heldCollectible != null)
        {
            return;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.7f);
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
                        if (distance < 0.7f)
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
            bool isStandingOnInvalidStorage = false;

            foreach (var storagePoint in storagePoints)
            {
                float distance = Vector3.Distance(transform.position, storagePoint.position);
                if (distance < 0.7f)
                {
                    StorageController storage = storagePoint.GetComponent<StorageController>();

                    if (storage != null && !storage.IsFilled)
                    {
                        if (storage.CanStoreCollectible(heldCollectible))
                        {
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestStorage = storagePoint;
                            }
                        }
                        else
                        {
                            isStandingOnInvalidStorage = true;
                        }
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
                    audioManager.PlaySFX(audioManager.StorageDrop);
                }
                heldCollectible = null;
            }
            else if (isStandingOnInvalidStorage)
            {
                if (errorMessageController != null)
                {
                    errorMessageController.ShowErrorMessage();
                }
            }
            else
            {
                heldCollectible.Drop(transform.position);
                heldCollectible = null;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
}
