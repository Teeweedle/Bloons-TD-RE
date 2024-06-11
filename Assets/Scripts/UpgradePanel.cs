using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Image _towerImg;
    [SerializeField] private TextMeshProUGUI _towerName;
    [SerializeField] private TextMeshProUGUI _numBloonsPopped, _sellPrice;
    [SerializeField] private UpgradeButton[] _upgrade = new UpgradeButton[3];
    [SerializeField] private Image[] _pathClosed = new Image[3];

    public delegate void CloseWindowCallBack();
    public static event CloseWindowCallBack _onCloseWindow;

    public delegate void ChangeTowerSprite(Sprite aTowerSprite);
    public static event ChangeTowerSprite _changeSprite;

    private GameObject _currentTower;
    private const float _SELLPRICEPERCENT = 0.7f;
    private const int _MAXUPGRADELIMIT = 5;


    private void OnEnable()
    {
        GameManager._updatePanel += InitializeUpgradePanel;
        UpgradeButton.UpdatePanel += UpdateImage;
        UpgradeButton.SetUpgradeLimit += SetUpgradeLimit;
        BaseTower._onUpdateBloonsPopped += UpdateTowerNumBloonsPopped;
    }
    private void OnDisable()
    {
        GameManager._updatePanel -= InitializeUpgradePanel;
        UpgradeButton.UpdatePanel -= UpdateImage;
        UpgradeButton.SetUpgradeLimit -= SetUpgradeLimit;
        BaseTower._onUpdateBloonsPopped -= UpdateTowerNumBloonsPopped;
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
        Destroy(_currentTower);
        gameObject.SetActive(false);
        //TODO: Return money/ update money UI
    }
    /// <summary>
    /// Initializes data for upgrade panel based on the tower clicked
    /// </summary>
    /// <param name="aTower"></param>
    public void InitializeUpgradePanel(TowerStats aTower, GameObject aSelectedTower)
    {
        _currentTower = aSelectedTower;
        _towerName.text = aTower.towerName;
        _numBloonsPopped.text = $"{aTower.numberOfBloonsPopped}";
        _sellPrice.text = ($"${GetSellPrice(aTower.cost)}");//TODO: Update to scale with difficulty
        SetTowerImage(aTower);

        InitializeUpgradeGrid(_upgrade, aTower);
    }
    /// <summary>
    /// Passes the selected upgrade data and the upgrade array to the currently selected tower to upgrade it.
    /// </summary>
    /// <param name="aTowerUpgrade"></param>
    /// <param name="aUpgradeArray"></param>
    public void UpgradeTowerStats(TowerUpgrade aTowerUpgrade, int[] aUpgradeArray)
    {
        _currentTower.GetComponent<BaseTower>().UpdateStats(aTowerUpgrade, aUpgradeArray);
    }
    /// <summary>
    /// Iterates through an array of scripts (UpgradeButton) to update the UI based on the tower data
    /// </summary>
    /// <param name="aUpgradeButtons">Array of Upgrade buttons (3)</param>
    /// <param name="aTower">The current tower</param>
    private void InitializeUpgradeGrid(UpgradeButton[] aUpgradeButtons, TowerStats aTower)
    {
        var (lBiggestUpgade, lIndex) = GetUpgradeAndIndex(aTower.upgradeLevelArray);
        bool lHasUpgradeLimit = lBiggestUpgade >= 3;

        ResetUpgradeLimit(aUpgradeButtons);

        for (int i =  0; i < aUpgradeButtons.Length; i++)
        {
            aUpgradeButtons[i].SetCurrentTower(aTower);
            if (lHasUpgradeLimit && i != lIndex)
                aUpgradeButtons[i].MaxUpgradeLevelProp = 2;            
            aUpgradeButtons[i].UpdateUpgradeSection(aTower.name, aTower.upgradeLevelArray[i]);
            //if tower has an upgrade already update owned upgrade section
            aUpgradeButtons[i].InitializeOwnedUpgrades(aTower, aTower.upgradeLevelArray[i]);
        }
    }
    /// <summary>
    /// Resets all upgrade paths to allow for the maximum upgrade limit.
    /// Only use on initialization
    /// </summary>
    /// <param name="aUpgradeButons">Array of the upgrade paths</param>
    private void ResetUpgradeLimit(UpgradeButton[] aUpgradeButons)
    {
        foreach(var aUpgradeItem in aUpgradeButons)
        {
            aUpgradeItem.MaxUpgradeLevelProp = _MAXUPGRADELIMIT;
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
    private void SetTowerImage(TowerStats aTower)
    {
        var (aBiggestUpgrade, aIndex) = GetUpgradeAndIndex(aTower.upgradeLevelArray);
        if (aBiggestUpgrade == 0)
            _towerImg.sprite = aTower.towerSprite;
        else
        {
            LoadUpgradeSprite(aTower, aIndex, aBiggestUpgrade);
        }
    }
    /// <summary>
    /// Loads the upgrade spite into the upgrade panel and triggers an event to update the GO sprite.
    /// </summary>
    /// <param name="aTower"></param>
    /// <param name="aPathIndex"></param>
    /// <param name="aUpgradeIndex"></param>
    private void LoadUpgradeSprite(TowerStats aTower, int aPathIndex, int aUpgradeIndex)
    {
        _towerImg.sprite = aTower.upgradePath[aPathIndex].upgrades[aUpgradeIndex-1].towerImageSprite;
        _changeSprite?.Invoke(aTower.upgradePath[aPathIndex].upgrades[aUpgradeIndex-1].GOSprite);
    } 
    /// <summary>
    /// Called from an event in UpgradeButton to update the image on the panel when a new upgrade is purchased. Verifies the 
    /// tower is only upgraded in 2 of the 3 trees.
    /// </summary>
    private void UpdateImage()
    {
        TowerStats lTowerData = _currentTower.GetComponent<BaseTower>().GetTowerStats();
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
                lIndex = i;
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
            DisableUpgradeTree(aUpgradeArray);
            return true; 
        }
        else
        {
            EnableAllUpgradeTrees();
        }
        return false;
    }
    /// <summary>
    /// Hides all **Path Closed** images so you can see all upgrade trees.
    /// </summary>
    private void EnableAllUpgradeTrees()
    {
        foreach (Image aImg in _pathClosed)
        {
            aImg.GetComponent<PathClosed>().EnableButton();
            aImg.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Shows the **Path Closed** image over a given tree (with no upgrades) to follow the 2/3 rule.
    /// </summary>
    /// <param name="aUpgradeArray">Current tower upgrade array</param>
    private void DisableUpgradeTree(int[] aUpgradeArray)
    {
        EnableAllUpgradeTrees();
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
    /// <summary>
    /// Sets max upgrade limit for each upgrade tree.
    /// </summary>
    /// <param name="aUpgradePath">Tree with the maximum allowed upgrade path</param>
    private void SetUpgradeLimit(int aUpgradePath)
    {
        //Disable top 3 tick trackers for upgrade level for all other
        
        //Send max upgrade tracker to OTHER buttons to limit upgades
        for(int i = 0; i < _upgrade.Length; i++)
        {
            if(i != aUpgradePath)
            {
                _upgrade[i].MaxUpgradeLevelProp = 2;
            }
        }
    }
    /// <summary>
    /// Updates the towers collected xp during the round, used to upgrade the tower.
    /// </summary>
    /// <param name="aNumBloonsPopped">XP gained from popping bloons.</param>
    private void UpdateTowerNumBloonsPopped(int aNumBloonsPopped)
    {
        _numBloonsPopped.text = $"{aNumBloonsPopped}";
    }
}
