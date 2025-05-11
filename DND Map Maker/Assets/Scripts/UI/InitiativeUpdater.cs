using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InitiativeUIManager;

public class InitiativeUpdater : MonoBehaviour
{
    public InitiativeUIManager initiativeUIManager;
    public List<InitiativeEntry> newEntries = new List<InitiativeEntry>();

    // Call this to update the initiative order
    public void UpdateInitiativeOrder(List<InitiativeEntry> players, List<InitiativeEntry> enemies)
    {
        //Debug.Log("Sorting New Entries");
        newEntries.Clear();
        initiativeUIManager.entries.Clear();

        foreach (var player in players)
        {
            InitiativeEntry playerEntry = new InitiativeEntry
            {
                entryName = player.entryName,
                initiative = player.initiative,
                icon = player.icon,
                // Add other fields if needed
            };
            newEntries.Add(playerEntry);
        }

        if (enemies.Count != 0)
        {
            foreach (var enemy in enemies)
            {
                InitiativeEntry enemyEntry = new InitiativeEntry
                {
                    entryName = enemy.entryName,
                    initiative = enemy.initiative,
                    icon = enemy.icon,
                    // Add other fields if needed
                };
                newEntries.Add(enemyEntry);
            }
        }

        newEntries.Sort((a, b) => b.initiative.CompareTo(a.initiative));

        // Update InitiativeUIManager
        Debug.Log($"Updating Initiative. Players: {players.Count}, Enemies: {enemies.Count}");

        initiativeUIManager.entries = newEntries;
        initiativeUIManager.UpdateInitiativeUI(newEntries); // Optional method to refresh the list
    }
}
