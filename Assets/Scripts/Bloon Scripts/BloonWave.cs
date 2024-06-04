using System.Collections.Generic;

public class RoundsDictionary 
{
    public Dictionary<int, List<BloonWave>> rounds { get; set; } = new Dictionary<int, List<BloonWave>>(); // <Wave number, List<BloonWave>>
}
public class BloonWave
{
    public string bloonType { get; set; }
    public int count { get; set; }
    public float startTime { get; set; }
    public float endTime { get; set; }
}


