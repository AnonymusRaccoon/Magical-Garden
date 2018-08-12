﻿using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TreeItem
{
    public TreeType type;
    public string description;
    public Sprite icon;
    public TileBase[] tiles = new TileBase[16];
    public int count;
}

public enum TreeType
{
    Nothing,
    ThirstyTree,
    Cactus,
    AppleTree,
    TribbleTree,
    SwapTree,
    ChameleonTree
};