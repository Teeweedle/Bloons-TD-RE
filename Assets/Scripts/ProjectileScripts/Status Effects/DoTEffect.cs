using System.Collections;
using UnityEngine;

public class DoTEffect : IStatusEffect
{
    private int damage;
    private float duration;
    private float tickRate;
    public DoTEffect(int aDamage, float aDuration, float aTickRate)
    {
        damage = aDamage;
        duration = aDuration;
        tickRate = aTickRate;
    }
    public void Apply(BaseBloon aBloon, BaseTower aParentTower)
    {
        aBloon.StartCoroutine(ApplyDoT(aBloon, aParentTower));
    }
    private IEnumerator ApplyDoT(BaseBloon aBloon, BaseTower aParentTower)
    {
        float lElapsedTime = 0;
        while (lElapsedTime < duration)
        {
            aBloon.TakeDamage(damage, 0, aParentTower);
            lElapsedTime += tickRate;
            yield return new WaitForSeconds(tickRate);
        }
    }
    public void Remove(BaseBloon aBloon)
    {
        throw new System.NotImplementedException();
    }

    public void Update(BaseBloon aBloon)
    {
        throw new System.NotImplementedException();
    }
}
