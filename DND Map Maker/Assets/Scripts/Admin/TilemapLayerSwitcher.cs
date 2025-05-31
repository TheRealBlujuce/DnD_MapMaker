using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapLayerSwitcher : MonoBehaviour
{

    [Header("Assign Your Tilemap Layers")]
    public GameObject basementLayer;
    public GameObject defaultLayer;
    public GameObject firstFloorLayer;
    public GameObject secondFloorLayer;
    public GameObject thirdFloorLayer;

    [Header("UI Elements")]
    public TextMeshProUGUI layerNameText;  // Assign a UI Text (or TextMeshProUGUI if using TMP)
    public float fadeDuration = 0.5f;
    public float displayDuration = 1.5f;

    private GameObject[] layers;
    private string[] layerNames = { "Basement", "Ground Floor", "First Floor", "Second Floor", "Third Floor" };
    private Coroutine fadeCoroutine;

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
        AssignTilemapLayers();

        layers = new GameObject[] {
            basementLayer,   // Index 0 - Shift+1
            defaultLayer,    // Index 1 - Shift+2
            firstFloorLayer, // Index 2 - Shift+3
            secondFloorLayer,// Index 3 - Shift+4
            thirdFloorLayer  // Index 4 - Shift+5
        };
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ActivateLayer(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ActivateLayer(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) ActivateLayer(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) ActivateLayer(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) ActivateLayer(4);
        }
    }

    void ActivateLayer(int index)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i] != null)
                layers[i].SetActive(i == index);
        }

        if (layerNameText != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeLayerName(layerNames[index]));
        }
    }

    IEnumerator FadeLayerName(string name)
    {
        layerNameText.text = name;

        Color color = layerNameText.color;
        color.a = 0;
        layerNameText.color = color;

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            layerNameText.color = color;
            yield return null;
        }

        // Hold
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            layerNameText.color = color;
            yield return null;
        }
    }

    #region Get the tilemap layers
    public void AssignTilemapLayers()
    {
        basementLayer = FindLayer("Basement Layer");
        defaultLayer = FindLayer("Ground Layer");
        firstFloorLayer = FindLayer("Indoor Layer");
        secondFloorLayer = FindLayer("Second Floor Layer");
        thirdFloorLayer = FindLayer("Third Floor Layer");

    }

    private GameObject FindLayer(string layerName)
    {
        GameObject found = GameObject.Find(layerName);
        if (found == null)
        {
            found = FindInactiveObjectByName(layerName);
            if (found == null)
            {
                Debug.LogWarning($"Layer \"{layerName}\" not found in the scene (active or inactive).");
            }
        }
        return found;
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name && obj.hideFlags == HideFlags.None && obj.scene.IsValid())
                return obj;
        }
        return null;
    }


    #endregion
}
