using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private Material _outlineShader, _defaultShader;
    [SerializeField] private SpriteRenderer _towerSpriteRenderer;
    
    private bool _isPlaced, _isSelected;

    [SerializeField] private TowerDataObject _towerData = new();
    private string _targetPriority;
    [SerializeField] private Dictionary<GameObject, (float distance, bool isStrong)> _targetDictionary 
        = new Dictionary<GameObject, (float distance, bool isStrong)>();
    private GameObject _currentTarget;

    public delegate void TowerSelected(GameObject aTowerSelected);
    public static event TowerSelected _onTowerSelected;

    public delegate void UpdatePrice(int aPrice);
    public static event UpdatePrice _onUpdatePrice;

    private void OnEnable()
    {
        ChangeTarget.setTargetPriority += SetTargetPriority;
    }
    private void OnDisable()
    {
        ChangeTarget.setTargetPriority -= SetTargetPriority;
    }
    private void Start()
    {
        _isPlaced = false;
    }
    private void Update()
    {
        GetTarget("test");
    }
    private void Fire(GameObject aProjectile, int aAmount)
    {
        //TODO: Implement
        for (int i = 0; i < aAmount; i++)
        {
            Instantiate(aProjectile, this.transform.position, Quaternion.identity);
        }
    }
    private void GetTarget(string aTargetPriority)
    {
        //TODO: Implement
        switch (aTargetPriority)
        {
            case "First":
                GetFirst();
                break;
            case "Last":
                GetLast();
                break;
            case "Close"://TODO: Implement close behaviour
                break;
            case "Strong"://TODO: Implement to check for strong bool
                break;
            default:
                GetFirst();
                break;

        }
    }
    /// <summary>
    /// Gets the element in the dictionary that has been around the longest.
    /// </summary>
    /// <returns></returns>
    private GameObject GetFirst()
    {
        var lFirst = _targetDictionary.FirstOrDefault();
        var lLast = _targetDictionary.LastOrDefault();

        return lFirst.Value.distance < lLast.Value.distance ? lLast.Key : lFirst.Key;
    }
    /// <summary>
    /// Gets the eleme in the dictionary that has been around the shortest.
    /// </summary>
    /// <returns></returns>
    private GameObject GetLast()
    {
        var lFirst = _targetDictionary.FirstOrDefault();
        var lLast = _targetDictionary.LastOrDefault();

        return lFirst.Value.distance > lLast.Value.distance ? lLast.Key : lFirst.Key;
    }
    public void TargetAquired(Collider2D aTarget)
    {
        BaseBloon lBloon = aTarget.gameObject.GetComponent<BaseBloon>();
        _targetDictionary.Add(aTarget.gameObject, (lBloon.GetBloonDistance(), lBloon.GetIsStrong()));
        Debug.Log("Target found!");
    }   
    public void TargetLost(Collider2D aTarget)
    {
        _targetDictionary.Remove(aTarget.gameObject);
        Debug.Log("Target lost");
    }
    private void SetTargetPriority(string aPriority)
    {
        _targetPriority = aPriority;
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
    public void SetTowerSprite(Sprite aSprite)
    {
        _towerSpriteRenderer.sprite = aSprite;
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
