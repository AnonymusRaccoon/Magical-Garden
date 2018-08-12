using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InventoryManager : MonoBehaviour
{
    public Camera cam;
    public Tilemap groundMap;
    public Tilemap treeMap;
    public TileBase testTile;

    [Space]
    public GameObject[] slots;
    private Item[] items = new Item[12];
    private Dictionary<string, TileBase[]> trees;

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
            if (CanPlantAt(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, treeMap.transform.parent.position.z))))
            {
                PlaceTree(items[draggedPosition]);
                items[draggedPosition] = null;
                slots[draggedPosition].transform.GetChild(0).position = defaultPos;
                slots[draggedPosition].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                slots[draggedPosition].transform.GetChild(0).position = defaultPos;
            }
            draggedPosition = -1;
        }
    }

    private bool CanPlantAt(Vector3 position)
    {
        Vector3Int pos = treeMap.WorldToCell(position);

        if(- 8 < pos.x && pos.x < 11 && -15 < pos.y && pos.y < 4)
        {
            if (treeMap.GetTile(pos) == null)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    private void PlaceTree(Item item)
    {
        switch (item.Name)
        {

            default:
                break;
        }
    }

    public void StartDraggin(int index)
    {
        slots[index].transform.SetAsLastSibling();
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
