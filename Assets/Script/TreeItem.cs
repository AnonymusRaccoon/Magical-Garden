using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TreeItem
{
    public TreeType type;
    public string description;
    public Sprite icon;
    public TileBase[] tiles;
}

public enum TreeType
{
    ThirstyTree,
    Cactus,
    AppleTree,
    TribbleTree,
    SwapTree,
    ChameleonTree
};