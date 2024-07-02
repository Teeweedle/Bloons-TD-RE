using System.Collections.Generic;
using UnityEngine;

public static class ProjectilePool 
{
    private static Queue<GameObject> projectilePool = new Queue<GameObject>();

    /// <summary>
    /// Gets a projectile from the pool and sets it active
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject GetProjectile(GameObject prefab)
    {
        //create new projectile if pool is empty
        if (projectilePool.Count == 0)
        {
            return Object.Instantiate(prefab);
        }
        else
        {
            GameObject lProjectile = projectilePool.Dequeue();
            lProjectile.SetActive(true);
            return lProjectile;
        }
    }
    /// <summary>
    /// Returns a projectile to the pool and sets it inactive
    /// </summary>
    /// <param name="projectile">GameObject to return</param>
    public static void ReturnToPool(GameObject projectile)
    {
        projectilePool.Enqueue(projectile);
        projectile.SetActive(false);
    }
}
