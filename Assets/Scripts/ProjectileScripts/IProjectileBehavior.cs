using System.Collections.Generic;
using UnityEngine;

public interface IProjectileBehavior
{
    /// <summary>
    /// Initializes and instantiates the projectile
    /// </summary>
    /// <param name="aTarget">Target of the projectile</param>
    /// <param name="aParentTower">The tower that fired the projectile</param>
    void IntializeProjectile(GameObject aTarget, BaseTower aParentTower, List<IStatusEffect> aStatusEffectList);
    /// <summary>
    /// Sets the properties of the projectile, sprite, location, rotation
    /// </summary>
    /// <param name="aProjectile">Object we are setting properties on</param>
    /// <param name="aParentTower">Tower that fired the projectile, ie the spawn location</param>
    void SetProjectileProperties(GameObject aProjectile, BaseTower aParentTower)
    {
        //set projectile sprite
        aProjectile.GetComponent<SpriteRenderer>().sprite = aParentTower.GetProjectileSprite();
        //set orientation of projectile
        //TODO: Set rotation of sprite
        aProjectile.transform.position = aParentTower.transform.position;
        aProjectile.transform.rotation = Quaternion.identity;
    }
}
