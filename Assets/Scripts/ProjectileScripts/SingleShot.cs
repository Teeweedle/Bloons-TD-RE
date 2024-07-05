using UnityEngine;
public class SingleShot : IProjectileBehavior
{
    public void IntializeProjectile(GameObject aTarget, BaseTower aParentTower)
    {
        GameObject lProjectile = ProjectilePool.Instance.GetProjectile(aParentTower._projectile, aParentTower._towerStats.projectileType);
        ((IProjectileBehavior)this).SetProjectileProperties(lProjectile, aParentTower);
        //BaseProjectile lBaseProjectile = lProjectile.GetComponent<BaseProjectile>();
        var lComponents = lProjectile.GetComponents<MonoBehaviour>();
        if (lComponents != null)
        {
            foreach (var script in lComponents)
            {
                if (script is IProjectile projectile)
                {
                    projectile.SetProjectileStats(aParentTower);
                }
            }               
            //lProjectileScript.SetProjectileStats(aParentTower);
        }
    }
}

