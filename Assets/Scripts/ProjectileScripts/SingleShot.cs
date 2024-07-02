using UnityEngine;

public class SingleShot : IProjectileBehavior
{
    public void IntializeProjectile(GameObject aTarget, BaseTower aParentTower)
    {
        GameObject lProjectile = Object.Instantiate(aParentTower._projectile, aParentTower.transform.position, Quaternion.identity);
        BaseProjectile lBaseProjectile = lProjectile.GetComponent<BaseProjectile>();
        if (lBaseProjectile != null)
        {
            lBaseProjectile.SetProjectileStats(aParentTower);
        }
    }
}
