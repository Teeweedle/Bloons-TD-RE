using UnityEngine;

public interface IProjectileBehavior
{
    /// <summary>
    /// Initializes and instantiates the projectile
    /// </summary>
    /// <param name="aTarget">Target of the projectile</param>
    /// <param name="aParentTower">The tower that fired the projectile</param>
    void IntializeProjectile(GameObject aTarget, BaseTower aParentTower);

    void SetProjectileProperties(GameObject aProjectile, BaseTower aParentTower)
    {
        //Default implementation
        aProjectile.transform.position = aParentTower.transform.position;
        aProjectile.transform.rotation = Quaternion.identity;
    }
}
