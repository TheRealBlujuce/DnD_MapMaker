using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static InitiativeUIManager;

public class InitiativeGUI : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform rowContainer;
    public InitiativeUIManager initiativeUIManager;
    public InitiativeUpdater initiativeUpdater; // Reference to the updater script
    private List<CharacterInitiativeUI> currentRows = new List<CharacterInitiativeUI>();

    private List<InitiativeEntry> players = new List<InitiativeEntry>();
    private List<InitiativeEntry> enemies = new List<InitiativeEntry>();
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
        tokenImageDB = FindFirstObjectByType<TokenImageDatabase>();
    }


    private void Start()
    {
        SetPlayers();
    }

    public void SetPlayers()
    {
        Debug.Log("SetPlayers called");
        players = initiativeUIManager.playerEntries;
        BuildRows();
    }

    private void BuildRows()
    {
        // Clear old rows
        foreach (Transform child in rowContainer)
            Destroy(child.gameObject);

        currentRows.Clear();

        // Create new rows
        foreach (var player in players)
        {
            // This gets the sprite from iconID each time
            Sprite playerSprite = tokenImageDB.GetSpriteById(player.iconID);

            var rowObj = Instantiate(rowPrefab, rowContainer);
            var row = rowObj.GetComponent<CharacterInitiativeUI>();

            row.Initialize(player.entryName, player.initiative, playerSprite);
            currentRows.Add(row);
        }

    }

    public void OnUpdateInitiativeButtonClicked()
    {
        List<InitiativeEntry> updatedPlayers = new List<InitiativeEntry>();

        foreach (var row in currentRows)
        {
            Sprite icon = row.GetPlayerIcon();

            ulong iconID = 0;
            if (icon != null)
            {
                iconID = tokenImageDB.GetIdFromSprite(icon);
                //Debug.LogWarning($"Got ID#: {iconID}  For {row.GetPlayerName()}");
            }
            else
            {
                //Debug.LogWarning($"Null sprite for {row.GetPlayerName()} — can't assign iconID.");
            }

            updatedPlayers.Add(new InitiativeEntry
            {
                entryName = row.GetPlayerName(),
                entryIcon = icon,
                initiative = row.GetInitiativeValue(),
                iconID = iconID
            });
        }


        initiativeUpdater.UpdateInitiativeOrder(updatedPlayers, initiativeUIManager.enemyEntries);
    }

}
