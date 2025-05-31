using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public string header = "";
    
    public string content = "";

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Instance.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Instance.Hide();
    }
}

