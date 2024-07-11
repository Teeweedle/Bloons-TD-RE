public class TowerUpgradeHandler
{
    public void UpdateStats(BaseTower aTower, TowerUpgrade aTowerUpgrade, int[] aUpgradeArray)
    {
        aTower.UpdatePierce(aTowerUpgrade.pierce);
        aTower.UpdateDamage(aTowerUpgrade.damage);
        aTower.UpdateProjectileCollisionType(aTowerUpgrade.collisionType);
        aTower.UpdateRange(aTowerUpgrade.range);
        aTower.UpdateProjectileSpeed(aTowerUpgrade.projectileSpeed);
        aTower.UpdateProjectileSprite(aTowerUpgrade.upgradeName);
        aTower.UpdateAttackSpeed(aTowerUpgrade.attackSpeed);
        aTower.UpdateProjectileLifeSpan(aTowerUpgrade.projectileLifeSpan);
        aTower.UpdateCamoDetection(aTowerUpgrade.hasCamoDetection);
        aTower.UpdateCost(aTowerUpgrade.cost);
        //Update Projectile Behavior, Single Shot, Multi Shot, etc
        aTower.UpdateProjectileBehavior(aTowerUpgrade);

        if(aTowerUpgrade.projectileStatusEffect != null)
            aTower.UpdateStatusEffects(ProjectileBehaviorFactory.GetNewStatusEffect(aTowerUpgrade.projectileStatusEffect));

        aTower.UpdateUpgradeArray(aTower._towerStats.upgradeLevelArray, aUpgradeArray);        
    }

}
