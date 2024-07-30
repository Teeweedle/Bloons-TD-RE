using System.Collections.Generic;
using UnityEngine;

public static class ProjectileMovementFactory
{
    private static readonly Dictionary<string, IProjectileMovement> projectileMovements
            = new Dictionary<string, IProjectileMovement>
        {
            { "Straight", new StraightMovement() }            
        };
    public static IProjectileMovement GetProjectileMovement(string aProjectileType)
    {
        if (projectileMovements.TryGetValue(aProjectileType, out var projectileMovementType))
        {
            return projectileMovementType;
        }
        else
        {
            Debug.LogError($"Projectile Movement not found {aProjectileType}");
            return null;
        }
    }
}
