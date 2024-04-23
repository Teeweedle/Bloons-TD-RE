using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public string _type, _targetType;
    public int _dmg, _pierce, _range, _xp;
    public float _attackSpeed;
    public bool _hasCamoDetection;

    private void Start()
    {
    }
    private void Fire(GameObject aProjectile, int aAmount)
    {
        for (int i = 0; i < aAmount; i++)
        {
            Instantiate(aProjectile, this.transform.position, Quaternion.identity);
        }
    }
    private void TargetBloon(string aTargetType)
    {

    }
    public void HighLight()
    {

    }
    public void UnHighLight()
    {

    }
    public void AssignStats(TowerDataObject aData)
    {
        _type = aData.type;
        _attackSpeed = aData.attackSpeed;
        _dmg = aData.damage;
        _pierce = aData.pierce;
        _range = aData.range;
        _hasCamoDetection = aData.hasCamoDetection;
    }
}
