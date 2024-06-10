using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerStats", menuName = "Tower Stats")]
public class TowerStats : ScriptableObject
{
    public string towerName;
    public int cost;
    public int damage;
    public int pierce;
    public int range;
    public float attackSpeed;
    public bool hasCamoDetection;
    public string description;
    public Sprite towerSprite;

    public TowerUpgrade[] upgdrades;
}
