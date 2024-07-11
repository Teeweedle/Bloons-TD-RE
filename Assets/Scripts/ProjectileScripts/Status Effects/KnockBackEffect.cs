using System.Collections;
using UnityEngine;

public class KnockBackEffect : IStatusEffect
{
    private float myDuration;
    public KnockBackEffect(float aDuration)
    {
        myDuration = aDuration;
    }
    /// <summary>
    /// Knocks back the bloon, ie gives it a negative speed for a duration
    /// </summary>
    /// <param name="aBloon"></param>
    /// <param name="aParentTower"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Apply(BaseBloon aBloon, BaseTower aParentTower)
    {        
        aBloon.mySpeed = -aBloon.mySpeed;
        aBloon.StartCoroutine(RemoveAfterDuration(aBloon));
    }
    private IEnumerator RemoveAfterDuration(BaseBloon aBloon)
    {
        yield return new WaitForSeconds(myDuration);
        Remove(aBloon);
    }
    public void Remove(BaseBloon aBloon)
    {
        aBloon.mySpeed = -aBloon.mySpeed;
    }
    public void Update(BaseBloon aBloon)
    {
        //Not used
    }
}
