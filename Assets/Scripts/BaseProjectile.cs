using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour, IProjectile
{
    public float speed { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public float lifeSpan { get; set; }
    public int childCount { get; set; }
    public  BaseTower parentTower { get; set; }
    public SpriteRenderer projectileSprite { get; set; }
    public int projectileID { get; private set; }
    public Vector3 direction { get; private set; }
    public IProjectileCollisionBehavior collisionBehavior { get; private set; }


    private readonly Dictionary<string, IProjectileCollisionBehavior> projectileCollisionDictionary 
        = new Dictionary<string, IProjectileCollisionBehavior>
    {
            { "Base", new BaseProjectileCollision() },
            { "Bounce", new BounceProjectileCollision() }
    };
    private void Start()
    {
        projectileID = GetInstanceID();
        projectileSprite = GetComponent<SpriteRenderer>();
    }
    public void SetProjectileStats(BaseTower aParentTower)
    {
        parentTower = aParentTower;
        speed = aParentTower._towerStats.projectileSpeed;
        damage = aParentTower._towerStats.damage;
        health = aParentTower._towerStats.pierce;
        lifeSpan = aParentTower._towerStats.projectileLifeSpan;
        SetDirection(-aParentTower.transform.up);
    }
    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
        if ((lifeSpan -= Time.deltaTime) <= 0)
        {
            ProjectilePool.Instance.ReturnToPool(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 aDirection)
    {
        direction = aDirection;
        //rotate the sprite to match the direction
        float lAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, lAngle);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //call current collision behavior
        collisionBehavior?.OnTriggerEnter2D(this, collision);
    }
    public void SetCollisionType(string aCollisionType)
    {
        if(projectileCollisionDictionary.TryGetValue(aCollisionType, out IProjectileCollisionBehavior lCollisionBehavior))
        {
            collisionBehavior = lCollisionBehavior;
        }
        else
        {
            Debug.LogError("Collision type not found");
        }
    }
    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0) 
        {
            ProjectilePool.Instance.ReturnToPool(gameObject);
        }
    }    
}
