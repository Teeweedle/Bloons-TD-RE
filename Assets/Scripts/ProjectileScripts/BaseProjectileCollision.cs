using UnityEngine;

public class BaseProjectileCollision : IProjectileCollisionBehavior
{
    private const string BLOONTAG = "Bloon";
    private const string WALLTAG = "Wall";

    public void OnTriggerEnter2D(BaseProjectile aProjectile, Collider2D aCollider)
    {
        if (aCollider.CompareTag(BLOONTAG))
        {
            //if the bloon takes dmg (might have immunity if it was just destroyed
            if (aCollider.gameObject.GetComponent<BaseBloon>().TakeDamage(aProjectile.damage, aProjectile.projectileID, aProjectile.parentTower))
            {
                aProjectile.TakeDamage();
            }
        }
        else if (aCollider.CompareTag(WALLTAG) || aCollider.CompareTag("Edge"))
        {
            Debug.Log("Hit Wall");
            //if hit wall or edge of screen, return to pool 
            ProjectilePool.Instance.ReturnToPool(aProjectile.gameObject);
        }
    }
}
