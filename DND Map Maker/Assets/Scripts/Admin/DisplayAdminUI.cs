using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayAdminUI : MonoBehaviour
{
    public RectTransform uiPanel; // Assign in Inspector
    public Button toggleButton;   // Assign in Inspector
    public float slideDuration = 0.25f;

    private bool isOpen = false;
    private Vector2 closedPosition;
    private Vector2 openPosition;
    private Coroutine currentCoroutine;

    void Start()
    {
        if (uiPanel == null || toggleButton == null)
        {
            //Debug.LogError("UI Panel or Button not assigned!");
            return;
        }

        closedPosition = uiPanel.anchoredPosition;
        openPosition = closedPosition + new Vector2(456f, 0f); // Slide to the left visually

    }

    public void ToggleUI()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        Vector2 targetPosition = isOpen ? closedPosition : openPosition;
        currentCoroutine = StartCoroutine(SmoothMove(uiPanel, targetPosition, slideDuration));

        isOpen = !isOpen;
    }

    private IEnumerator SmoothMove(RectTransform panel, Vector2 targetPos, float duration)
    {
        Vector2 startPos = panel.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            panel.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        panel.anchoredPosition = targetPos;
    }
}
