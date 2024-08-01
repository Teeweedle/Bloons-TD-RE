using UnityEngine;

public class Fragmenting : MonoBehaviour, IProjectileOnDeath
{
    private int numberOfProjectiles;
    private Sprite childProjectileSprite;
    public Fragmenting(int aNumberOfProjectiles, Sprite aChildProjectileSprite)
    {
        numberOfProjectiles = aNumberOfProjectiles;
        childProjectileSprite = aChildProjectileSprite;
    }
    public void TriggerOnDeath(GameObject aParentProjectileGO, BaseTower aParentTower)
    {
        float lOffSet = 360 / numberOfProjectiles;
        for(int i = 0; i < numberOfProjectiles; i++)
        {
            //get a new projectile
            GameObject lNewProjectileGo = ProjectilePool.Instance.GetProjectile(aParentTower._projectile, aParentTower._towerStats.collisionType);

            SetProjectileProperties(lNewProjectileGo, aParentProjectileGO);
            //get script
            BaseProjectile lBaseProjectile = lNewProjectileGo.GetComponent<BaseProjectile>();
            if (lBaseProjectile != null)
            {
                //change to get child stats
                lBaseProjectile.SetProjectileStats(aParentTower, aParentTower._towerStats.projectileStats);
                lBaseProjectile.SetCollisionType(aParentTower._towerStats.collisionType);

                float lAngle = (i - (numberOfProjectiles / 2)) * lOffSet;
                Vector3 lDirection = Quaternion.Euler(0f, 0f, lAngle) * -aParentTower.transform.up;
                lBaseProjectile.SetDirection(lDirection);
            }
        }
    }
    public void SetProjectileProperties(GameObject aNewProjectileGO, GameObject aParentProjectileGO)
    {
        //set sprite
        aNewProjectileGO.GetComponent<SpriteRenderer>().sprite = childProjectileSprite;
        //set spawn location
        aNewProjectileGO.transform.position = aParentProjectileGO.transform.position;
        aParentProjectileGO.transform.rotation = Quaternion.identity;
    }
}
