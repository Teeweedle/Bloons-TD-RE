using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "ProjectileStats", menuName = "ProjectileStats")]
public class ProjectileStatsSO : ScriptableObject
{
    public string collisionType;
    public int damage;
    public float pierce;
    public float speed;
    public float lifeSpan;
    public StatusEffectSO statusEffect;
    public SpriteAtlas projectileSpriteAtlas;
    public Sprite currentSprite;

    public void UpdateProjectileStats(TowerUpgrade aTowerUpgrade)
    {
        UpdatePierce(aTowerUpgrade.pierce);
        UpdateDamage(aTowerUpgrade.damage);
        UpdateProjectileSpeed(aTowerUpgrade.projectileSpeed);
        UpdateProjectileLifeSpan(aTowerUpgrade.projectileLifeSpan);
        UpdateProjectileSprite(aTowerUpgrade.upgradeName);
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
    /// <summary>
    /// Updates the sprite of the tower based on the name of the upgrade. Only changes if there is a sprite associated with the upgrade.
    /// Gets sprite from projectileSpriteAtlas
    /// </summary>
    /// <param name="aUpgradeName">Name of the upgrade</param>
    private void UpdateProjectileSprite(string aUpgradeName)
    {
        Sprite lSprite = projectileSpriteAtlas.GetSprite(aUpgradeName);
        if (lSprite != null)
        {
            currentSprite = lSprite;
        }
    }
    public void SetDefaultSprite()
    {
        currentSprite = projectileSpriteAtlas.GetSprite("Base");
    }
}
