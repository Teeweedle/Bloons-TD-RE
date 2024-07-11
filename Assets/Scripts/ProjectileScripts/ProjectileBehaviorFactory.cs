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
    private static readonly Dictionary<string, Func<StatusEffectSO, IStatusEffect>> statusEffectDictionary
        = new Dictionary<string, Func<StatusEffectSO, IStatusEffect>>()
        {
            { "Knockback", statusEffect => new KnockBackEffect(statusEffect.duration) },
            { "DoT", statusEffect => new DoTEffect(statusEffect.damage, statusEffect.duration, statusEffect.tickRate) },
            { "Slow", statusEffect => new SlowEffect(statusEffect.amount, statusEffect.duration) }
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
    /// <summary>
    /// Returns a new status effect to be added to the list of status effects
    /// </summary>
    /// <param name="aTower"></param>
    /// <param name="aStatusEffect"></param>
    /// <returns></returns>
    public static IStatusEffect GetNewStatusEffect(StatusEffectSO aStatusEffect)
    {
        if (statusEffectDictionary.ContainsKey(aStatusEffect.type))
        {
            return statusEffectDictionary[aStatusEffect.type](aStatusEffect);
        }
        else
        {
            Debug.LogError($"Status Effect {aStatusEffect.type} not found");
            return null;
        }
    }
}
