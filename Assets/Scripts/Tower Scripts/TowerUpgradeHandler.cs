public class TowerUpgradeHandler
{
    public void UpdateStats(BaseTower aTower, TowerUpgrade aTowerUpgrade, int[] aUpgradeArray)
    {
        aTower.UpdateProjectileCollisionType(aTowerUpgrade.collisionType);
        aTower.UpdateRange(aTowerUpgrade.range);
        aTower.UpdateAttackSpeed(aTowerUpgrade.attackSpeed);
        aTower.UpdateCamoDetection(aTowerUpgrade.hasCamoDetection);
        aTower.UpdateCost(aTowerUpgrade.cost);
        //Update Projectile Behavior, Single Shot, Multi Shot, etc
        aTower.UpdateProjectileBehavior(aTowerUpgrade);

        if(aTowerUpgrade.projectileStatusEffect != null)
            aTower.UpdateStatusEffects(ProjectileBehaviorFactory.GetNewStatusEffect(aTowerUpgrade.projectileStatusEffect));

        aTower.UpdateUpgradeArray(aTower._towerStats.upgradeLevelArray, aUpgradeArray);        
    }

}
