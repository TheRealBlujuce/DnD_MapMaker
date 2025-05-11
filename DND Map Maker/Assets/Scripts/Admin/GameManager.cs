using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum ToolState
    {
        None,
        Line,
        Circle,
    }

    public ToolState CurrentTool { get; private set; } = ToolState.None;

    public event Action<ToolState> OnToolChanged;

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
        if (Input.GetKeyDown(KeyCode.Alpha1)){ SetTool(ToolState.Line);}
        if (Input.GetKeyDown(KeyCode.Alpha2)){ SetTool(ToolState.Circle);}
    }

    public bool IsToolActive(ToolState tool)
    {
        return CurrentTool == tool;
    }

    private void Update()
    {
        SwapCurrentTool();
    }
}
