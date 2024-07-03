using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "NewTowerStats", menuName = "Tower Stats")]
public class TowerStats : ScriptableObject
{
    public string towerName;
    public int cost;
    public int damage;
    public int pierce;
    public float range;
    public float projectileSpeed;
    public float projectileLifeSpan;
    public float attackSpeed;
    public bool hasCamoDetection;
    public string description;
    public Sprite towerSprite;
    public Sprite towerGOSprite;
    public SpriteAtlas projectileSpriteAtlas;
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
