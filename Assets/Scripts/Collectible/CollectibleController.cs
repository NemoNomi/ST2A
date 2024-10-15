using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public bool isCollected = false;
    private Transform playerHolding;

    private Vector3 offset = new Vector3(0, 1f, 0); 

    public void Collect(Transform player)
    {
        isCollected = true;
        playerHolding = player;
        transform.position = player.position + offset;
    }

    public void Drop(Vector3 dropPosition)
    {
        isCollected = false;
        playerHolding = null;
        transform.position = dropPosition;
    }

    void Update()
    {
        if (isCollected && playerHolding != null)
        {
            transform.position = playerHolding.position + offset;
        }
    }
}
