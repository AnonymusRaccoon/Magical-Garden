using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TreeItem
{
    public TreeType type;
    public string description;
    public Sprite icon;
    public TileBase[] tiles = new TileBase[16];
    [EnumFlags] public TreeType canOverrideTree;
    [EnumFlags] public PlotType canBePlacedOn;
    public int count;
}

public enum TreeType
{
    Nothing,
    AppleTree,
    JSP,
    TribbleTree,
    Lol,
    SwapTree,
    Nop,
    ThirstyTree,
    Cactus,
    ChameleonTree
};