using UnityEngine;

public class StraightMovement : MonoBehaviour, IProjectileMovement
{
    public void Move(GameObject aProjectile, Vector3 aDirection, float aSpeed)
    {
        aProjectile.transform.position += aDirection * aSpeed * Time.deltaTime;
    }
}
