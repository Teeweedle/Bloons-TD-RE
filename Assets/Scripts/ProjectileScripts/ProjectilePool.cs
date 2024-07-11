using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    private Queue<GameObject> projectilePool = new Queue<GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Gets a projectile from the pool and sets it active
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns>Active projectile with behavior attached</returns>
    public GameObject GetProjectile(GameObject prefab, string aProjectileType)
    {
        GameObject lProjectile;
        //create new projectile if pool is empty
        if (projectilePool.Count == 0)
        {
            lProjectile = Instantiate(prefab);
        }
        else
        {
            lProjectile = projectilePool.Dequeue();
            lProjectile.SetActive(true);
        }

        return lProjectile;
    }
    /// <summary>
    /// Returns a projectile to the pool and sets it inactive
    /// </summary>
    /// <param name="aProjectile">GameObject to return</param>
    public void ReturnToPool(GameObject aProjectile)
    {
        projectilePool.Enqueue(aProjectile);
        aProjectile.SetActive(false);
    }
}
