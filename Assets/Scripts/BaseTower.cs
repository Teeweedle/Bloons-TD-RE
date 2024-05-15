using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private Material _outlineShader, _defaultShader;
    [SerializeField] private SpriteRenderer _towerSpriteRenderer;
    
    private bool _isPlaced, _isSelected;

    [SerializeField] private TowerDataObject _towerData = new();

    public delegate void TowerSelected(GameObject aTowerSelected);
    public static event TowerSelected _onTowerSelected;

    public delegate void UpdatePrice(int aPrice);
    public static event UpdatePrice _onUpdatePrice;

    public delegate void MaxUpgrade();
    public static event MaxUpgrade _onMaxUpgrade;
   
    private void Start()
    {
        _isPlaced = false;
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
        _towerSpriteRenderer.material = _outlineShader;
        _isSelected = true;
    }
    public void UnHighLight()
    {
        _towerSpriteRenderer.material = _defaultShader;
        _isSelected = false;
    }
    /// <summary>
    /// Assign loaded stats to this tower and make upgrade array an actual array
    /// </summary>
    /// <param name="aData"></param>
    public void AssignStats(TowerDataObject aData)
    {
        _towerData = aData;

        _towerData.upgradeLevelArray = _towerData.upgradeLevel.Select(c => int.Parse(c.ToString())).ToArray();
    }
    public void UpdateStats(TowerDataObject aUpgradeData, int[] aUpgradeArray)
    {
        _towerData.pierce += aUpgradeData.pierce;
        _towerData.damage += aUpgradeData.damage;
        _towerData.range += aUpgradeData.range;
        _towerData.attackSpeed += aUpgradeData.attackSpeed;
        if (!_towerData.hasCamoDetection)
        {
            _towerData.hasCamoDetection = aUpgradeData.hasCamoDetection;
        }
        _towerData.upgradeLevelArray = UpdateUpgradeArray(_towerData.upgradeLevelArray, aUpgradeArray);

        _towerData.cost += aUpgradeData.cost;
        _onUpdatePrice?.Invoke(_towerData.cost);
    }
    private int[] UpdateUpgradeArray(int[] aOriginalArray, int[] aUpdatedArray)
    {
        for (int i = 0; i < aOriginalArray.Length; i++)
        {
            aOriginalArray[i] += aUpdatedArray[i];
            if (aOriginalArray[i] >= 3)
                _onMaxUpgrade?.Invoke();
        }
        return aOriginalArray;
    }
    public void OnMouseDown()
    {
        if(_isPlaced && !_isSelected)
        {
            HighLight();
            _onTowerSelected?.Invoke(this.gameObject);
        }
        _isPlaced = true;
    }    
    public TowerDataObject GetTowerData() { return _towerData; }
}
