using UnityEngine;

public class BounceProjectile : MonoBehaviour, IProjectile
{
    public float speed { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public float lifeSpan { get; set; }
    public int childCount { get; set; }
    public BaseTower parentTower { get; set; }
    public SpriteRenderer projectileSprite { get; set; }
    private Vector3 direction;
    private const string BLOONTAG = "Bloon";
    private const string WALLTAG = "Wall";
    private int projectileID;
    // Start is called before the first frame update
    void Start()
    {
        projectileID = GetInstanceID();
        projectileSprite = GetComponent<SpriteRenderer>();
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
    public void SetProjectileStats(BaseTower aParentTower)
    {
        parentTower = aParentTower;
        speed = aParentTower._towerStats.projectileSpeed;
        damage = aParentTower._towerStats.damage;
        health = aParentTower._towerStats.pierce;
        lifeSpan = aParentTower._towerStats.projectileLifeSpan;
        SetDirection(-aParentTower.transform.up);
    }
    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            ProjectilePool.Instance.ReturnToPool(gameObject);
        }
    }
    private void Bounce()
    {
        direction = -Vector3.Reflect(direction, Vector3.up);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(BLOONTAG))
        {
            //if the bloon takes dmg (might have immunity if it was just destroyed
            if (collision.gameObject.GetComponent<BaseBloon>().TakeDamage(damage, projectileID, parentTower))
            {
                TakeDamage();
            }
            //else if bloon hits a wall or edge of screen, bounce
        }else if(collision.CompareTag(WALLTAG) || collision.CompareTag("Edge"))
        {
            Debug.Log("We Bouncing");
            Bounce();
        }
    }
}
