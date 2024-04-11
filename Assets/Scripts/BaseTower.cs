using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public int _dmg;
    public int _pierce;
    public int _range;
    public float _attackSpeed;
    public bool _hasCamoDetection;

    private void Start()
    {
        _dmg = 0;
        _pierce = 0;
        _range = 0;
        _attackSpeed = 0;
        _hasCamoDetection = false;
        LoadTowerData("Dart Monkey", "No Upgrades");
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
    private void LoadTowerData(string aTowerType, string aTowerName)
    {
        try
        {
            string lTowerPath = Path.Combine(Application.dataPath, $"Tower Data/{aTowerType}/{aTowerName}.json");
            string lJsonString = File.ReadAllText(lTowerPath);

            TowerDataObject lTowerObject = JsonUtility.FromJson<TowerDataObject>(lJsonString);
            UpdateTowerStats(lTowerObject);
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"File not found: {ex.Message}");
        }
        
    }
    private void UpdateTowerStats(TowerDataObject aTowerObject)
    {
        _dmg = aTowerObject.damage;
        _pierce = aTowerObject.pierce;
        _range = aTowerObject.range;
        _attackSpeed = aTowerObject.attackSpeed / 100;
        _hasCamoDetection = aTowerObject.hasCamoDetection;
    }
}
