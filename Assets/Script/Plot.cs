[System.Serializable]
public class Plot
{
    public PlotType type;
    public bool isUsed;
}

public enum PlotType
{
    Normal,
    Dry,
    Water,
    Brick,
    AdjacentWater
}
