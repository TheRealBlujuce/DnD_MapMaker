using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class TokenImageDatabase : MonoBehaviour
{
    // A dictionary to map iconId to the actual sprite
    private Dictionary<ulong, Sprite> spriteDatabase = new Dictionary<ulong, Sprite>();

    // The relative path to your sprites inside the Resources folder
    public string spritesFolder = "Sprites/Tokens/";

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
        StartCoroutine(DelayedLoad());
    }

    IEnumerator DelayedLoad()
    {
        yield return null; // wait one frame
        LoadSprites();
    }

    // Load all sprites into the dictionary
    private void LoadSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritesFolder);
        //Debug.Log($"Loaded {sprites.Length} sprites from path: {spritesFolder}");

        if (sprites.Length == 0)
        {
            //Debug.LogWarning("No sprites found in Resources path: " + spritesFolder);
        }

        foreach (var sprite in sprites)
        {
            if (sprite == null)
            {
                //Debug.LogWarning("Found null sprite while loading. Skipping...");
                continue;
            }

            ulong iconId = GenerateIdFromSprite(sprite);
            spriteDatabase[iconId] = sprite;
        }
    }


    // A method to generate a unique ID for each sprite (this can be customized)
    private ulong GenerateIdFromSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            //Debug.LogError("Tried to generate ID from null sprite!");
            return 0;
        }

        return (ulong)sprite.name.GetHashCode();
    }


    // Get a sprite by its ID
    public Sprite GetSpriteById(ulong iconId)
    {
        if (spriteDatabase.TryGetValue(iconId, out Sprite sprite))
        {
            //Debug.Log($"Sprite found for ID {iconId}: {sprite.name}");
            return sprite;
        }
        else
        {
            //Debug.LogWarning($"Sprite NOT found for ID: {iconId}");
            return null;
        }
    }

    // Get the ID of a sprite by its name (or another identifying feature)
    public ulong GetIdFromSprite(Sprite sprite)
    {
        // The ID is generated based on the sprite's name
        return GenerateIdFromSprite(sprite);
    }
}
