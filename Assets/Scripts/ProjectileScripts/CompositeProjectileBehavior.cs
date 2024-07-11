using System.Collections.Generic;
using UnityEngine;

public class CompositeProjectileBehavior : IProjectileBehavior
{
    private IProjectileBehavior projectileBehavior;
    private List<IStatusEffect> projectileStatusEffects = new List<IStatusEffect>();
    /// <summary>
    /// Sets the projectile behavior, SingleShot, MultiShot, etc.
    /// </summary>
    /// <param name="aBehavior">Type of behavior</param>
    public void SetProjectileBehavior(IProjectileBehavior aBehavior)
    {
        projectileBehavior = aBehavior;
    }
    public void IntializeProjectile(GameObject aTarget, BaseTower aParentTower, List<IStatusEffect> aStatusEffectList)
    {        
        projectileBehavior.IntializeProjectile(aTarget, aParentTower, aStatusEffectList);
    }
    public void SetProjectileProperties(GameObject aProjectile, BaseTower aParentTower)
    {
        throw new System.NotImplementedException();
    }
    public void AddStatusEffect(IStatusEffect aStatusEffect)
    {
        projectileStatusEffects.Add(aStatusEffect);
    }
    public void RemoveStatusEffect(IStatusEffect aStatusEffect)
    {
        projectileStatusEffects.Remove(aStatusEffect);
    }

    public List<IStatusEffect> GetStatusEffects()
    {
        return projectileStatusEffects;
    }
}
