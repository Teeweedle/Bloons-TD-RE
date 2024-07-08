using UnityEngine;

public interface IProjectileCollisionBehavior 
{
    void OnTriggerEnter2D(BaseProjectile aProjectile, Collider2D aCollider);
}
