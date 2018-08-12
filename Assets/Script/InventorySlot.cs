using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject.Find("GameManager").GetComponent<InventoryManager>().StartDraggin(int.Parse(transform.parent.name.Substring(6, 2)) - 1);
    }
}
