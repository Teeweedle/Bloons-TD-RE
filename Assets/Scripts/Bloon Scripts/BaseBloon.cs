using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBloon : MonoBehaviour
{
    [SerializeField] protected float distance;
    [SerializeField] protected bool isStrong, isCamo, isRegrow, isFortified;
    [SerializeField] protected float speed;
    [SerializeField] protected string child;
    [SerializeField] protected int childCount, health, cash;
    [SerializeField] protected Sprite bloonSprite;

    public delegate IEnumerator BaseBloonDelegate(int aChildCount, float aDistance, int aPathPositon, Vector3 aBloonPosition);
    public static event BaseBloonDelegate bloonDeath;
    void Start()
    {
        GetComponent<BloonMovement>().SetSpeed(speed);
        bloonSprite = GetComponent<SpriteRenderer>().sprite;
        distance = 0.0f;
    }
    void Update()
    {
        distance += Time.deltaTime;
    }
    public void TakeDamage(int aDamage)
    {
        health -= aDamage;
        if(health <= 0)
        {
            //TODO: Bloon death animation
            int lPathPosition = GetComponent<BloonMovement>().GetPathPostion();
            bloonDeath?.Invoke(childCount, distance, lPathPosition, transform.position);
            BloonSpawner._instance.ReturnObjectToPool(gameObject);           
        }
    }
    public void SetDistance (float aDistance)
    {
        distance = aDistance;
    }
    /// <summary>
    /// Initializes bloons default variables.
    /// </summary>
    public abstract void InitializeBloon();
    /// <summary>
    /// Sets the sprite of the bloon.
    /// </summary>
    public abstract void SetSprite();
    public float GetBloonDistance()
    {
        return distance;
    }
    public bool GetIsStrong()
    {
        return isStrong;
    }
}
/// <summary>
/// Used for keeping bloon in order based on their distance. Used for tower priority targeting.
/// </summary>
public class DistanceComparer : IComparer<BaseBloon> 
{
    public int Compare(BaseBloon x, BaseBloon y)
    {
        if (x.GetBloonDistance() > y.GetBloonDistance()) return 1;
        if (x.GetBloonDistance() < y.GetBloonDistance()) return -1;
        return 0;
    }
}


