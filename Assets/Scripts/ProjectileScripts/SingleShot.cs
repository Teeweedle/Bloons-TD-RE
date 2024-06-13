using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : IProjectileBehavior
{
    public void IntializeProjectile(GameObject aProjectile, GameObject aTarget, BaseTower aParentTower)
    {
        BaseProjectile lBaseProjectile = aProjectile.GetComponent<BaseProjectile>();
        if(lBaseProjectile != null)
        {
            lBaseProjectile.IntializeProjectile(aParentTower);
        }
    }
}
