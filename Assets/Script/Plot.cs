[System.Serializable]
public class Plot
{
    [EnumFlagsAttribute]
    public PlotType type;
    public bool isUsed;
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
