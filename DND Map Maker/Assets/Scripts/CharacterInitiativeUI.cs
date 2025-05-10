using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInitiativeUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TMP_InputField initiativeInput;
    public Image iconSprite;

    private string characterName;
    private Sprite characterIcon;

    public void Initialize(string name, int initiative, Sprite imageIcon)
    {
        characterIcon = imageIcon;
        iconSprite.sprite = characterIcon;
        characterName = name;
        nameText.text = name;
        initiativeInput.text = initiative.ToString();
    }

    public string GetPlayerName() => characterName;

    public int GetInitiativeValue()
    {
        int val = 0;
        int.TryParse(initiativeInput.text, out val);
        return val;
    }

    public Sprite GetPlayerIcon() => characterIcon;

    public void DeleteRow()
    {
        Destroy(this.gameObject);
    }


}
