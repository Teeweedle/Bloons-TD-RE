using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "NewProjectileStats", menuName = "ProjectileStats")]
public class ProjectileStatsSO : ScriptableObject
{
    public string collisionType;
    public int damage;
    public int pierce;
    public float speed;
    public float lifeSpan;
    public StatusEffectSO statusEffect;
    public SpriteAtlas projectileSpriteAtlas;
    public Sprite currentSprite;

    public void UpdateProjectileStats(TowerUpgrade aTowerUpgrade)
    {
        UpdatePierce(aTowerUpgrade.projectileStats.pierce);
        UpdateDamage(aTowerUpgrade.projectileStats.damage);
        UpdateProjectileSpeed(aTowerUpgrade.projectileStats.speed);
        UpdateProjectileLifeSpan(aTowerUpgrade.projectileStats.lifeSpan);
        UpdateProjectileSprite(aTowerUpgrade.projectileStats.sprite);
    }
    private void UpdatePierce(int aPierce)
    {
        pierce += aPierce;
    }
    private void UpdateDamage(int aDamage)
    {
        damage += aDamage;
    }
    private void UpdateProjectileSpeed(float aProjectileSpeed)
    {
        if (aProjectileSpeed != 0)
            speed *= (1.0f + aProjectileSpeed);
    }
    private void UpdateProjectileLifeSpan(float aProjectileLifeSpan)
    {
        if (aProjectileLifeSpan != 0)
            lifeSpan *= (1.0f + aProjectileLifeSpan);
    }
    private void UpdateProjectileSprite(Sprite aSprite)
    {
        if(aSprite != null)
            currentSprite = aSprite;
    }
}
