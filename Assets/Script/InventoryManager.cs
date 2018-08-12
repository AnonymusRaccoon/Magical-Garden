using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InventoryManager : MonoBehaviour
{
    public Camera cam;
    public Tilemap treeMap;
    public TileBase testTile;

    [Space]
    public GameObject[] slots;
    private TreeItem[] items = new TreeItem[12];
    public Plot[] plots = new Plot[25];
    private Dictionary<int, TreeItem> trees;

    private int draggedPosition = -1;
    private Vector3 defaultPos;


    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (draggedPosition != -1)
            {
                slots[draggedPosition].transform.GetChild(1).position = Input.mousePosition;
            }
        }
        else if(draggedPosition != -1)
        {
            if (CanPlantAt(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, treeMap.transform.parent.position.z))))
            {
                PlaceTree(items[draggedPosition]);
                items[draggedPosition] = null;
                slots[draggedPosition].transform.GetChild(1).position = defaultPos;
                slots[draggedPosition].transform.GetChild(1).gameObject.SetActive(false);
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

        if (-8 <= pos.x && pos.x <= 11 && -15 <= pos.y && pos.y <= 4)
        {
            if (plots[GetPosIndex(pos)].isUsed)
                return false;
            else
                return true;
        }
        else
            return false;
    }

    private void PlaceTree(TreeItem item)
    {
        //switch (item.type)
        //{
        //    case TreeType.AppleTree:
        //        break;
        //    default:
        //        break;
        //}
    }

    public void StartDraggin(int index)
    {
        slots[index].transform.SetAsLastSibling();
        draggedPosition = index;
        defaultPos = slots[draggedPosition].transform.position;
    }

    //public void AddItem(TreeItem item)
    //{
    //    for (int i = 0; i < 11; i++)
    //    {
    //        if (items[i] == null)
    //            items[i] = item;
    //    }
    //}

    private int GetPosIndex(Vector3Int pos)
    {
        pos.x += 8;
        pos.y += 15;

        int index = -1;
        switch (pos.x)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                index = 0;
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                index = 1;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
                index = 2;
                break;
            case 12:
            case 13:
            case 14:
            case 15:
                index = 3;
                break;
            case 16:
            case 17:
            case 18:
            case 19:
                index = 4;
                break;
        }
        switch (pos.y)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                index += 0;
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                index += 5;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
                index += 10;
                break;
            case 12:
            case 13:
            case 14:
            case 15:
                index += 15;
                break;
            case 16:
            case 17:
            case 18:
            case 19:
                index = 20;
                break;
        }
        return index;
    }
}
