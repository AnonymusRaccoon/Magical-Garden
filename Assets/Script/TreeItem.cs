using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TreeItem
{
    public TreeType type;
    [TextArea(15, 20)]
    public string description;
    public Sprite icon;
    public Sprite iconGris;
    public TileBase[] tiles = new TileBase[16];
    [EnumFlags] public PlotType canBePlacedOn;
    public int count;
    public int maxInstanceForWin;
}

public enum TreeType
{
    Nothing,
    AppleTree,
    RoundabboutTree,
    TribbleTree,
    Unamed1,
    SwapTree,
    GluttonTree,
    ThirstyTree,
    Cactus,
    Trunk = 16
};