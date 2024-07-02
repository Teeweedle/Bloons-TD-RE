using UnityEngine;

public class NormalShot : ITowerBehavior
{
    private CompositeProjectileBehavior projectileBehavior;
    public NormalShot(CompositeProjectileBehavior aProjectileBehavior)
    {
        projectileBehavior = aProjectileBehavior;
    }
    public void Fire(GameObject aTarget, BaseTower aParentTower)
    {
        if (Time.time > aParentTower.NextFireTime)
        {
            aParentTower.LookAtTarget(aTarget.transform);
            //instantiate projectile
            projectileBehavior.IntializeProjectile(aTarget, aParentTower);
            //set next fire time
            aParentTower.NextFireTime = Time.time + aParentTower._towerStats.attackSpeed;
        }
    }
}
