using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    private float _range;
    private float _dmg;
    private float _atckSpeed;

    private void Fire(GameObject aProjectile, int aAmount)
    {
        for (int i = 0; i < aAmount; i++)
        {
            Instantiate(aProjectile, this.transform.position, Quaternion.identity);
        }
    }
}
