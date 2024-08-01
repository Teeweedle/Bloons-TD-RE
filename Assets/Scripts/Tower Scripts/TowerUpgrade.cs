using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Tower Upgrade")]
public class TowerUpgrade : ScriptableObject
{
    public string upgradeName;
    public string projectileBehaviorName;
    public string collisionType;
    public StatusEffectSO projectileStatusEffect;
    public ProjectileUpgradeSO projectileStats;
    public int cost;
    public int xp;
    public int numberOfProjectiles;
    public float projectileOffset;
    public float range;
    public float attackSpeed;
    public bool hasCamoDetection;
    public string description;
    public Sprite upgradeSprite;
    public Sprite towerImageSprite;
    public Sprite GOSprite;
}
