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

        float centerX = (bottomLeft.x + topRight.x) / 2f;

        minXLeft = bottomLeft.x + buffer;
        maxXLeft = centerX - buffer;
        minYLeft = bottomLeft.y + buffer;
        maxYLeft = topRight.y - buffer;

        minXRight = centerX + buffer;
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
}
