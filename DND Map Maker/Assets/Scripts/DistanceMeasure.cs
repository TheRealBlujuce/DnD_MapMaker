using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you're using TextMeshPro

public class DistanceMeasure : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Camera mainCamera;
    public TextMeshProUGUI distanceText; // Replace with `public Text distanceText;` if not using TMP

    private Vector3 startPoint;
    private bool isMeasuring = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift)) // Right mouse button pressed
        {
            startPoint = GetMouseWorldPosition();
            isMeasuring = true;
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint);
            distanceText.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift) && isMeasuring)
        {
            Vector3 currentPoint = GetMouseWorldPosition();
            lineRenderer.SetPosition(1, currentPoint);

            float distanceInUnits = Vector3.Distance(startPoint, currentPoint);
            int distanceInFeet = Mathf.RoundToInt(distanceInUnits) * 5;

            if (distanceText != null)
            {
                distanceText.text = distanceInFeet + " ft";
                Vector3 screenPos = Input.mousePosition;
                distanceText.transform.position = screenPos + new Vector3(10, -10); // Slight offset
            }
        }

        if (Input.GetMouseButtonUp(1) && !Input.GetKey(KeyCode.LeftShift) && isMeasuring)
        {
            lineRenderer.enabled = false;
            isMeasuring = false;
            distanceText.gameObject.SetActive(false);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Distance from camera (adjust if necessary)
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
