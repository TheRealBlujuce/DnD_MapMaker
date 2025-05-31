using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static InitiativeUIManager;

public class GhostTokenPlacer : MonoBehaviour
{
    [Header("Token Settings")]
    public GameObject tokenPrefab;
    public Sprite icon;
    public ulong iconId;
    public string baseName = "Enemy";
    public Color color = Color.white;
    public CharacterSize characterSize = CharacterSize.Medium;

    [Header("Refs")]
    public Camera mainCamera;
    public LayerMask placementLayer;
    public InitiativeUIManager initiativeUIManager;

    public static Dictionary<string, HashSet<string>> usedSuffixes = new();
    private TokenImageDatabase tokenImageDB;

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
        tokenImageDB = FindFirstObjectByType<TokenImageDatabase>();
    }

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            TryPlace();
        }
    }

    void FollowMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // Ensure it's at the correct Z depth for 2D

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, placementLayer);

        transform.position = mouseWorldPos; // Optional: let ghost follow cursor even if not over collider
        
    }

    void TryPlace()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 placementPos = mouseWorldPos;

        //GameObject tokenGO = Instantiate(tokenPrefab, placementPos, Quaternion.identity);
        
        if (!usedSuffixes.ContainsKey(baseName))
            usedSuffixes[baseName] = new HashSet<string>();

        string suffix = GetNextAvailableSuffix(baseName);
        usedSuffixes[baseName].Add(suffix);
        string fullName = $"{baseName} {suffix}";

        InitiativeEntry entry = new InitiativeEntry
        {
            entryIcon = icon,
            iconID = iconId,
            initiative = Random.Range(1, 21),
            entryName = fullName,
            color = color
        };
        initiativeUIManager.enemyEntries.Add(entry);

        SpawnTokenServerRpc(placementPos, iconId, true, characterSize, fullName);

        //tokenGO.name = fullName;
        //tokenGO.GetComponent<TokenSettings>().tokenIcon = icon;
        //tokenGO.GetComponent<TokenSettings>().iconID = iconId;
        //tokenGO.GetComponent<TokenSettings>().tokenIsEnemy = true;
        //tokenGO.GetComponent<TokenSettings>().characterSize = characterSize;


        //Debug.Log($"Spawned {fullName} at {placementPos}");

        Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnTokenServerRpc(Vector3 position, ulong iconId, bool isEnemy, CharacterSize size, string fullName)
    {
        GameObject token = Instantiate(tokenPrefab, position, Quaternion.identity);
        //token.GetComponent<NetworkObject>().Spawn();

        token.GetComponent<TokenSettings>().Initialize(iconId, isEnemy, size, fullName);
    }


    string GetNextAvailableSuffix(string baseName)
    {
        int index = 1;
        while (true)
        {
            string suffix = GetLetterSuffix(index);
            if (!usedSuffixes[baseName].Contains(suffix))
                return suffix;
            index++;
        }
    }


    string GetLetterSuffix(int index)
    {
        index--;
        string result = "";
        while (index >= 0)
        {
            result = (char)('A' + (index % 26)) + result;
            index = index / 26 - 1;
        }
        return result;
    }
}
