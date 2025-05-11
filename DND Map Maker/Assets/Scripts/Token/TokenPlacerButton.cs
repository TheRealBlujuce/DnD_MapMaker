using UnityEngine;

public class TokenPlacerButton : MonoBehaviour
{
    public GameObject ghostTokenPrefab;
    public GameObject tokenPrefab;
    public Sprite icon;
    public string baseName = "Enemy";
    public Color color = Color.white;
    public CharacterSize characterSize = CharacterSize.Medium;
    public Camera mainCamera;
    public LayerMask placementLayer;
    public InitiativeUIManager initiativeUIManager;

    public void StartPlacement()
    {
        GameObject ghost = Instantiate(ghostTokenPrefab);
        ghost.layer = LayerMask.NameToLayer("Ignore Raycast");

        var placer = ghost.GetComponent<GhostTokenPlacer>();
        placer.tokenPrefab = tokenPrefab;
        placer.icon = icon;
        placer.baseName = baseName;
        placer.color = color;
        placer.mainCamera = mainCamera;
        placer.placementLayer = placementLayer;
        placer.initiativeUIManager = initiativeUIManager;
        placer.characterSize = characterSize;

        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.sprite = icon;
            sr.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }
}

