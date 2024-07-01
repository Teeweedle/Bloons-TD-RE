using System;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileBehaviorFactory
{
    private static readonly Dictionary<string, Func<TowerUpgrade, IProjectileBehavior>> behaviors
        = new Dictionary<string, Func<TowerUpgrade, IProjectileBehavior>>()
        {
            { "SingleShot", upgrade => new SingleShot() },
            { "MultiShot", upgrade => new MultiShot(upgrade.numberOfProjectiles, upgrade.projectileOffset) }
        };

    public static IProjectileBehavior CreateBehavior(TowerUpgrade aTowerUpgrade)
    {
        if(behaviors.TryGetValue(aTowerUpgrade.projectileBehaviorName, out var behavior))
        {
            return behavior(aTowerUpgrade);
        }
        else
        {
            Debug.LogError($"Behavior {aTowerUpgrade.projectileBehaviorName} not found");
            return null;
        }
    }
}
