﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InventoryManager : MonoBehaviour
{
    public Camera cam;
    public Tilemap treeMap;
    public Tilemap selectorMap;
    public TileBase[] selector;

    [Space]
    public GameObject[] slots;
    public TreeItem[] items = new TreeItem[12];
    public Plot[] plots = new Plot[25];

    private int draggedPosition = -1;
    private Vector3 defaultPos;
    private Vector2Int selectorPos;


    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (draggedPosition != -1)
            {
                slots[draggedPosition].transform.GetChild(1).position = Input.mousePosition;
                DisplaySelector(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectorMap.transform.parent.position.z)));
            }
        }
        else if(draggedPosition != -1)
        {
            HideSelector(selectorPos);
            if (CanPlantAt(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, treeMap.transform.parent.position.z))))
            {
                PlaceTree(items[draggedPosition], cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, treeMap.transform.parent.position.z)));
                items[draggedPosition].count -= 1;
                slots[draggedPosition].transform.GetChild(1).position = defaultPos;
            }
            else
            {
                slots[draggedPosition].transform.GetChild(1).position = defaultPos;
            }
            draggedPosition = -1;
        }
    }

    private bool CanPlantAt(Vector3 position)
    {
        Vector3Int pos = treeMap.WorldToCell(position);
        if (-8 <= pos.x && pos.x <= 11 && -15 <= pos.y && pos.y <= 4)
        {
            if (GetPlotIndex(pos) != -1 && plots[GetPlotIndex(pos)].treePlaced == TreeType.Nothing && (plots[GetPlotIndex(pos)].type & PlotType.Normal) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    private void PlaceTree(TreeItem item, Vector3 position)
    {
        Vector3Int cellPos = treeMap.WorldToCell(position);
        int index = GetPlotIndex(cellPos);
        Vector2Int plotPos = GetPlotPosition(cellPos);
        plots[index].treePlaced = item.type;

        int i = 0;
        for (int y = 0; y > -4; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                treeMap.SetTile(new Vector3Int(plotPos.x * 4 + x, plotPos.y * 4 + y, 0), item.tiles[i]);
                i++;
            }
        }
        

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

    private void DisplaySelector(Vector3 position)
    {
        Vector3Int pos = selectorMap.WorldToCell(position);
        Vector2Int plotPos = GetPlotPosition(pos);

        if (selectorPos.x != 999 && selectorPos.y != 999)
        {
            if (selectorPos == plotPos)
                return;

            HideSelector(selectorPos);
        }

        if (plotPos.x == 999 || plotPos.y == 999)
            return;

        bool canPlant = CanPlantAt(position);
        selectorPos = plotPos;
        int i = -1;
        for (int y = 0; y > -4; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                i++;
                if ((i == 5 || i == 6 || i == 9 || i == 10) && canPlant)
                    continue;
                treeMap.SetTile(new Vector3Int(plotPos.x * 4 + x, plotPos.y * 4 + y, 0), selector[i]);
            }
        }
    }

    private void HideSelector(Vector2Int pos)
    {
        int i = 0;
        for (int y = 0; y > -4; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                treeMap.SetTile(new Vector3Int(pos.x * 4 + x, pos.y * 4 + y, 0), null);
                i++;
            }
        }
        selectorPos = new Vector2Int(999, 999);
    }

    private int GetPlotIndex(Vector3Int pos)
    {
        Vector2Int plotPos = GetPlotPosition(pos);
        if (plotPos.x == 999 || plotPos.y == 999)
            return -1;

        plotPos.x += 2;
        plotPos.y += 3;
        return plotPos.x + plotPos.y * 5;
    }

    private Vector2Int GetPlotPosition(Vector3Int position)
    {
        position.x += 8;
        position.y += 15;

        Vector2Int pos = new Vector2Int(999, 999);
        switch (position.x)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                pos.x = 0;
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                pos.x = 1;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
                pos.x = 2;
                break;
            case 12:
            case 13:
            case 14:
            case 15:
                pos.x = 3;
                break;
            case 16:
            case 17:
            case 18:
            case 19:
                pos.x = 4;
                break;
        }
        switch (position.y)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                pos.y = 0;
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                pos.y = 1;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
                pos.y = 2;
                break;
            case 12:
            case 13:
            case 14:
            case 15:
                pos.y = 3;
                break;
            case 16:
            case 17:
            case 18:
            case 19:
                pos.y = 4;
                break;
        }
        if(pos.x != 999 || pos.y != 999)
        {
            pos.x -= 2;
            pos.y -= 3;
        }
        return pos;
    }
}
