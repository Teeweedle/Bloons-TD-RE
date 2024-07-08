using UnityEngine;

public class SingleShot : IProjectileBehavior{

    public void IntializeProjectile(GameObject aTarget, BaseTower aParentTower)
    {
        GameObject lProjectileGO = ProjectilePool.Instance.GetProjectile(aParentTower._projectile, aParentTower._towerStats.collisionType);
        ((IProjectileBehavior)this).SetProjectileProperties(lProjectileGO, aParentTower);
        BaseProjectile lBaseProjectile = lProjectileGO.GetComponent<BaseProjectile>();
        if (lBaseProjectile != null)
        {
            lBaseProjectile.SetProjectileStats(aParentTower);
            //assign collision behavior
            lBaseProjectile.SetCollisionType(aParentTower._towerStats.collisionType);
            
        }
    }
}

