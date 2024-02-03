using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("<b>Zoom : </b>")]
    [Space(1)]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoomIn = 5f;
    public float maxZoomOut = 10f;

    [Space(10)]
    [Header("<b>Edge : </b>")]
    [Space(1)]
    public float moveSpeed = 5f;
    public float edgeDetectionDistance = 10f;

    private bool isMaxZoomedIn = false;
    private float currentZoomLevel;

    private void Start()
    {
        currentZoomLevel = maxZoomOut;
    }

    void Update()
    {
        HandleZoom();
        HandleMove();
    }

    void HandleZoom()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        // Check for mouse wheel click
        if (Input.GetMouseButtonDown(2)) // Change to Input.GetMouseButtonDown(1) for right-click
        {
            // Toggle between max zoom in and max zoom out
            isMaxZoomedIn = !isMaxZoomedIn;

            // Set the target zoom level based on the toggle state
            float targetZoom = isMaxZoomedIn ? maxZoomIn : maxZoomOut;

            // Smoothly interpolate to the target zoom level
            StartCoroutine(SmoothZoom(targetZoom));
        }
        else
        {
            // If not clicking the mouse wheel, perform regular zoom
            ZoomCamera(scrollDelta);
        }
    }

    void HandleMove()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Check if the mouse is near the edges of the screen
        if (mousePosition.x < edgeDetectionDistance)
        {
            MoveCamera(Vector3.left);
        }
        else if (mousePosition.x > Screen.width - edgeDetectionDistance)
        {
            MoveCamera(Vector3.right);
        }

        if (mousePosition.y < edgeDetectionDistance)
        {
            MoveCamera(Vector3.down);
        }
        else if (mousePosition.y > Screen.height - edgeDetectionDistance)
        {
            MoveCamera(Vector3.up);
        }
    }

    void MoveCamera(Vector3 direction)
    {
        Camera.main.transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void ZoomCamera(float zoomDelta)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            float targetZoom = mainCamera.orthographicSize - zoomDelta * zoomSpeed * Time.deltaTime;

            // Clamp the targetZoom based on the zoom limits
            targetZoom = Mathf.Clamp(targetZoom, minZoom, isMaxZoomedIn ? maxZoomIn : maxZoomOut);

            if (mainCamera.orthographic)
            {
                mainCamera.orthographicSize = targetZoom;
            }
            else
            {
                mainCamera.fieldOfView = targetZoom;
            }
        }
    }


    IEnumerator SmoothZoom(float targetZoom)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            float startZoom = mainCamera.orthographic ? mainCamera.orthographicSize : mainCamera.fieldOfView;
            float timer = 0f;
            float duration = 0.5f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;

                if (mainCamera.orthographic)
                {
                    mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);
                }
                else
                {
                    mainCamera.fieldOfView = Mathf.Lerp(startZoom, targetZoom, t);
                }

                yield return null;
            }
        }
    }
}
