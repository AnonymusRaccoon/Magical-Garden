using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject.Find("GameManager").GetComponent<InventoryManager>().StartDraggin(transform.parent.GetSiblingIndex());
    }
}
