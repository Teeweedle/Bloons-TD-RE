
using UnityEngine;

public class NormalShot : ITowerBehavior
{
    //private IProjectileBehavior projectileBehavior;
    //public NormalShot(IProjectileBehavior aProjectileBehavior)
    //{
    //    projectileBehavior = aProjectileBehavior;
    //}
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
            GameObject lProjectile = Object.Instantiate(aParentTower._projectile, aParentTower.transform.position, Quaternion.identity);
            projectileBehavior.IntializeProjectile(lProjectile, aTarget, aParentTower);
   
            aParentTower.NextFireTime = Time.time + aParentTower._towerStats.attackSpeed;
        }
    }
}
