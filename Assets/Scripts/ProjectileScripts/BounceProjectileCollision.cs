using UnityEngine;

public class BounceProjectileCollision : IProjectileCollisionBehavior
{
    private const string BLOONTAG = "Bloon";
    private const string WALLTAG = "Wall";
    public void OnTriggerEnter2D(BaseProjectile aProjectile, Collider2D aCollider)
    {
        if (aCollider.CompareTag(BLOONTAG))
        {
            BaseBloon lBloon = aCollider.gameObject.GetComponent<BaseBloon>();
            //if the bloon takes dmg (might have immunity if it was just destroyed
            if (lBloon.TakeDamage(aProjectile.damage, aProjectile.projectileID, aProjectile.parentTower))
            {
                aProjectile.TakeDamage();
                aProjectile.ApplyStatusEffects(lBloon);
            }
        }//else if projectile hits a wall or edge of screen, bounce
        else if (aCollider.CompareTag(WALLTAG) || aCollider.CompareTag("Edge"))
        {
            Bounce(aProjectile);
        }
    }
    private void Bounce(BaseProjectile aProjectile)
    {
        Vector3 lDirection = -Vector3.Reflect(aProjectile.direction, Vector3.up);

        aProjectile.SetDirection(lDirection);
    }
}
