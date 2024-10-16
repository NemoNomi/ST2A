using UnityEngine;

public class StorageController : MonoBehaviour
{
    [Header("Storage Settings")]
    public int requiredCollectibles = 1;
    private int currentCollectibles = 0;
    public bool IsFilled { get; private set; } = false;

    private Color originalColor;
    private SpriteRenderer sr;
    private CollectibleController storedCollectible;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
        CheckIfFilled();
    }

    public void AddCollectible(CollectibleController collectible)
    {
        if (storedCollectible == null)
        {
            storedCollectible = collectible;
            currentCollectibles++;
            Debug.Log(gameObject.name + " hat jetzt " + currentCollectibles + " Collectibles.");
            CheckIfFilled();
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
}
