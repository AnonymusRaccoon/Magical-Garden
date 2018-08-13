using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static InventoryManager manager;

    public void OnPointerDown(PointerEventData eventData)
    {
        manager.StartDraggin(int.Parse(transform.parent.name.Substring(6, 2)) - 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(manager.draggedPosition == -1)
            GameObject.Find("GameManager").GetComponent<Pokedex>().PokeDescription(int.Parse(transform.parent.name.Substring(6, 2)) - 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (manager.draggedPosition == -1)
            GameObject.Find("GameManager").GetComponent<Pokedex>().UpdateMissionText();
    }
}
