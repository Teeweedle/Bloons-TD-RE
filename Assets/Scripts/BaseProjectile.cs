using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public float speed { get; set; }
    public int health { get; set; }
    public float lifeSpan { get; set; }
    private Vector3 direction;


    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if ((lifeSpan -= Time.deltaTime) <= 0)
        {
            OnDestruction();
        }
    }
    public void SetDirection(Vector3 aDirection)
    {
        direction = aDirection;
    }
    private void OnDestruction()
    {
        Destroy(gameObject);
    }
}
