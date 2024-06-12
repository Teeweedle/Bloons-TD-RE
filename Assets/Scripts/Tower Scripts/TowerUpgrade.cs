using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Tower Upgrade")]
public class TowerUpgrade : ScriptableObject
{
    public string upgradeName;
    public int cost;
    public int xp;
    public int damage;
    public int pierce;
    public int range;
    public float attackSpeed;
    public bool hasCamoDetection;
    public string description;
    public Sprite upgradeSprite;
    public Sprite towerImageSprite;
    public Sprite GOSprite;
}
