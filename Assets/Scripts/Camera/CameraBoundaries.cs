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

    private BoxCollider2D[] middleLineCollidersPlayer1;
    private BoxCollider2D[] middleLineCollidersPlayer2;

    private float minXLeft, maxXLeft, minYLeft, maxYLeft;
    private float minXRight, maxXRight, minYRight, maxYRight;

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
            middleLineCollidersPlayer1 = middleLinePlayer1.GetComponentsInChildren<BoxCollider2D>();
            if (middleLineCollidersPlayer1.Length == 0)
            {
                Debug.LogError("Player 1 middle line has no BoxCollider2D components.");
            }
        }

        if (middleLinePlayer2 != null)
        {
            middleLineCollidersPlayer2 = middleLinePlayer2.GetComponentsInChildren<BoxCollider2D>();
            if (middleLineCollidersPlayer2.Length == 0)
            {
                Debug.LogError("Player 2 middle line has no BoxCollider2D components.");
            }
        }

        CalculateCameraBounds();
    }

    void Update()
    {
        CalculateCameraBounds();
        ClampPlayerPositionsCamera();
        ClampPlayerPositionsCollisions();
    }

    // --------- KAMERA BOUNDARIES ----------
    void CalculateCameraBounds()
    {
        if (pixelPerfectCamera == null)
            return;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        minXLeft = bottomLeft.x + buffer;
        maxXLeft = topRight.x / 2f - buffer;
        minYLeft = bottomLeft.y + buffer;
        maxYLeft = topRight.y - buffer;

        minXRight = topRight.x / 2f + buffer;
        maxXRight = topRight.x - buffer;
        minYRight = bottomLeft.y + buffer;
        maxYRight = topRight.y - buffer;
    }

    void ClampPlayerPositionsCamera()
    {
        if (player1 != null)
        {
            Vector3 pos1 = player1.position;
            pos1.x = Mathf.Clamp(pos1.x, minXLeft, maxXLeft);
            pos1.y = Mathf.Clamp(pos1.y, minYLeft, maxYLeft);
            player1.position = pos1;
        }

        if (player2 != null)
        {
            Vector3 pos2 = player2.position;
            pos2.x = Mathf.Clamp(pos2.x, minXRight, maxXRight);
            pos2.y = Mathf.Clamp(pos2.y, minYRight, maxYRight);
            player2.position = pos2;
        }
    }

    // --------- PHYSISCHE KOLLISION MIT MIDDLE LINE ----------
    void ClampPlayerPositionsCollisions()
    {
        if (player1 != null)
        {
            Vector3 pos1 = player1.position;

            foreach (var collider in middleLineCollidersPlayer1)
            {
                if (collider != null && player1.GetComponent<Collider2D>().IsTouching(collider))
                {
                    pos1.x = Mathf.Min(pos1.x, collider.bounds.min.x - buffer);
                }
            }

            player1.position = pos1;
        }

        if (player2 != null)
        {
            Vector3 pos2 = player2.position;

            foreach (var collider in middleLineCollidersPlayer2)
            {
                if (collider != null && player2.GetComponent<Collider2D>().IsTouching(collider))
                {
                    pos2.x = Mathf.Max(pos2.x, collider.bounds.max.x + buffer);
                }
            }

            player2.position = pos2;
        }
    }
}
