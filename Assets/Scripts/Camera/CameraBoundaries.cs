using UnityEngine;
using UnityEngine.U2D;

public class CameraBoundaries : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public PixelPerfectCamera pixelPerfectCamera;

    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Boundary Settings")]
    public float buffer = 5f;

    public GameObject middleLinePlayer1;
    public GameObject middleLinePlayer2;

    private float minXLeft, maxXLeft, minYLeft, maxYLeft;
    private float minXRight, maxXRight, minYRight, maxYRight;

    private BoxCollider2D middleLineColliderPlayer1;
    private BoxCollider2D middleLineColliderPlayer2;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (pixelPerfectCamera == null)
        {
            pixelPerfectCamera = mainCamera.GetComponent<PixelPerfectCamera>();
            if (pixelPerfectCamera == null)
            {
                Debug.LogError("CameraBoundaries: PixelPerfectCamera-Komponente nicht an der Hauptkamera gefunden.");
                return;
            }
        }

        if (middleLinePlayer1 != null)
        {
            middleLineColliderPlayer1 = middleLinePlayer1.GetComponent<BoxCollider2D>();
            if (middleLineColliderPlayer1 == null)
            {
                Debug.LogError("Player 1 middle line does not have a BoxCollider2D component.");
            }
        }

        if (middleLinePlayer2 != null)
        {
            middleLineColliderPlayer2 = middleLinePlayer2.GetComponent<BoxCollider2D>();
            if (middleLineColliderPlayer2 == null)
            {
                Debug.LogError("Player 2 middle line does not have a BoxCollider2D component.");
            }
        }

        CalculateBounds();
    }

    void Update()
    {
        CalculateBounds();
        ClampPlayerPositions();
    }

    void CalculateBounds()
    {
        if (pixelPerfectCamera == null)
            return;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        float middleLineXPlayer1 = middleLinePlayer1.transform.position.x;
        float middleLineXPlayer2 = middleLinePlayer2.transform.position.x;

        minXLeft = bottomLeft.x + buffer;
        maxXLeft = middleLineXPlayer1 - buffer;
        minYLeft = bottomLeft.y + buffer;
        maxYLeft = topRight.y - buffer;

        minXRight = middleLineXPlayer2 + buffer;
        maxXRight = topRight.x - buffer;
        minYRight = bottomLeft.y + buffer;
        maxYRight = topRight.y - buffer;
    }

    void ClampPlayerPositions()
    {
        if (player1 != null)
        {
            Vector3 pos1 = player1.position;
            pos1.x = Mathf.Clamp(pos1.x, minXLeft, maxXLeft);
            pos1.y = Mathf.Clamp(pos1.y, minYLeft, maxYLeft);

            if (middleLineColliderPlayer1 != null && player1.GetComponent<Collider2D>().IsTouching(middleLineColliderPlayer1))
            {
                pos1.x = Mathf.Min(pos1.x, middleLinePlayer1.transform.position.x - buffer);
            }

            player1.position = pos1;
        }

        if (player2 != null)
        {
            Vector3 pos2 = player2.position;
            pos2.x = Mathf.Clamp(pos2.x, minXRight, maxXRight);
            pos2.y = Mathf.Clamp(pos2.y, minYRight, maxYRight);

            if (middleLineColliderPlayer2 != null && player2.GetComponent<Collider2D>().IsTouching(middleLineColliderPlayer2))
            {
                pos2.x = Mathf.Max(pos2.x, middleLinePlayer2.transform.position.x + buffer);
            }

            player2.position = pos2;
        }
    }
}
