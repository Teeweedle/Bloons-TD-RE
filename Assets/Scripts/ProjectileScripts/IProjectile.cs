using UnityEngine;

public interface IProjectile
{
    float speed { get; set; }
    int damage { get; set; }
    int health { get; set; }
    float lifeSpan { get; set; }
    int childCount { get; set; }
    BaseTower parentTower { get; set; }
    SpriteRenderer projectileSprite { get; set; }
    void SetProjectileStats(BaseTower aParentTower);
    void SetDirection(Vector3 aDirection);
    //TODO: Change to take in dmg amount
    void TakeDamage();

}
