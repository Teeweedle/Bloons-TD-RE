using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public float speed { get; set; }
    public int damage { get; set; }
    public int health { get; set; }
    public float lifeSpan { get; set; }
    public  BaseTower parentTower { get; set; }

    private Vector3 direction;
    private const string BLOONTAG = "Bloon";
    private int projectileID;

    private void Start()
    {
        projectileID = GetInstanceID();
    }
    public void IntializeProjectile(BaseTower aParentTower)
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
            Destroy(gameObject);
        }
    }
    public void SetDirection(Vector3 aDirection)
    {
        direction = aDirection;
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
            Destroy(gameObject);
        }
    }    
}
