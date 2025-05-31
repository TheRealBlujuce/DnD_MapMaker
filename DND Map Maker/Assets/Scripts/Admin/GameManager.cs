using UnityEngine;
using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject adminUI;
    private Camera mainCamera;
    private DistanceMeasure distanceTool;
    private ExpandingCircle circleTool;
    private TilemapLayerSwitcher tilemapLayerSwitcher;
    public enum ToolState
    {
        None,
        Line,
        Circle,
    }

    public ToolState CurrentTool { get; private set; } = ToolState.None;

    public event Action<ToolState> OnToolChanged;

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

        distanceTool = GetComponent<DistanceMeasure>();
        distanceTool.mainCamera = mainCamera;

        circleTool = GetComponent<ExpandingCircle>();
        circleTool.mainCamera = mainCamera;

        UpdateAdminUIState();

        Debug.Log("Main camera after scene load: " + mainCamera?.name);
    }

    private void Start()
    {
        UpdateAdminUIState();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

#region Set and Swap Admin Tools
    public void SetTool(ToolState newTool)
    {
        if (CurrentTool != newTool)
        {
            CurrentTool = newTool;
            Debug.Log($"Tool changed to: {CurrentTool}");
            OnToolChanged?.Invoke(CurrentTool);
        }
    }

    public void SwapCurrentTool()
    {
        if (Input.GetKey(KeyCode.LeftShift)) return;
        if (Input.GetKeyDown(KeyCode.Tilde)){ SetTool(ToolState.None);}
        if (Input.GetKeyDown(KeyCode.Alpha1)){ SetTool(ToolState.Line);}
        if (Input.GetKeyDown(KeyCode.Alpha2)){ SetTool(ToolState.Circle);}
    }

    public bool IsToolActive(ToolState tool)
    {
        return CurrentTool == tool;
    }

#endregion
    private void UpdateAdminUIState()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            //Debug.Log("I am the host/admin");
            adminUI.SetActive(true);
        }
        else
        {
            //Debug.Log("I am a client/player");
            adminUI.SetActive(false);
        }
    }
    private void Update()
    {
        SwapCurrentTool();
    }
}
