using UnityEngine;

public class BaseProjectileCollision : IProjectileCollisionBehavior
{
    private const string BLOONTAG = "Bloon";
    private const string WALLTAG = "Wall";
    public void OnTriggerEnter2D(BaseProjectile aProjectile, Collider2D aCollider)
    {
        if (aCollider.CompareTag(BLOONTAG))
        {
            BaseBloon lBloon = aCollider.gameObject.GetComponent<BaseBloon>();
            //if the bloon takes dmg (might have immunity if it was just destroyed
            if(lBloon != null)
            {
                if (lBloon.TakeDamage(aProjectile.damage, aProjectile.projectileID, aProjectile.parentTower))
                {
                    aProjectile.TakeDamage();
                    aProjectile.ApplyStatusEffects(lBloon);
                }
            }            
        }
        else if (aCollider.CompareTag(WALLTAG) || aCollider.CompareTag("Edge"))
        {
            //if hit wall or edge of screen, return to pool 
            ProjectilePool.Instance.ReturnToPool(aProjectile.gameObject);
        }
    }
}
