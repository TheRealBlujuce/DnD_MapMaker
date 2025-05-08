using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController2D : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 20f;

    [Header("Pan Settings")]
    public float panSpeed = 1f;

    private Camera cam;
    private Vector3 lastMousePosition;
    private PixelPerfectCamera pixelCam;


    void Start()
    {
        cam = Camera.main;
        pixelCam = cam.GetComponent<PixelPerfectCamera>();
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            // Convert scroll into an integer zoom step (+1 or -1)
            int zoomDelta = scroll > 0 ? -1 : 1;

            // Apply zoom step
            int newPPU = pixelCam.assetsPPU + zoomDelta * Mathf.RoundToInt(zoomSpeed);

            // Clamp between limits
            pixelCam.assetsPPU = Mathf.Clamp(newPPU, (int)minZoom, (int)maxZoom);
        }
    }


    void HandlePan()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            lastMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 currentMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = lastMousePosition - currentMousePosition;
            transform.position += delta;
        }
    }
}
