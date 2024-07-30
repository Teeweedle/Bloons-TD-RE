
using UnityEngine;

public interface IProjectileOnDeath 
{
    void TriggerOnDeath(GameObject aParentProjectile, BaseTower aParentTower);
    void SetProjectileProperties(GameObject aNewProjectileGO, GameObject aParentProjectileGO);
}
