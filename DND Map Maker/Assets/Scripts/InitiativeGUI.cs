using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
            var rowObj = Instantiate(rowPrefab, rowContainer);
            var row = rowObj.GetComponent<CharacterInitiativeUI>();
            row.Initialize(player.entryName, player.initiative, player.icon);
            currentRows.Add(row);
        }
    }

    public void OnUpdateInitiativeButtonClicked()
    {
        List<InitiativeEntry> updatedPlayers = new List<InitiativeEntry>();

        foreach (var row in currentRows)
        {
            updatedPlayers.Add(new InitiativeEntry
            {
                entryName = row.GetPlayerName(),
                icon = row.GetPlayerIcon(),
                initiative = row.GetInitiativeValue()
            });
        }
        //Debug.Log("Button To Update Entries Clicked");
        initiativeUpdater.UpdateInitiativeOrder(updatedPlayers, initiativeUIManager.enemyEntries);
    }
}
