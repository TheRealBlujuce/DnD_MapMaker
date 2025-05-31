using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    private List<SelectableToken> selectedTokens = new List<SelectableToken>();
    private Dictionary<SelectableToken, Vector3> dragOffsets = new Dictionary<SelectableToken, Vector3>();
    private bool isDragging = false;
    private Camera mainCamera;
    public float gridSize = 1f;

    void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }
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
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (selectedTokens.Count == 0) return;

        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            ClearSelection();
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartDragging();
        }
        else if (Input.GetMouseButton(1) && isDragging)
        {
            DragTokens();
        }
        else if (Input.GetMouseButtonUp(1) && isDragging)
        {
            StopDragging();
        }
    }

    public void Select(SelectableToken token)
    {
        if (!selectedTokens.Contains(token))
        {
            selectedTokens.Add(token);
            token.Select();
        }
    }

    public void ToggleSelection(SelectableToken token)
    {
        if (selectedTokens.Contains(token))
        {
            selectedTokens.Remove(token);
            token.Deselect();
        }
        else
        {
            selectedTokens.Add(token);
            token.Select();
        }
    }

    public void ClearSelection()
    {
        foreach (var token in selectedTokens)
        {
            token.Deselect();
        }
        selectedTokens.Clear();
    }

    private void StartDragging()
    {
        isDragging = true;
        dragOffsets.Clear();

        Vector3 mouseWorld = GetMouseWorld();
        foreach (var token in selectedTokens)
        {
            dragOffsets[token] = token.transform.position - mouseWorld;
        }
    }

    private void DragTokens()
    {
        Vector3 mouseWorld = GetMouseWorld();
        foreach (var pair in dragOffsets)
        {
            var newPos = mouseWorld + pair.Value;
            pair.Key.transform.position = newPos;
        }
    }

    private void StopDragging()
    {
        isDragging = false;
        foreach (var token in selectedTokens)
        {
            SnapToGrid(token);
        }
    }

    private void SnapToGrid(SelectableToken token)
    {
        Vector3 pos = token.transform.position;
        float snappedX = Mathf.Round(pos.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(pos.y / gridSize) * gridSize;
        token.transform.position = new Vector3(snappedX, snappedY, pos.z);
    }

    private Vector3 GetMouseWorld()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 10f; // Make sure it's far enough to be in world view
        return mainCamera.ScreenToWorldPoint(screenPos);
    }
}
