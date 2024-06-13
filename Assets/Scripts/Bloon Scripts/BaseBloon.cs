using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBloon : MonoBehaviour
{
    [SerializeField] protected float distance;
    [SerializeField] protected bool isStrong, isCamo, isRegrow, isFortified;
    [SerializeField] protected float speed;
    [SerializeField] protected string childType;
    [SerializeField] protected int childCount, health, cash, xp;
    [SerializeField] protected SpriteRenderer bloonSpriteRender;

    public float mySpeed {
        get => speed;
        set { 
            if(value != speed)
            {
                speed = value;
                GetComponent<BloonMovement>().SetSpeed(speed);
            }
        }
    }

    private int lastProjectileHitID;
    private float immunityDuration;
    private List<IStatusEffect> activeStatusEffects;
    public delegate void BaseBloonDelegate(int aChildCount, float aDistance, int aPathPositon, Vector3 aBloonPosition, string aBloonType, int aProjectileID);
    public static event BaseBloonDelegate spawnChildren;

    public delegate void BaseBloonDeath(int aCash);
    public static event BaseBloonDeath bloonRewards;
    private void Awake()
    {
        bloonSpriteRender = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        distance = 0.0f;
    }
    void Update()
    {
        distance += Time.deltaTime;
        if(immunityDuration > 0.0f)
        {
            immunityDuration -= Time.deltaTime;
        }
    }
    public bool TakeDamage(int aDamage, int aProjectileID, BaseTower aParentTower)
    {
        if(lastProjectileHitID == aProjectileID && immunityDuration > 0f)
        {
            //providing immunity
            return false;
        }
        health -= aDamage;
        if (health <= 0)
        {
            //TODO: Bloon death animation
            //TODO: Pop sound
            aParentTower.NumBloonsPopped(xp);
            bloonRewards?.Invoke(cash);//updates UI with cash gain in GameManager
            int lPathPosition = GetComponent<BloonMovement>().GetPathPostion();
            //tells bloon spawner to check for children to spawn
            spawnChildren?.Invoke(childCount, distance, lPathPosition, transform.position, childType, aProjectileID);
            BloonSpawner._instance.ReturnObjectToPool(this.gameObject);           
        }
        return true;
    }
    public void ApplyStatusEffect (IStatusEffect aStatusEffect, BaseTower aParentTower)
    {
        activeStatusEffects.Add(aStatusEffect);
        aStatusEffect.Apply(this, aParentTower);
    }
    public void RemoveStatusEffect (IStatusEffect aStatusEffect)
    {
        aStatusEffect.Remove(this);
        activeStatusEffects.Remove(aStatusEffect);
    }
    /// <summary>
    /// Grants immunity to this bloon from the last projectile that hit it temporarily
    /// </summary>
    /// <param name="aProjectileID">ID of the projectile that hit it</param>
    /// <param name="aImmunityDuration">How long am I immune for</param>
    public void GrantImmunity(int aProjectileID, float aImmunityDuration)
    {
        lastProjectileHitID = aProjectileID;
        immunityDuration = aImmunityDuration;
    }
    /// <summary>
    /// Sets distance, how far along the path they are
    /// </summary>
    /// <param name="aDistance"></param>
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


