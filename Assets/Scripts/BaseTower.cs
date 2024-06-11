using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private Material _outlineShader, _defaultShader;
    [SerializeField] private SpriteRenderer _towerSpriteRenderer;
    [SerializeField] private GameObject _projectile;

    private bool _isPlaced, _isSelected;

    public TowerStats _towerStats;

    private string _targetPriority;
    private SortedSet<BaseBloon> _targetSortedList = new SortedSet<BaseBloon>(new DistanceComparer());
    private GameObject _currentTarget;
    private const float _angleOffset = 90f;
    private float _nextFireTime;

    private Dictionary<string, Action> _getTargetAction;
    private Action _cachedTargetAction;

    public delegate void TowerSelected(GameObject aTowerSelected);
    public static event TowerSelected _onTowerSelected;

    public delegate void UpdatePrice(int aPrice);
    public static event UpdatePrice _onUpdatePrice;

    public delegate void UpdateTowerXP(int aTowerXP);
    public static event UpdateTowerXP _onUpdateBloonsPopped;

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
        _nextFireTime = 0f;
        _isPlaced = false;
        _getTargetAction = new Dictionary<string, Action> {
            { "First", () => _currentTarget = GetFirstTarget() },
            { "Last", () => _currentTarget = GetLastTarget() },
            { "Close", () => _currentTarget = GetCloseTarget() },//TODO: Implement close behaviour
            { "Strong", () => _currentTarget = GetStrongTarget() }//TODO: Implement to check for strong bool

        };
        SetDefaultTargetPriority();
    }
    private void Update()
    {
        if (_isPlaced)
        {
            if (_currentTarget != null)
            {
                Fire(_currentTarget);
            }
        }
    }
    private void Fire(GameObject aProjectile, int aAmount)
    {
        //TODO: Implement
        for (int i = 0; i < aAmount; i++)
        {
            Instantiate(aProjectile, this.transform.position, Quaternion.identity);
        }
    }
    private void Fire(GameObject aTarget)
    {
        if (Time.time > _nextFireTime)
        {
            LookAtTarget(aTarget.transform);
            GameObject lProjectile = Instantiate(_projectile, transform.position, Quaternion.identity);
            BaseProjectile lProjectileScript = lProjectile.GetComponent<BaseProjectile>();
            if (lProjectileScript != null)
            {
                lProjectileScript.speed = 8f;//default for dart monkey
                lProjectileScript.health = _towerStats.pierce;
                lProjectileScript.damage = _towerStats.damage;
                lProjectileScript.lifeSpan = 0.75f;//default for dart monkey
                lProjectileScript.parentTower = this;
                lProjectileScript.SetDirection(-transform.up);
            }
            _nextFireTime = Time.time + _towerStats.attackSpeed;
        }        
    }
    /// <summary>
    /// Rotates the current gameObject to "look at" the targeted bloon.
    /// </summary>
    /// <param name="aTarget">The target transform</param>
    private void LookAtTarget(Transform aTarget)
    {
        Vector3 lDirection = aTarget.position - transform.position;
        float lAngle = Mathf.Atan2(lDirection.y, lDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, lAngle + _angleOffset));
    }
    /// <summary>
    /// Gets the bloon at the end of the sorted list.
    /// </summary>
    /// <returns>Bloon with the longest distance</returns>
    private GameObject GetFirstTarget()
    {
        return _targetSortedList.LastOrDefault().gameObject;
    }
    /// <summary>
    /// Gets the bloon that is at the start of the sorted list.
    /// <returns>Bloon with the shortest distance</returns>
    private GameObject GetLastTarget()
    {
        return _targetSortedList.FirstOrDefault().gameObject;
    }
    private GameObject GetStrongTarget()
    {
        return null;
    }
    private GameObject GetCloseTarget()
    {
        return null;
    }
    /// <summary>
    /// Assigns the new target to the target dictionary
    /// </summary>
    /// <param name="aTarget">Newly in range bloon</param>
    public void TargetAquired(Collider2D aTarget)
    {
        BaseBloon lBloon = aTarget.gameObject.GetComponent<BaseBloon>();
        _targetSortedList.Add(lBloon);
        _cachedTargetAction?.Invoke();
    }
    /// <summary>
    /// Removes the bloon from the target dictionary.
    /// </summary>
    /// <param name="aTarget">Bloon that moved out of range.</param>
    public void TargetLost(Collider2D aTarget)
    {
        _targetSortedList.Remove(aTarget.GetComponent<BaseBloon>());
        if(_targetSortedList.Count == 0)
            _currentTarget = null;
        else
            _cachedTargetAction?.Invoke();
    }
    /// <summary>
    /// Invoked from an event in ChangeTarget based on the priority that is chosen in the upgrade panel.
    /// </summary>
    /// <param name="aPriority"></param>
    private void SetTargetPriority(string aPriority)
    {
        _targetPriority = aPriority;
        UpdateCachedAction();
    }

    private void UpdateCachedAction()
    {
        if (_getTargetAction.TryGetValue(_targetPriority, out var action))
        {
            _cachedTargetAction = action;
        }
        else
            _cachedTargetAction = null;
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
    public void UpdateStats(TowerUpgrade aTowerUpgrade, int[] aUpgradeArray)
    {
        _towerStats.pierce += aTowerUpgrade.pierce;
        _towerStats.damage += aTowerUpgrade.damage;
        _towerStats.range += aTowerUpgrade.range;
        _towerStats.attackSpeed += aTowerUpgrade.attackSpeed;
        if (!_towerStats.hasCamoDetection)
        {
            _towerStats.hasCamoDetection = aTowerUpgrade.hasCamoDetection;
        }
        _towerStats.upgradeLevelArray = UpdateUpgradeArray(_towerStats.upgradeLevelArray, aUpgradeArray);

        _towerStats.cost += aTowerUpgrade.cost;
        _onUpdatePrice?.Invoke(_towerStats.cost);
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
    public TowerStats GetTowerStats() { return _towerStats; }
    public void SetTowerStats(TowerStats aTowerStats)
    {
        _towerStats = aTowerStats;
    }
    private void SetDefaultTargetPriority()
    {
        SetTargetPriority("First");
    }
    /// <summary>
    /// Stores XP gained through popping bloons in the TowerData Object.
    /// </summary>
    /// <param name="aNumBloonPopped">XP gained for popping a respective bloon.</param>
    public void NumBloonsPopped(int aNumBloonPopped)
    {
        _towerStats.numberOfBloonsPopped += aNumBloonPopped;
        //_towerData.numBloonsPopped += aNumBloonPopped;
        if(_isSelected)
        {
            _onUpdateBloonsPopped?.Invoke(_towerStats.numberOfBloonsPopped);
        }
    }
}
