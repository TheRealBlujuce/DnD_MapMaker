using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InitiativeUIManager : MonoBehaviour
{
    [System.Serializable]
    public class InitiativeEntry
    {
        public Sprite icon;
        public int initiative;
        public string entryName;
        public Color color = Color.white;
    }

    public List<InitiativeEntry> playerEntries = new List<InitiativeEntry>();
    public List<InitiativeEntry> enemyEntries = new List<InitiativeEntry>();
    public List<InitiativeEntry> entries = new List<InitiativeEntry>();
    [SerializeField] private List<GameObject> spawnedEntries = new List<GameObject>();
    public GameObject entryPrefab;
    public GameObject initiativeHolder;
    public float spacing = 80f;
    public int visibleCount = 8;
    public float scrollSpeed = 5f;
    public float highlightScale = 1.2f;
    public float fadeAmount = 0.25f;

    
    private int selectedIndex = 0;
    private float scrollOffset = 0f;

    public RectTransform turnBanner; // assign in Inspector
    public TextMeshProUGUI turnBannerText;
    public float bannerDuration = 2f;
    public float bannerSlideTime = 0.5f;

    private float targetOffset = 0f;
    private bool isAnimating = false;
    private bool isScrolling = false;

    private bool isUpdating;


    void Start()
    {
        UpdateVisuals();
        
    }

    void Update()
    {
        HandleScroll();
        UpdateVisuals();
    }

    void HandleScroll()
    {
        if (isAnimating || isScrolling) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % entries.Count;
            StartCoroutine(ScrollAndShowBanner(entries[selectedIndex].entryName));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + entries.Count) % entries.Count;
            StartCoroutine(ScrollAndShowBanner(entries[selectedIndex].entryName));
        }
    }


    IEnumerator UpdateAllEntriesCoroutine(List<InitiativeEntry> newEntryList)
    {
        isUpdating = true;

        // Destroy all spawned entries
        for (int i = 0; i < spawnedEntries.Count; i++)
        {
            Destroy(spawnedEntries[i]);
            yield return null;
        }

        spawnedEntries.Clear(); //  Now safe to clear

        // Spawn new entries
        for (int i = 0; i < newEntryList.Count; i++)
        {
            InitiativeEntry data = newEntryList[i];
            GameObject go = Instantiate(entryPrefab, initiativeHolder.transform);
            go.name = $"Entry_{i}";

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.55f, 0.93f);
            rt.anchorMax = new Vector2(0.55f, 0.93f);
            rt.pivot = new Vector2(0.5f, 0.5f);

            Image icon = go.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI name = go.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            Image bg = icon;

            icon.sprite = data.icon;
            name.text = data.entryName;
            bg.color = data.color;

            if (!go.TryGetComponent(out CanvasGroup cg))
                cg = go.AddComponent<CanvasGroup>();

            spawnedEntries.Add(go);

            yield return null; // wait one frame between spawns
        }

        // Optionally reset scroll position and selected index if needed
        selectedIndex = 0;
        targetOffset = 0f;
        isUpdating = false;
        turnBanner.transform.SetAsLastSibling();
        yield return ShowTurnBanner(entries[selectedIndex].entryName);
    }


    IEnumerator ShowTurnBanner(string playerName)
    {
        isAnimating = true;

        // Set text
        turnBannerText.text = $"{playerName}'s Turn";

        // Slide in from left
        float elapsed = 0f;
        Vector2 start = new Vector2(-1920, turnBanner.anchoredPosition.y);
        Vector2 mid = new Vector2(0, turnBanner.anchoredPosition.y);
        turnBanner.anchoredPosition = start;

        while (elapsed < bannerSlideTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / bannerSlideTime);
            turnBanner.anchoredPosition = Vector2.Lerp(start, mid, t);
            yield return null;
        }

        // Wait for a few seconds
        yield return new WaitForSeconds(bannerDuration);

        // Slide out to right
        elapsed = 0f;
        Vector2 end = new Vector2(1920*2, turnBanner.anchoredPosition.y);

        while (elapsed < bannerSlideTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / bannerSlideTime);
            turnBanner.anchoredPosition = Vector2.Lerp(mid, end, t);
            yield return null;
        }

        isAnimating = false;
    }

    IEnumerator ScrollAndShowBanner(string playerName)
    {
        isScrolling = true;

        // Set new target offset
        targetOffset = Mathf.Max(0, selectedIndex - visibleCount / 2) * spacing;

        // Wait until scrollOffset is very close to targetOffset
        while (Mathf.Abs(scrollOffset - targetOffset) > 0.01f)
        {
            yield return null;
        }

        isScrolling = false;
        yield return StartCoroutine(ShowTurnBanner(playerName));
    }


    void UpdateVisuals()
    {
        if (!isUpdating)
        {
            scrollOffset = Mathf.Lerp(scrollOffset, targetOffset, Time.deltaTime * scrollSpeed);

            for (int i = 0; i < spawnedEntries.Count; i++)
            {
                GameObject go = spawnedEntries[i];
                RectTransform rt = go.GetComponent<RectTransform>();
                CanvasGroup cg = go.GetComponent<CanvasGroup>();

                float y = -i * spacing + scrollOffset;
                rt.anchoredPosition = new Vector2(0, y);

                bool isSelected = i == selectedIndex;
                float targetScale = isSelected ? highlightScale : 1f;
                rt.localScale = Vector3.Lerp(rt.localScale, Vector3.one * targetScale, Time.deltaTime * 10f);
            }
        }
    }

    public void UpdateInitiativeUI(List<InitiativeEntry> newEntries)
    {
        //Debug.Log("Updating Entries");
        StartCoroutine(UpdateAllEntriesCoroutine(newEntries));
    }
}
