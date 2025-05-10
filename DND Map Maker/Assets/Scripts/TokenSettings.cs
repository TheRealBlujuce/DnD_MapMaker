using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static InitiativeUIManager;

public class TokenSettings : MonoBehaviour
{

    public float tokenSize = 1f;
    public float targetTokenSize;
    public CharacterSize characterSize = CharacterSize.Medium;
    public Sprite tokenIcon;
    public float tokenGrowthSpeed = 1.25f;
    public bool tokenIsPlayer;
    public bool tokenIsEnemy;
    public int entryIndex;

    [SerializeField] private Vector2 tokenScale = new Vector2(1, 1);
    private SpriteRenderer tokenRenderer;
    private Coroutine scaleCoroutine;
    private CharacterSize previousSize;
    private InitiativeUIManager initiativeUIManager;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        UpdateCharacterTokenSize();
        initiativeUIManager = FindFirstObjectByType<InitiativeUIManager>();
        tokenRenderer = GetComponent<SpriteRenderer>();
        tokenRenderer.sprite = tokenIcon;
        transform.localScale = tokenScale;
    }

    private void UpdateCharacterTokenSize()
    {
        switch (characterSize)
        {
            case CharacterSize.Small:
            targetTokenSize = 0.5f;
            break;

            case CharacterSize.Medium:
            targetTokenSize = 1f;
            break;

            case CharacterSize.Large:
            targetTokenSize = 2f;
            break;

            case CharacterSize.Huge:
            targetTokenSize = 4f;
            break;

            case CharacterSize.Gigantic:
            targetTokenSize = 6f;
            break;

        }

    }

    void Update()
    {
        if (characterSize != previousSize)
        {
            previousSize = characterSize;
            UpdateCharacterTokenSize();

            if (scaleCoroutine != null)
                StopCoroutine(scaleCoroutine);

            scaleCoroutine = StartCoroutine(ScaleToSizeCoroutine(targetTokenSize));
        }

        DeleteToken();
    }

    private IEnumerator ScaleToSizeCoroutine(float newSize)
    {
        float startSize = tokenSize;
        float elapsed = 0f;
        float duration = Mathf.Abs(newSize - startSize) / tokenGrowthSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            tokenSize = Mathf.Lerp(startSize, newSize, elapsed / duration);
            tokenScale = new Vector2(tokenSize, tokenSize);
            transform.localScale = tokenScale;
            yield return null;
        }

        tokenSize = newSize;
        tokenScale = new Vector2(tokenSize, tokenSize);
        transform.localScale = tokenScale;
    }

    private void DeleteToken()
    {
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                Debug.Log("Deleting Token!");
                if (tokenIsEnemy)
                {
                    string tokenName = gameObject.name;

                    // Find the entry with the same name
                    InitiativeEntry entryToRemove = initiativeUIManager.enemyEntries.Find(e => e.entryName == tokenName);
                    if (entryToRemove != null)
                    {
                        initiativeUIManager.enemyEntries.Remove(entryToRemove);
                        // Extract suffix and release it
                        string[] parts = tokenName.Split(' ');
                        if (parts.Length >= 2)
                        {
                            string baseName = parts[0];
                            string suffix = parts[1];

                            if (GhostTokenPlacer.usedSuffixes.ContainsKey(baseName))
                            {
                                GhostTokenPlacer.usedSuffixes[baseName].Remove(suffix);
                                Debug.Log($"Freed suffix {suffix} from {baseName}");
                            }
                        }

                        Debug.Log($"Removed enemy entry: {tokenName}");
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.LogWarning($"No enemy entry found with name: {tokenName}");
                    }
                }
            }
        }
    }


}
public enum CharacterSize
{
    Small,
    Medium,
    Large,
    Huge,
    Gigantic
}