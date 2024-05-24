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
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BloonMovement>().SetSpeed(speed);
        distance = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        distance += Time.deltaTime;
    }
    public float GetBloonDistance()
    {
        return distance;
    }
    public bool GetIsStrong()
    {
        return isStrong;
    }
    private void OnDestroy()
    {
        //TODO: Spawn child 
        //Pass current position on the path
        //Pass distance variable
    }
    public abstract void IntializeBloon();
    public abstract void SetSprite();
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


