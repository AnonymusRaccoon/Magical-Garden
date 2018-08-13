﻿using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Camera cam;
    public Tilemap treeMap;
    public Tilemap selectorMap;
    public TileBase[] selector;

    [Space]
    public GameObject[] slots;
    public TreeItem[] items = new TreeItem[15];
    public Plot[] plots = new Plot[25];

    public int draggedPosition = -1;
    private Vector3 defaultPos;
    private Vector2Int selectorPos;


    private void Start()
    {
        InventorySlot.manager = this;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (draggedPosition != -1)
            {
                slots[draggedPosition].transform.GetChild(1).position = Input.mousePosition;
                DisplaySelector(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectorMap.transform.parent.position.z)), items[draggedPosition]);
            }
        }
        else if(draggedPosition != -1)
        {
            HideSelector(selectorPos);
            if (CanPlantAt(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, treeMap.transform.parent.position.z)), items[draggedPosition]))
            {
                PlaceTree(items[draggedPosition], cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, treeMap.transform.parent.position.z)));
                items[draggedPosition].count -= 1;
                slots[draggedPosition].transform.GetChild(1).position = defaultPos;
                TextMeshProUGUI CompteurItem = slots[draggedPosition].GetComponentInChildren<TextMeshProUGUI>();
                CompteurItem.text = items[draggedPosition].count.ToString();
                if (items[draggedPosition].count < 1)
                {
                    slots[draggedPosition].transform.GetChild(1).GetComponent<Image>().sprite = items[draggedPosition].iconGris;
                }
            }
            else
            {
                slots[draggedPosition].transform.GetChild(1).position = defaultPos;
            }
            draggedPosition = -1;
        }
    }

    private bool CanPlantAt(Vector3 position, TreeItem item)
    {
        Vector3Int pos = treeMap.WorldToCell(position);
        if (-8 <= pos.x && pos.x <= 11 && -15 <= pos.y && pos.y <= 4)
        {
            if (GetPlotIndex(pos) != -1 && (plots[GetPlotIndex(pos)].treePlaced == TreeType.Nothing || (plots[GetPlotIndex(pos)].treePlaced & item.canOverrideTree) != 0) && (plots[GetPlotIndex(pos)].type & item.canBePlacedOn) != 0)
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

    private bool CanPlantAt(int index, TreeItem item)
    {
        if (index != -1 && (plots[index].treePlaced == TreeType.Nothing/* || (plots[index].treePlaced & item.canOverrideTree) != 0*/) && (plots[index].type & item.canBePlacedOn) != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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

        CallPowers(index);
    }

    private void PlaceTree(TreeItem item, int index)
    {
        Vector2Int plotPos = GetPlotPositionByIndex(index);
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
    }

    private void DeleteTreeAt(int index)
    {
        Vector2Int plotPos = GetPlotPositionByIndex(index);
        plots[index].treePlaced = TreeType.Nothing;

        int i = 0;
        for (int y = 0; y > -4; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                treeMap.SetTile(new Vector3Int(plotPos.x * 4 + x, plotPos.y * 4 + y, 0), null);
                i++;
            }
        }
    }

    private async void CallPowers(int spawnIndex)
    {
        List<int> dontCallPlotPower = new List<int>();

        //Spawn Power;
        if (plots[spawnIndex].treePlaced == TreeType.SwapTree)
        {
            List<int> plot = new List<int>();

            if (spawnIndex - 1 >= 0 && spawnIndex % 5 != 0 && plots[spawnIndex - 1].treePlaced != TreeType.Nothing)
                plot.Add(spawnIndex - 1);
            if (spawnIndex + 1 <= 24 && spawnIndex % 5 != 4 && plots[spawnIndex + 1].treePlaced != TreeType.Nothing)
                plot.Add(spawnIndex + 1);
            if (spawnIndex - 5 >= 0 && plots[spawnIndex - 5].treePlaced != TreeType.Nothing)
                plot.Add(spawnIndex - 5);
            if (spawnIndex + 5 <= 24 && plots[spawnIndex + 5].treePlaced != TreeType.Nothing)
                plot.Add(spawnIndex + 5);

            if (plot.Count > 0)
            {
                await Task.Delay(1000);
                int r = Random.Range(0, plot.Count);
                dontCallPlotPower.Add(plot[r]);
                PlaceTree(items[(int)plots[plot[r]].treePlaced - 1], spawnIndex);
                PlaceTree(items[(int)TreeType.SwapTree - 1], plot[r]);
            }
        }

        //Check for other powers
        List<int> callPowers = new List<int>();
        int p = -1;
        await Task.Delay(1000);
        for (int i = 0; i < 25; i++)
        {
            if (callPowers.Count > 0)
            {
                p = i - 1;
                i = callPowers[0];
                callPowers.RemoveAt(0);
            }
            else if(p != -1)
            {
                i = p;
                p = -1;
            }

            if (dontCallPlotPower.Contains(i))
                continue;

            if (plots[i].treePlaced == TreeType.TribbleTree)
            {
                List<int> freePlots = new List<int>();

                if (i - 1 >= 0 && i % 5 != 0 && CanPlantAt(i - 1, items[(int)TreeType.TribbleTree - 1]))
                    freePlots.Add(i - 1);
                if (i + 1 <= 24 && i % 5 != 4 && CanPlantAt(i + 1, items[(int)TreeType.TribbleTree - 1]))
                    freePlots.Add(i + 1);
                if (i - 5 >= 0 && CanPlantAt(i - 5, items[(int)TreeType.TribbleTree - 1]))
                    freePlots.Add(i - 5);
                if (i + 5 <= 24 && CanPlantAt(i + 5, items[(int)TreeType.TribbleTree - 1]))
                    freePlots.Add(i + 5);


                if(freePlots.Count > 0)
                {
                    int r = Random.Range(0, freePlots.Count);
                    dontCallPlotPower.Add(freePlots[r]);
                    PlaceTree(items[(int)TreeType.TribbleTree - 1], freePlots[r]);
                    await Task.Delay(1000);
                }

                List<int> plot = new List<int>();

                if (i - 1 >= 0 && i % 5 != 0 && plots[i - 1].treePlaced == TreeType.AppleTree)
                    plot.Add(i - 1);
                if (i + 1 <= 24 && i % 5 != 4 && plots[i + 1].treePlaced == TreeType.AppleTree)
                    plot.Add(i + 1);
                if (i - 5 >= 0 && plots[i - 5].treePlaced == TreeType.AppleTree)
                    plot.Add(i - 5);
                if (i + 5 <= 24 && plots[i + 5].treePlaced == TreeType.AppleTree)
                    plot.Add(i + 5);

                if (plot.Count > 0)
                {
                    foreach (int r in plot)
                    {
                        PlaceTree(items[(int)TreeType.Trunk - 1], r);
                    }
                    await Task.Delay(1000);
                }
            }
            else if(plots[i].treePlaced == TreeType.ThirstyTree)
            {
                List<int> plot = new List<int>();

                if (i - 1 >= 0 && i % 5 != 0 && plots[i - 1].treePlaced == TreeType.Cactus)
                    plot.Add(i - 1);
                if (i + 1 <= 24 && i % 5 != 4 && plots[i + 1].treePlaced == TreeType.Cactus)
                    plot.Add(i + 1);
                if (i - 5 >= 0 && plots[i - 5].treePlaced == TreeType.Cactus)
                    plot.Add(i - 5);
                if (i + 5 <= 24 && plots[i + 5].treePlaced == TreeType.Cactus)
                    plot.Add(i + 5);

                if(plot.Count > 0)
                {
                    int r = Random.Range(0, plot.Count);
                    dontCallPlotPower.Add(plot[r]);
                    DeleteTreeAt(i);
                    PlaceTree(items[(int)TreeType.AppleTree - 1], plot[r]);
                    await Task.Delay(1000);
                }
            }
            else if (plots[i].treePlaced == TreeType.SwapTree && ((i - 1 >= 0 && i % 5 != 0 && plots[i - 1].treePlaced == TreeType.AppleTree) || (i + 1 <= 24 && i % 5 != 4 && plots[i + 1].treePlaced == TreeType.AppleTree) || (i - 5 >= 0 && plots[i - 5].treePlaced == TreeType.AppleTree) || (i + 5 <= 24 && plots[i + 5].treePlaced == TreeType.AppleTree)))
            {
                List<int> plot = new List<int>();

                if (i - 1 >= 0 && i % 5 != 0 && plots[i - 1].treePlaced != TreeType.Nothing)
                    plot.Add(i - 1);
                if (i + 1 <= 24 && i % 5 != 4 && plots[i + 1].treePlaced != TreeType.Nothing)
                    plot.Add(i + 1);
                if (i - 5 >= 0 && plots[i - 5].treePlaced != TreeType.Nothing)
                    plot.Add(i - 5);
                if (i + 5 <= 24 && plots[i + 5].treePlaced != TreeType.Nothing)
                    plot.Add(i + 5);

                if (plot.Count > 0)
                {
                    int r = Random.Range(0, plot.Count);
                    dontCallPlotPower.Add(plot[r]);
                    callPowers.Add(i);
                    PlaceTree(items[(int)plots[plot[r]].treePlaced - 1], i);
                    PlaceTree(items[(int)TreeType.SwapTree - 1], plot[r]);
                    await Task.Delay(1000);
                }
            }
        }
    }

    public void StartDraggin(int index)
    {
        //if(items[index].count > 0)
        //{
            slots[index].transform.SetAsLastSibling();
            draggedPosition = index;
            defaultPos = slots[draggedPosition].transform.position;
        //}
    }

    //public void AddItem(TreeItem item)
    //{
    //    for (int i = 0; i < 11; i++)
    //    {
    //        if (items[i] == null)
    //            items[i] = item;
    //    }
    //}

    private void DisplaySelector(Vector3 position, TreeItem item)
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

        bool canPlant = CanPlantAt(position, item);
        selectorPos = plotPos;
        int i = -1;
        for (int y = 0; y > -4; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                i++;
                if ((i == 5 || i == 6 || i == 9 || i == 10) && canPlant)
                    continue;
                selectorMap.SetTile(new Vector3Int(plotPos.x * 4 + x, plotPos.y * 4 + y, 0), selector[i]);
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
                selectorMap.SetTile(new Vector3Int(pos.x * 4 + x, pos.y * 4 + y, 0), null);
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

    private Vector2Int GetPlotPositionByIndex(int index)
    {
        return new Vector2Int(index % 5 - 2, index / 5 - 3);
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
