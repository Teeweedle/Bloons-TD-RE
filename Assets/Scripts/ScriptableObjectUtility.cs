using UnityEngine;

public static class ScriptableObjectUtility
{
    public static T Clone<T>(T source) where T : TowerStats
    {
        T clone = ScriptableObject.CreateInstance<T>();
        clone.towerName = source.towerName;
        clone.cost = source.cost;
        clone.damage = source.damage;
        clone.pierce = source.pierce;
        clone.range = source.range;
        clone.attackSpeed = source.attackSpeed;
        clone.hasCamoDetection = source.hasCamoDetection;
        clone.description = source.description;
        clone.towerSprite = source.towerSprite;
        clone.numberOfBloonsPopped = source.numberOfBloonsPopped;
        clone.upgradePath = source.upgradePath;

        clone.upgradeLevelArray = new int[source.upgradePath.Length];

        return clone;
    }
}
