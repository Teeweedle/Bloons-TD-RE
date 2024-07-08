using UnityEngine;

public class MultiShot : IProjectileBehavior
{
    private int numberOfProjectiles;
    private float offset;
    public MultiShot(int aNumberOfProjectiles, float aOffset)
    {
        numberOfProjectiles = aNumberOfProjectiles;
        offset = aOffset;
    }
    public void IntializeProjectile(GameObject aTarget, BaseTower aParentTower)
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject lProjectileGO = ProjectilePool.Instance.GetProjectile(aParentTower._projectile, aParentTower._towerStats.collisionType);
            ((IProjectileBehavior)this).SetProjectileProperties(lProjectileGO, aParentTower);
            BaseProjectile lBaseProjectile = lProjectileGO.GetComponent<BaseProjectile>();
            if (lBaseProjectile != null)
            {
                lBaseProjectile.SetProjectileStats(aParentTower);
                lBaseProjectile.SetCollisionType(aParentTower._towerStats.collisionType);
                //set projectile spread
                float lAngle = (i - (numberOfProjectiles / 2)) * offset;
                Vector3 lDirection = Quaternion.Euler(0f, 0f, lAngle) * -aParentTower.transform.up;
                lBaseProjectile.SetDirection(lDirection);
            }
        }
    }
}
