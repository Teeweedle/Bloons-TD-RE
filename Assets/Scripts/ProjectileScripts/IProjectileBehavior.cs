using UnityEngine;

public interface IProjectileBehavior
{
    void IntializeProjectile(GameObject aProjectile, GameObject aTarget, BaseTower aParentTower);
}
