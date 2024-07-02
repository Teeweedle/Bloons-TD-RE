
using UnityEngine;

public class CompositeProjectileBehavior : IProjectileBehavior
{
    private IProjectileBehavior projectileBehavior;

    public void SetProjectileBehavior(IProjectileBehavior aBehavior)
    {
        projectileBehavior = aBehavior;
    }
    public void IntializeProjectile(GameObject aTarget, BaseTower aParentTower)
    {        
        projectileBehavior.IntializeProjectile(aTarget, aParentTower);
    }

    public void SetProjectileProperties(GameObject aProjectile, BaseTower aParentTower)
    {
        throw new System.NotImplementedException();
    }
}
