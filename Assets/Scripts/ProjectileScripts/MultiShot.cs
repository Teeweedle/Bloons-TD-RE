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
            GameObject lProjectile = Object.Instantiate(aParentTower._projectile, aParentTower.transform.position, Quaternion.identity);
            BaseProjectile lBaseProjectile = lProjectile.GetComponent<BaseProjectile>();
            if (lBaseProjectile != null)
            {
                lBaseProjectile.SetProjectileStats(aParentTower);

                float lAngle = (i - (numberOfProjectiles / 2)) * offset;
                Vector3 lDirection = Quaternion.Euler(0f, 0f, lAngle) * -aParentTower.transform.up;
                lBaseProjectile.SetDirection(lDirection);
            }
        }
    }
}
