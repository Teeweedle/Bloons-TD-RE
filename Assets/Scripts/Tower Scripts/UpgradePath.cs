using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradePath", menuName = "UpgradePath")]
public class UpgradePath : ScriptableObject
{
    public int upgradeLevel;
    public TowerUpgrade[] upgrades;
}
