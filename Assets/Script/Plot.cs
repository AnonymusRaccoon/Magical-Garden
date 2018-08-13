[System.Serializable]
public class Plot
{
    [EnumFlags]
    public PlotType type;
    public TreeType treePlaced;

    public int startIndex;
}

[System.Flags]
public enum PlotType
{
    Normal = 1,
    Dry = 2,
    Water = 4,
    Brick = 8,
    AdjacentWater = 16,
    Cliff = 32
}
