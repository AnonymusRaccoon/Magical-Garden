using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] slots;
    private Item[] items = new Item[12];

    private int draggedPosition = -1;
    private Vector3 defaultPos;


    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (draggedPosition != -1)
            {
                slots[draggedPosition].transform.GetChild(0).position = Input.mousePosition;
            }
        }
        else if(draggedPosition != -1)
        {
            items[draggedPosition] = null;
            slots[draggedPosition].transform.GetChild(0).position = defaultPos;
            slots[draggedPosition].transform.GetChild(0).gameObject.SetActive(false);
            draggedPosition = -1;
        }
    }

    public void StartDraggin(int index)
    {
        draggedPosition = index;
        defaultPos = slots[draggedPosition].transform.position;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < 11; i++)
        {
            if (items[i] == null)
                items[i] = item;
        }
    }
}
