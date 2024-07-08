using UnityEngine;

public class BounceProjectileCollision : IProjectileCollisionBehavior
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
        }//else if bloon hits a wall or edge of screen, bounce
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
