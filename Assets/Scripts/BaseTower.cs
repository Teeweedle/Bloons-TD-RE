using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private Material _outlineShader, _defaultShader;
    [SerializeField] private SpriteRenderer _towerSpriteRenderer;
    [SerializeField] public GameObject _projectile;
    public Sprite _currrentProjectileSprite { get; private set; }
    private bool _isPlaced, _isSelected;

    public TowerStats _towerStats;

    private string _targetPriority;
    private SortedSet<BaseBloon> _targetSortedList = new SortedSet<BaseBloon>(new DistanceComparer());
    private GameObject _currentTarget;
    private const float angleOffset = 90f;
    public float NextFireTime { get; set; }
    public ITowerBehavior towerBehavior;
    public CompositeProjectileBehavior compositeProjectileBehavior;

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
        NextFireTime = 0f;
        _isPlaced = false;
        _getTargetAction = new Dictionary<string, Action> {
            { "First", () => _currentTarget = GetFirstTarget() },
            { "Last", () => _currentTarget = GetLastTarget() },
            { "Close", () => _currentTarget = GetCloseTarget() },//TODO: Implement close behaviour
            { "Strong", () => _currentTarget = GetStrongTarget() }//TODO: Implement to check for strong bool

        };
        SetDefaultTargetPriority();
        SetDefaultProjectileTexture();
        compositeProjectileBehavior = new CompositeProjectileBehavior();
        compositeProjectileBehavior.SetProjectileBehavior(new SingleShot());
        
        towerBehavior = new DefaultShot(compositeProjectileBehavior);
    }
    private void Update()
    {
        if (_isPlaced)
        {
            if (_currentTarget != null)
            {
                towerBehavior.Fire(_currentTarget, this);
            }
        }
    }
    /// <summary>
    /// Rotates the current gameObject to "look at" the targeted bloon.
    /// </summary>
    /// <param name="aTarget">The target transform</param>
    public void LookAtTarget(Transform aTarget)
    {
        Vector3 lDirection = aTarget.position - transform.position;
        float lAngle = Mathf.Atan2(lDirection.y, lDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, lAngle + angleOffset));
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
    /// <summary>
    /// Updates the stats of the tower based on the associated Scriptable Object
    /// </summary>
    /// <param name="aTowerUpgrade">The Scriptable Object that contains the new stats</param>
    /// <param name="aUpgradeArray">Current upgrade level of the tower</param>
    public void UpdateStats(TowerUpgrade aTowerUpgrade, int[] aUpgradeArray)
    {
        UpdatePierce(aTowerUpgrade.pierce);
        UpdateDamage(aTowerUpgrade.damage);
        UpdateRange(aTowerUpgrade.range);
        UpdateProjectileSpeed(aTowerUpgrade.projectileSpeed);
        UpdateProjectileSprite(aTowerUpgrade.upgradeName);
        UpdateAttackSpeed(aTowerUpgrade.attackSpeed);
        UpdateProjectileLifeSpan(aTowerUpgrade.projectileLifeSpan);
        UpdateCamoDetection(aTowerUpgrade.hasCamoDetection);
        UpdateCost(aTowerUpgrade.cost);
        UpdateProjectileBehavior(aTowerUpgrade);
        _towerStats.upgradeLevelArray = UpdateUpgradeArray(_towerStats.upgradeLevelArray, aUpgradeArray);

        _onUpdatePrice?.Invoke(_towerStats.cost);
    }
    /// <summary>
    /// Updates the sprite of the tower based on the name of the upgrade. Only changes if there is a sprite associated with the upgrade
    /// </summary>
    /// <param name="aUpgradeName"></param>
    private void UpdateProjectileSprite(string aUpgradeName)
    {
        Sprite lSprite = _towerStats.projectileSpriteAtlas.GetSprite(aUpgradeName);
        if (lSprite != null)
        {
            _currrentProjectileSprite = lSprite;
        }
    }

    private void UpdateProjectileBehavior(TowerUpgrade aTowerUpgrade)
    {
        if(!string.IsNullOrEmpty(aTowerUpgrade.projectileBehaviorName))
            compositeProjectileBehavior.SetProjectileBehavior(ProjectileBehaviorFactory.CreateBehavior(aTowerUpgrade));
    }

    private void UpdatePierce(int aPierce) 
    { 
        _towerStats.pierce += aPierce;
    }
    private void UpdateDamage(int aDamage) 
    { 
        _towerStats.damage += aDamage; 
    }
    private void UpdateRange(float aRange) 
    { 
        if(aRange != 0)
            _towerStats.range *= (1.0f * aRange); 
    }
    private void UpdateProjectileSpeed(float aProjectileSpeed) 
    { 
        if(aProjectileSpeed != 0)
            _towerStats.projectileSpeed *= (1.0f + aProjectileSpeed); 
    }
    private void UpdateAttackSpeed(float aAttackSpeed) 
    { 
        if(aAttackSpeed != 0)
            _towerStats.attackSpeed *= (1.0f - aAttackSpeed); 
    }
    private void UpdateProjectileLifeSpan(float aProjectileLifeSpan) 
    { 
        if(aProjectileLifeSpan != 0)
            _towerStats.projectileLifeSpan *= (1.0f + aProjectileLifeSpan); 
    }
    private void UpdateCamoDetection(bool aCamoDetection) 
    {
        if (!_towerStats.hasCamoDetection)
        {
            _towerStats.hasCamoDetection = aCamoDetection;
        }
    }
    private void UpdateCost(int aCost) 
    { 
        _towerStats.cost += aCost; 
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
    public void SetTowerStats(TowerStats aTowerStats){ _towerStats = aTowerStats; }
    private void SetDefaultTargetPriority(){ SetTargetPriority("First"); }
    private void SetDefaultProjectileTexture()
    {
        _currrentProjectileSprite = _towerStats.projectileSpriteAtlas.GetSprite("Base");
    }
    /// <summary>
    /// Stores XP gained through popping bloons in the TowerData Object.
    /// </summary>
    /// <param name="aNumBloonPopped">XP gained for popping a respective bloon.</param>
    public void NumBloonsPopped(int aNumBloonPopped)
    {
        _towerStats.numberOfBloonsPopped += aNumBloonPopped;
        if(_isSelected)
        {
            _onUpdateBloonsPopped?.Invoke(_towerStats.numberOfBloonsPopped);
        }
    }
}
