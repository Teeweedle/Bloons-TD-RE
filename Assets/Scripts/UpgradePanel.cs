using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BaseTower;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Image _towerImg;
    [SerializeField] private TextMeshProUGUI _towerName;
    [SerializeField] private TextMeshProUGUI _towerXP, _sellPrice;
    [SerializeField] private UpgradeButton[] _upgrade = new UpgradeButton[3];
    [SerializeField] private Image[] _pathClosed = new Image[3];

    public delegate void CloseWindowCallBack();
    public static event CloseWindowCallBack _onCloseWindow;

    private GameObject _currentTower;
    private const float _SELLPRICEPERCENT = 0.7f;
    private const string _TOWERPATH = "Sprites/UI/Towers";


    private void OnEnable()
    {
        GameManager._updatePanel += InitPanelData;
        UpgradeButton.UpdatePanel += UpdateImage;
        BaseTower._onMaxUpgrade += MaxUpgradeReached;
    }
    private void OnDisable()
    {
        GameManager._updatePanel -= InitPanelData;
        UpgradeButton.UpdatePanel -= UpdateImage;
        BaseTower._onMaxUpgrade -= MaxUpgradeReached;
    }
    void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void CloseWindow()
    {
        _onCloseWindow?.Invoke();        
    }
    public void SellTower()
    {
        //TODO: Sell tower logic
    }
    /// <summary>
    /// Initializes data for upgrade panel based on the tower clicked
    /// </summary>
    /// <param name="aTower"></param>
    public void InitPanelData(TowerDataObject aTower, GameObject aSelectedTower)
    {
        if (!aTower.maxTree)
        {
            aTower.maxTree = CheckUpgradeLevel(aTower.upgradeLevelArray);
        }
        else
        {
            DisableTree(aTower.upgradeLevelArray);
        }
        _currentTower = aSelectedTower;
        _towerName.text = aTower.name;
        _towerXP.text = aTower.xp.ToString();
        //TODO: Update based on current tower upgrade level (set to the image of the highest current upgrade)
        SetTowerImage(aTower);
        //_towerImg.sprite = Resources.Load<Sprite>($"Sprites/UI/Towers/{aTower.name}/No upgrades");
        UpdateUpgradeGrid(_upgrade, aTower);
        _sellPrice.text = ($"${GetSellPrice(aTower.cost)}");//TODO: Update to scale with difficulty
    }
    /// <summary>
    /// Passes the selected upgrade data and the upgrade array to the currently selected tower to upgrade it.
    /// </summary>
    /// <param name="aUpgrade"></param>
    /// <param name="aUpgradeArray"></param>
    public void UpgradeTower(TowerDataObject aUpgrade, int[] aUpgradeArray)
    {
        _currentTower.GetComponent<BaseTower>().UpdateStats(aUpgrade, aUpgradeArray);
    }
    /// <summary>
    /// Iterates through an array of scripts (UpgradeButton) to update the UI based on the tower data
    /// </summary>
    /// <param name="aUpgrade">Array of Upgrade buttons (3)</param>
    /// <param name="aTower">The current tower</param>
    private void UpdateUpgradeGrid(UpgradeButton[] aUpgrade, TowerDataObject aTower)
    {
        for(int i =  0; i < _upgrade.Length; i++)
        {
            aUpgrade[i].UpdateUpgradeSection(aTower.name, aTower.upgradeLevelArray[i]);
            //if tower has an upgrade already update owned upgrade section
            aUpgrade[i].InitializeOwnedUpgrades(aTower.name, aTower.upgradeLevelArray[i]);
        }
    }
    /// <summary>
    /// Tower resale value is at 70% of the base cost (TODO: Changes based on difficulty)
    /// </summary>
    /// <param name="aCost"></param>
    /// <returns></returns>
    private string GetSellPrice(int aCost)
    {
        return Mathf.Round((aCost * _SELLPRICEPERCENT)).ToString();
    }
    /// <summary>
    /// Sets the tower image based on the highest currently owned upgrade.
    /// </summary>
    /// <param name="aTower">Currently selected tower data.</param>
    private void SetTowerImage(TowerDataObject aTower)
    {
        var (aBiggestUpgrade, aIndex) = GetUpgradeAndIndex(aTower.upgradeLevelArray);
        if (aBiggestUpgrade == 0)
            _towerImg.sprite = Resources.Load<Sprite>($"{_TOWERPATH}/{aTower.name}/No upgrades");
        else
        {
            LoadUpgradeSprite(aTower.name, aIndex, aBiggestUpgrade);
        }
    }
    /// <summary>
    /// Loads all sprites in a given upgrade track. Then matches the current upgrade level with the first char of a sprite and loads it.
    /// </summary>
    /// <param name="aTowerName">A tower name.</param>
    /// <param name="aUpgradeTrack">Highest currently upgraded track.</param>
    /// <param name="aUpgradeLevel">Current upgrade level for a given track.</param>
    private void LoadUpgradeSprite(string aTowerName, int aUpgradeTrack, int aUpgradeLevel)
    {
        Sprite[] UpgradeSprites = Resources.LoadAll<Sprite>($"{_TOWERPATH}/{aTowerName}/{aUpgradeTrack}/");
        foreach (Sprite sprite in UpgradeSprites)
        {
            if (sprite.name[0].ToString() == aUpgradeLevel.ToString())
            {
                _towerImg.sprite = sprite;
                return;//sprite found
            }
        }
    }
    /// <summary>
    /// Called from an event in UpgradeButton to update the image on the panel when a new upgrade is purchased. Verifies the 
    /// tower is only upgraded in 2 of the 3 trees.
    /// </summary>
    private void UpdateImage()
    {
        TowerDataObject lTowerData = _currentTower.GetComponent<BaseTower>().GetTowerData();
        if (lTowerData.maxTree)
        {
            DisableTree(lTowerData.upgradeLevelArray);
        }else
            CheckUpgradeLevel(lTowerData.upgradeLevelArray);
        SetTowerImage(lTowerData);
    }
    /// <summary>
    /// Gets the highest currently owned upgrade and its index in the currently selected towers upgrade array.
    /// </summary>
    /// <param name="aUpgradeLevelArray">Currently selected towers upgrade array.</param>
    /// <returns>A tuple containing the largest upgrade and its index</returns>
    private (int aBiggestUpgrade, int aIndex) GetUpgradeAndIndex(int[] aUpgradeLevelArray)
    {
        int lBiggestUpgrade = -1, lIndex = -1;
        for (int i = 0; i < aUpgradeLevelArray.Length; i++)
        {
            if (aUpgradeLevelArray[i] > lBiggestUpgrade)
            {
                lBiggestUpgrade = aUpgradeLevelArray[i];
                lIndex = i + 1;
            }
        }
        return (lBiggestUpgrade, lIndex);
    }
    /// <summary>
    /// Checks Upgrade Array to make sure tower is only upgrades in 2 of the 3 trees.
    /// </summary>
    /// <param name="aUpgradeArray">Upgrade level of the currently selected tower.</param>
    private bool CheckUpgradeLevel(int[] aUpgradeArray)
    {
        if(aUpgradeArray[0] > 0 && aUpgradeArray[1] > 0 ||
            aUpgradeArray[0] > 0 && aUpgradeArray[2] > 0 ||
            aUpgradeArray[1] > 0 && aUpgradeArray[2] > 0) 
        {
            DisableTree(aUpgradeArray);
            return true; 
        }
        else
        {
            ShowAllTrees();
        }
        return false;
    }

    private void ShowAllTrees()
    {
        foreach (Image aImg in _pathClosed)
        {
            aImg.GetComponent<PathClosed>().EnableButton();
            aImg.gameObject.SetActive(false);
        }
    }

    private void DisableTree(int[] aUpgradeArray)
    {
        ShowAllTrees();
        for (int i = 0; i < aUpgradeArray.Length; i++)
        {
            if(aUpgradeArray[i] == 0)
            {
                _pathClosed[i].GetComponent<PathClosed>().DisableButton();
                _pathClosed[i].gameObject.SetActive(true);
                break;//found close tree, return
            }
        }             
    }
    private void MaxUpgradeReached()
    {
        //Disable top 3 tick trackers for upgrade level for all other
        
        //Send max upgrade tracker to OTHER buttons to limit upgades
        for(int i = 0; i < _upgrade.Length; i++)
        {
            _upgrade[i].DisableUpgradeTicks();
            _upgrade[i].SetMaxUpgradeLimit(2);
        }
    }
}
