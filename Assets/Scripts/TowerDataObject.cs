using System;

[Serializable]
public class TowerDataObject
{
    public string name, description, upgradeLevel;
    public int damage, pierce, range, cost, xp;
    public float attackSpeed;
    public bool hasCamoDetection;
    public int[] upgradeLevelArray = new int[3];
    public bool maxTree;
}
