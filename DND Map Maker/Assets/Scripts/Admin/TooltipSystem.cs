using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem Instance;

    [Header("Tooltip UI Elements")]
    public GameObject tooltipObject;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;
    public LayoutElement layoutElement;
    public int characterWrapLimit = 80;

    private RectTransform tooltipRectTransform;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        tooltipRectTransform = tooltipObject.GetComponent<RectTransform>();
        Hide();
    }

    public void Show(string content, string header = "")
    {
        tooltipObject.SetActive(true);

        headerText.text = string.IsNullOrEmpty(header) ? "" : header;
        contentText.text = content;

        headerText.gameObject.SetActive(!string.IsNullOrEmpty(header));
        //layoutElement.enabled = headerText.text.Length > characterWrapLimit || contentText.text.Length > characterWrapLimit;
    }

    public void Hide()
    {
        tooltipObject.SetActive(false);
    }
}
