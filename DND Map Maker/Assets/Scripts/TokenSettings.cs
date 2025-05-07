using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TokenSettings : MonoBehaviour
{

    public float tokenSize = 1f;
    public float targetTokenSize;
    public CharacterSize characterSize = CharacterSize.Medium;
    public Sprite tokenIcon;
    public float tokenGrowthSpeed = 1.25f;

    [SerializeField] private Vector2 tokenScale = new Vector2(1, 1);
    private SpriteRenderer tokenRenderer;
    private Coroutine scaleCoroutine;
    private CharacterSize previousSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCharacterTokenSize();
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
}
public enum CharacterSize
{
    Small,
    Medium,
    Large,
    Huge,
    Gigantic
}