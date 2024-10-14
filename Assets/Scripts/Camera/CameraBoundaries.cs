using UnityEngine;
using UnityEngine.U2D;

public class CameraBoundaries : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public PixelPerfectCamera pixelPerfectCamera;

    [Header("Players")]
    public Transform[] players;

    [Header("Boundary Settings")]
    public float buffer = 5f;

    private float minX, maxX, minY, maxY;

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

        minX = bottomLeft.x + buffer;
        maxX = topRight.x - buffer;
        minY = bottomLeft.y + buffer;
        maxY = topRight.y - buffer;
    }

    void ClampPlayerPositions()
    {
        foreach (Transform player in players)
        {
            if (player == null) continue;

            Vector3 pos = player.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            player.position = pos;
        }
    }
}
