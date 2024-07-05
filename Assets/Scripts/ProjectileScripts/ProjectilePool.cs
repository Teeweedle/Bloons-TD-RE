using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    private Queue<GameObject> projectilePool = new Queue<GameObject>();
    private Dictionary<string, Type> projectileTypeDictionary;
    private void Awake()
    {
        Instance = this;
        projectileTypeDictionary = new Dictionary<string, Type>
        {
            { "Base", typeof(BaseProjectile) },
            { "Bounce", typeof(BounceProjectile) }
        };
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
        AssignScript(lProjectile, aProjectileType);

        return lProjectile;
    }
    /// <summary>
    /// Returns a projectile to the pool and sets it inactive
    /// </summary>
    /// <param name="aProjectile">GameObject to return</param>
    public void ReturnToPool(GameObject aProjectile)
    {
        RemoveScripts(aProjectile);
        projectilePool.Enqueue(aProjectile);
        aProjectile.SetActive(false);
    }
    /// <summary>
    /// Removes all scripts that inherit IProjectile from a projectile
    /// </summary>
    /// <param name="aProjectile">The projectile to remove scripts from</param>
    private void RemoveScripts(GameObject aProjectile)
    {
        var scripts = aProjectile.GetComponents<MonoBehaviour>().Where(m => m is IProjectile).ToArray();

        foreach (var script in scripts)
        {
            Destroy(script);
        }
    }
    /// <summary>
    /// Checks a dictionary for the behavior script, if found it attaches it to the game object.
    /// </summary>
    /// <param name="aProjectile">Active projectile</param>
    /// <param name="aProjectileType">Projectile type</param>
    private void AssignScript(GameObject aProjectile, string aProjectileType)
    {
        if (projectileTypeDictionary.TryGetValue(aProjectileType, out Type lProjectileType))
        {
            aProjectile.AddComponent(lProjectileType);
        }
        else
        {
            Debug.LogError($"Projectile type {aProjectileType} not found");
        }
    }
}
