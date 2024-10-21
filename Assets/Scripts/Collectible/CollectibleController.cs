using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    #region Collectible Properties
    public bool isCollected = false;
    public string collectibleGroup;
    private Transform playerHolding;
    private Vector3 offset = new Vector3(0, 1f, 0);
    private Vector3 originalScale;

    #endregion

    void Start()
    {
        originalScale = transform.localScale;
    }

    #region Collect and Drop Methods
    public void Collect(Transform player)
    {
        isCollected = true;
        playerHolding = player;
        transform.position = player.position + offset;

        transform.localScale = originalScale * 0.8f;
    }

    public void Drop(Vector3 dropPosition)
    {
        isCollected = false;
        playerHolding = null;
        transform.position = dropPosition;

        transform.localScale = originalScale;
    }

    public void Drop(Vector3 dropPosition, StorageController storage)
    {
        if (storage.CanStoreCollectible(this))
        {
            isCollected = false;
            playerHolding = null;
            transform.position = dropPosition;

            transform.localScale = originalScale;

            storage.AddCollectible(this);
        }
        else
        {
            Debug.Log("Cannot drop the collectible here. The groups do not match.");
        }
    }
    #endregion

    #region Update Method
    void Update()
    {
        if (isCollected && playerHolding != null)
        {
            transform.position = playerHolding.position + offset;
        }
    }
    #endregion
}
