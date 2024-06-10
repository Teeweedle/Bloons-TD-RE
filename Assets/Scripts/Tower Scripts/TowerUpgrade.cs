using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradePath", menuName = "Tower Upgrade")]
public class TowerUpgrade : ScriptableObject
{
    public string towerName;
    public int cost;
    public int damage;
    public int pierce;
    public float attackSpeed;
    public bool hasCamoDetection;
    public string description;
    public Sprite upgradeSprite;
}
