using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using static InitiativeUIManager;

public class InitiativeUpdater : MonoBehaviour
{
    public InitiativeUIManager initiativeUIManager;
    public List<InitiativeEntry> newEntries = new List<InitiativeEntry>();
    public TokenImageDatabase tokenImageDB;

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


    // Call this to update the initiative order
    public void UpdateInitiativeOrder(List<InitiativeEntry> players, List<InitiativeEntry> enemies)
    {
        newEntries.Clear();
        initiativeUIManager.entries.Clear();

        foreach (var player in players)
        {
            Sprite playerSprite = tokenImageDB.GetSpriteById(player.iconID);
            if (playerSprite == null)
                Debug.LogWarning($"Sprite not found for player '{player.entryName}' with ID {player.iconID}");

            newEntries.Add(new InitiativeEntry
            {
                entryName = player.entryName,
                initiative = player.initiative,
                entryIcon = playerSprite,
                color = player.color
            });
        }


        foreach (var enemy in enemies)
        {
            Sprite enemySprite = tokenImageDB.GetSpriteById(enemy.iconID);

            newEntries.Add(new InitiativeEntry
            {
                entryName = enemy.entryName,
                initiative = enemy.initiative,
                entryIcon = enemySprite,
                color = enemy.color
            });
        }

        newEntries.Sort((a, b) => b.initiative.CompareTo(a.initiative));

        // Send to all clients
        if (NetworkManager.Singleton.IsHost)
        {
            initiativeUIManager.entries = newEntries;
            initiativeUIManager.UpdateInitiativeUI(newEntries);
        }
    }



}
