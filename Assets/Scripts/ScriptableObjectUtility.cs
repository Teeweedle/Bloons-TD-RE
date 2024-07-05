using UnityEngine;

public static class ScriptableObjectUtility
{
    public static T Clone<T>(T source) where T : TowerStats
    {
        T clone = ScriptableObject.CreateInstance<T>();
        clone.towerName = source.towerName;
        clone.projectileType = source.projectileType;
        clone.cost = source.cost;
        clone.damage = source.damage;
        clone.pierce = source.pierce;
        clone.range = source.range;
        clone.projectileSpeed = source.projectileSpeed;
        clone.projectileLifeSpan = source.projectileLifeSpan;
        clone.attackSpeed = source.attackSpeed;
        clone.hasCamoDetection = source.hasCamoDetection;
        clone.description = source.description;
        clone.towerSprite = source.towerSprite;
        clone.projectileSpriteAtlas = source.projectileSpriteAtlas;
        clone.numberOfBloonsPopped = source.numberOfBloonsPopped;
        clone.upgradePath = source.upgradePath;

        clone.upgradeLevelArray = new int[source.upgradePath.Length];

        return clone;
    }
}
