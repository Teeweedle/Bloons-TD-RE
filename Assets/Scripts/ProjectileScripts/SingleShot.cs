using UnityEngine;

public class SingleShot : IProjectileBehavior
{
    public void IntializeProjectile(GameObject aTarget, BaseTower aParentTower)
    {
        GameObject lProjectile = ProjectilePool.GetProjectile(aParentTower._projectile);
        ((IProjectileBehavior)this).SetProjectileProperties(lProjectile, aParentTower);
        BaseProjectile lBaseProjectile = lProjectile.GetComponent<BaseProjectile>();
        if (lBaseProjectile != null)
        {
            lBaseProjectile.SetProjectileStats(aParentTower);
        }
    }
}

