using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject.Find("GameManager").GetComponent<InventoryManager>().StartDraggin(int.Parse(transform.parent.name.Substring(6, 2)) - 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        GameObject.Find("GameManager").GetComponent<Pokedex>().PokeDescription(int.Parse(transform.parent.name.Substring(6, 2)) - 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Find("GameManager").GetComponent<Pokedex>().UpdateMissionText();
    }
}
