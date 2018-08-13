using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TreeItem
{
    public TreeType type;
    public string description;
    public Sprite icon;
    public Sprite iconGris;
    public TileBase[] tiles = new TileBase[16];
    [EnumFlags] public TreeType canOverrideTree;
    [EnumFlags] public PlotType canBePlacedOn;
    public int count;
}

public enum TreeType
{
    Nothing,
    AppleTree,
    ChameleonTree,
    TribbleTree,
    Unamed0,
    SwapTree,
    Unamed1,
    ThirstyTree,
    Cactus,
};