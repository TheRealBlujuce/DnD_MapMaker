using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class InitiativeUIManager : NetworkBehaviour
{
    [System.Serializable]
    public class InitiativeEntry : INetworkSerializable
    {
        public Sprite entryIcon;
        public string entryName;
        public int initiative;
        public Color color = Color.white;
        public ulong iconID;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref entryName);
            serializer.SerializeValue(ref initiative);
            serializer.SerializeValue(ref iconID);
        }
    }

    public List<InitiativeEntry> playerEntries = new List<InitiativeEntry>();
    public List<InitiativeEntry> enemyEntries = new List<InitiativeEntry>();
    public List<InitiativeEntry> entries = new List<InitiativeEntry>();
    [SerializeField] private List<GameObject> spawnedEntries = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedClientEntries = new List<GameObject>();

    public TokenImageDatabase tokenImageDB;
    public GameObject entryPrefab;
    public GameObject initiativeHolder;
    public GameObject clientInitiativeHolder;
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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        clientInitiativeHolder = FindGameObjectByName("Initiative Holder - Client");
        tokenImageDB = FindFirstObjectByType<TokenImageDatabase>();
        SetIconIDsForAllPlayers();
		
    }

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
        if (isAnimating || isScrolling || Input.GetKey(KeyCode.LeftShift)) return;

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

            // Get the sprite by the entry's iconID
            data.iconID = tokenImageDB.GetIdFromSprite(data.entryIcon);
            Sprite sprite = tokenImageDB.GetSpriteById(data.iconID);

            // Use the sprite in your UI (e.g., set it on an Image component)
            if (sprite != null)
            {
                icon.sprite = sprite;
            }
            else
            {
                //Debug.LogWarning($"Sprite not found for iconID: {data.iconID}");
            }


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

    IEnumerator UpdateAllClientEntriesCoroutine(List<InitiativeEntry> newEntryList)
    {
        isUpdating = true;

        // Destroy all spawned entries
        for (int i = 0; i < spawnedClientEntries.Count; i++)
        {
            Destroy(spawnedClientEntries[i]);
            yield return null;
        }

        spawnedClientEntries.Clear(); //  Now safe to clear

        // Spawn new entries
        for (int i = 0; i < newEntryList.Count; i++)
        {
            InitiativeEntry data = newEntryList[i];
            if (!entryPrefab)
            {
                //Debug.Log("Cant find an Entry Prefab :(");
                yield break;
            }
            GameObject go = Instantiate(entryPrefab, clientInitiativeHolder.transform);
            go.name = $"Entry_{i}";

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.55f, 0.93f);
            rt.anchorMax = new Vector2(0.55f, 0.93f);
            rt.pivot = new Vector2(0.5f, 0.5f);

            Image icon = go.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI name = go.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            Image bg = icon;

            // Get the sprite by the entry's iconID
            data.iconID = tokenImageDB.GetIdFromSprite(data.entryIcon);
            Sprite sprite = tokenImageDB.GetSpriteById(data.iconID);

            // Use the sprite in your UI (e.g., set it on an Image component)
            if (sprite != null)
            {
                icon.sprite = sprite;
            }
            else
            {
                //Debug.LogWarning($"Sprite not found for iconID: {data.iconID}");
            }


            name.text = data.entryName;
            bg.color = data.color;

            if (!go.TryGetComponent(out CanvasGroup cg))
                cg = go.AddComponent<CanvasGroup>();

            spawnedClientEntries.Add(go);

            yield return null; // wait one frame between spawns
        }

        // Optionally reset scroll position and selected index if needed
        selectedIndex = 0;
        targetOffset = 0f;
        isUpdating = false;
        //turnBanner.transform.SetAsLastSibling();
        //yield return ShowTurnBanner(entries[selectedIndex].entryName);
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

    public void SetIconIDsForAllPlayers()
    {
        foreach (var playerEntry in playerEntries)
        {
            // Assuming you have a method in your TokenImageDatabase to get the ID from a Sprite
            // Set the iconID using the sprite stored in the player entry.
            playerEntry.iconID = tokenImageDB.GetIdFromSprite(playerEntry.entryIcon);

            // Optionally, you can log the process to verify it's working.
            //Debug.Log($"Set iconID for {playerEntry.entryName}: {playerEntry.iconID}");
        }
    }


    // This is for the Admin
    public void UpdateInitiativeUI(List<InitiativeEntry> newEntries)
    {
        //Debug.Log("Updating Entries");
        StartCoroutine(UpdateAllEntriesCoroutine(newEntries));
        UpdateClientInitiativeUI(newEntries);
    }
    // This is for the Client
    public void UpdateClientInitiativeUI(List<InitiativeEntry> newEntries)
    {
        //Debug.Log("Updating Entries");
        StartCoroutine(UpdateAllClientEntriesCoroutine(newEntries));
    }
    GameObject FindGameObjectByName(string name)
    {
        GameObject[] allGameObjects = Object.FindObjectsOfType<GameObject>(true); // true allows for inactive objects to be included
        foreach (GameObject go in allGameObjects)
        {
            if (go.name == name)
            {
                return go;
            }
        }
        return null; // Return null if no object is found with the specified name
    }

	public void ClearAndRefreshInitiative()
	{
		// Clear all initiative-related lists
		
		enemyEntries.Clear();
		entries.Clear();

		// Optionally clear icon IDs if needed (probably not necessary)
		foreach (var go in spawnedEntries)
		{
			Destroy(go);
		}

		spawnedEntries.Clear();

		foreach (var go in spawnedClientEntries)
		{
			Destroy(go);
		}

		spawnedClientEntries.Clear();

		// Reset indices and scrolling
		selectedIndex = 0;
		scrollOffset = 0f;
		targetOffset = 0f;

		// Refresh visuals
		UpdateVisuals();
	}


}
