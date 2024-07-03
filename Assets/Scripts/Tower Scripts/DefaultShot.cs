using UnityEngine;

public class DefaultShot : ITowerBehavior
{
    private CompositeProjectileBehavior projectileBehavior;
    public DefaultShot(CompositeProjectileBehavior aProjectileBehavior)
    {
        projectileBehavior = aProjectileBehavior;
    }
    public void Fire(GameObject aTarget, BaseTower aParentTower)
    {
        if (Time.time > aParentTower.NextFireTime)
        {
            aParentTower.LookAtTarget(aTarget.transform);
            //initialize projectile behavior
            projectileBehavior.IntializeProjectile(aTarget, aParentTower);
            //set next fire time
            aParentTower.NextFireTime = Time.time + aParentTower._towerStats.attackSpeed;
        }
    }
}
