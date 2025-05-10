using System.Collections;
using TMPro;
using UnityEngine;
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

    void Start()
    {
        layers = new GameObject[] {
            basementLayer,   // Index 0 - Shift+1
            defaultLayer,    // Index 1 - Shift+2
            firstFloorLayer, // Index 2 - Shift+3
            secondFloorLayer,// Index 3 - Shift+4
            thirdFloorLayer  // Index 4 - Shift+5
        };

        ActivateLayer(1); // Default layer on startup
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
}
