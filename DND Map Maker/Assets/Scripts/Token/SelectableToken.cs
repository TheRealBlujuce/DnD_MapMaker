using UnityEngine;

public class SelectableToken : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color selectedColor = Color.green;
    public Color normalColor = Color.white;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Deselect(); // Ensure initial state
    }

    void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SelectionManager.Instance.ToggleSelection(this);
        }
        else
        {
            SelectionManager.Instance.ClearSelection();
            SelectionManager.Instance.Select(this);
        }
    }

    public void Select()
    {
        spriteRenderer.color = selectedColor;
    }

    public void Deselect()
    {
        spriteRenderer.color = normalColor;
    }
}
