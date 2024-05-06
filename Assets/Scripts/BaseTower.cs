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
    private void OnEnable()
    {
        UpgradeButton.UpdateTower += UpdateStats;
    }
    private void OnDisable()
    {
        UpgradeButton.UpdateTower -= UpdateStats;
    }
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
    public void UpdateStats(TowerDataObject aData, int[] aUpgradeArray)
    {
        _towerData.pierce += aData.pierce;
        _towerData.damage += aData.damage;
        _towerData.range += aData.range;
        _towerData.attackSpeed += aData.attackSpeed;
        _towerData.hasCamoDetection = aData.hasCamoDetection;
        _towerData.upgradeLevelArray = UpdateUpgradeArray(_towerData.upgradeLevelArray, aUpgradeArray);

    }

    private int[] UpdateUpgradeArray(int[] aOriginalArray, int[] aUpdatedArray)
    {
        int[] newArray = new int[aOriginalArray.Length];
        for (int i = 0; i < newArray.Length; i++ ) {
            if (aUpdatedArray[i] > 0)
                newArray[i] = aUpdatedArray[i];
            else
                newArray[i] = aOriginalArray[i];
        }

        
        return newArray;
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
