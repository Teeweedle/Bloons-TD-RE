using UnityEngine;

public class MultiShot : IProjectileBehavior
{
    private int numberOfProjectiles;
    private float offset;
    public MultiShot(int aNumberOfProjectiles, float aOffset)
    {
        numberOfProjectiles = aNumberOfProjectiles;
        offset = aOffset;
    }
    public void IntializeProjectile(GameObject aProjectile, GameObject aTarget, BaseTower aParentTower)
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject lNewProjectile = GameObject.Instantiate(aProjectile, aParentTower.transform.position, Quaternion.identity);
            BaseProjectile lBaseProjectile = lNewProjectile.GetComponent<BaseProjectile>();
            if (lBaseProjectile != null)
            {
                lBaseProjectile.IntializeProjectile(aParentTower);

                float lAngle = (i - (numberOfProjectiles / 2)) * offset;
                Vector3 lDirection = Quaternion.Euler(0f, 0f, lAngle) * -aParentTower.transform.up;
                lBaseProjectile.SetDirection(lDirection);
            }
        }
    }
}
