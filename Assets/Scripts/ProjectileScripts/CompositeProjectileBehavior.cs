
using System.Collections.Generic;
using UnityEngine;

public class CompositeProjectileBehavior : IProjectileBehavior
{
    private List<IProjectileBehavior> projectileBehaviors = new List<IProjectileBehavior>();

    public void AddBehavior(IProjectileBehavior aBehavior)
    {
        projectileBehaviors.Add(aBehavior);
    }
    public void IntializeProjectile(GameObject aProjectile, GameObject aTarget, BaseTower aParentTower)
    {
        foreach (IProjectileBehavior lBehavior in projectileBehaviors)
        {
            lBehavior.IntializeProjectile(aProjectile, aTarget, aParentTower);
        }
    }
}
