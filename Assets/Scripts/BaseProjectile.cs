using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public float speed { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public float lifeSpan { get; set; }
    public  BaseTower parentTower { get; set; }
    public SpriteRenderer projectileSprite { get; set; }

    private Vector3 direction;
    private const string BLOONTAG = "Bloon";
    private int projectileID;

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
        transform.position += direction * speed * Time.deltaTime;
        if ((lifeSpan -= Time.deltaTime) <= 0)
        {
            ProjectilePool.ReturnToPool(gameObject);
        }
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
        if (collision.CompareTag(BLOONTAG))
        {
            //if the bloon takes dmg (might have immunity if it was just destroyed
            if(collision.gameObject.GetComponent<BaseBloon>().TakeDamage(damage, projectileID, parentTower))
            {
                TakeDamage();
            }            
        }
    }
    private void TakeDamage()
    {
        health -= 1;
        if (health <= 0) 
        {
            ProjectilePool.ReturnToPool(gameObject);
        }
    }    
}
