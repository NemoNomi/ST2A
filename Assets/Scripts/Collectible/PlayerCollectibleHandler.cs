using UnityEngine;

public class PlayerCollectibleHandler : MonoBehaviour
{
    #region Fields
    private CollectibleController heldCollectible;
    private PlayerIdentifier playerIdentifier;
    public Transform[] storagePoints;
    public ErrorMessageController errorMessageController;

    [Header("Audio SFX")]
    private AudioManager audioManager;
    
    #endregion

private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    #region Initialization
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
    #endregion

    #region Update
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
    #endregion

    #region Pickup and Drop Logic
    void TryPickupCollectible()
    {
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
                Debug.Log("Das Collectible kann hier nicht abgelegt werden. Die Gruppen stimmen nicht überein.");
                if (errorMessageController != null)
                {
                    errorMessageController.ShowErrorMessage();
                }
            }
            else
            {
                heldCollectible.Drop(transform.position);
                Debug.Log("Collectible in der Welt abgelegt.");
                heldCollectible = null;
            }
        }
    }
    #endregion

    #region Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
    #endregion
}