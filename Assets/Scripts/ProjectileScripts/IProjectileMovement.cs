using UnityEngine;
public interface IProjectileMovement 
{
    void Move(GameObject aProjectile, Vector3 aDirection, float aSpeed);
}
