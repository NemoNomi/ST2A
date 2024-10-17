using UnityEngine;

public class StorageController : MonoBehaviour
{
    #region Storage Settings
    public int requiredCollectibles = 1;
    private int currentCollectibles = 0;
    public bool IsFilled { get; private set; } = false;
    #endregion

    #region Visuals and Collectible Reference
    private Color originalColor;
    private SpriteRenderer sr;
    private CollectibleController storedCollectible;
    #endregion

    #region Group-based Restriction
    public string allowedGroup;
    #endregion

    #region Initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
        CheckIfFilled();
    }
    #endregion

    #region Collectible Management
    public bool CanStoreCollectible(CollectibleController collectible)
    {
        return collectible.collectibleGroup == allowedGroup;
    }

    public void AddCollectible(CollectibleController collectible)
    {
        if (CanStoreCollectible(collectible))
        {
            if (storedCollectible == null)
            {
                storedCollectible = collectible;
                currentCollectibles++;
                Debug.Log(gameObject.name + " hat jetzt " + currentCollectibles + " Collectibles.");
                CheckIfFilled();
            }
        }
        else
        {
            Debug.Log("This collectible doesn't belong to the allowed group for this storage.");
        }
    }

    public void RemoveCollectible(CollectibleController collectible)
    {
        if (storedCollectible == collectible)
        {
            storedCollectible = null;
            currentCollectibles--;
            Debug.Log(gameObject.name + " hat jetzt " + currentCollectibles + " Collectibles.");
            CheckIfFilled();
        }
    }
    #endregion

    #region Status and Visual Updates
    void CheckIfFilled()
    {
        if (currentCollectibles >= requiredCollectibles)
        {
            IsFilled = true;
            SetStorageColor(Color.green);
        }
        else
        {
            IsFilled = false;
            SetStorageColor(originalColor);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckGameStatus();
        }
        else
        {
            Debug.LogError("GameManager Instance nicht gefunden!");
        }
    }

    void SetStorageColor(Color color)
    {
        if (sr != null)
        {
            sr.color = color;
        }
    }
    #endregion
}
