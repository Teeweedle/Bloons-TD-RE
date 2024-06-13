using System.Collections;
using UnityEngine;

public class SlowEffect : IStatusEffect
{
    private float slowAmount;
    private float slowDuration;
    public SlowEffect(float aSlowAmount, float aSlowDuration)
    {
        slowAmount = aSlowAmount;
        slowDuration = aSlowDuration;
    }
    public void Apply(BaseBloon aBloon, BaseTower aParentTower)
    {
        aBloon.mySpeed *= slowAmount;

        aBloon.StartCoroutine(RemoveAfterDuration(aBloon));
    }

    public void Remove(BaseBloon aBloon)
    {
        aBloon.mySpeed /= slowAmount;
    }
    public IEnumerator RemoveAfterDuration(BaseBloon aBloon)
    {
        yield return new WaitForSeconds(slowDuration);
        Remove(aBloon);
    }

    public void Update(BaseBloon aBloon)
    {
        throw new System.NotImplementedException();
    }

    
}
