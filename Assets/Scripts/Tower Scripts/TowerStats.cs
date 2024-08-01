using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerStats", menuName = "Tower Stats")]
public class TowerStats : ScriptableObject
{
    public string towerName;
    public string collisionType;
    public StatusEffectSO statusEffect;
    public ProjectileStatsSO projectileStats;
    public int cost;
    public float range;
    public float attackSpeed;
    public bool hasCamoDetection;
    public string description;
    public Sprite towerSprite;
    public Sprite towerGOSprite;
    public int[] upgradeLevelArray;
    public int numberOfBloonsPopped;

    public UpgradePath[] upgradePath;

    private void Start()
    {
        upgradeLevelArray = new int[upgradePath.Length];
    }
    public bool HasMaxTree()
    {
        for (int i = 0; i < upgradeLevelArray.Length; i++)
        {
            if (upgradeLevelArray[i] == 0)
            {
                return false;
            }
        }
        return true;
    }
}
