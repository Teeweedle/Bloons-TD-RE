using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Image UpgradeImage, ButtonImage;
    [SerializeField] private TextMeshProUGUI UpgradeName;
    [SerializeField] private TextMeshProUGUI UpgradePrice;
    [SerializeField] private GameObject UpgradeLevel;
    [SerializeField] private GameObject UpgradeContainer;
    [SerializeField] private GameObject OwnedUpgrade, NotUpgradedText;
    [SerializeField] private TextMeshProUGUI OwnedUpgradeName;
    [SerializeField] private Image OwnedUpgradeImage;
    [SerializeField] private TextMeshProUGUI InfoPanelName, InfoPanelDescription;
    [SerializeField] private UpgradePanel UpgradePanel;
    [SerializeField] private Image[] UpgradeTicks = new Image[3];
    [SerializeField] private Sprite MaxUpgradeSprite, UpgradeButtonSprite;

    private string TowerName;
    private int MaxUpgradeLevel, Three = 3;
    private TowerStats CurrentTower;
    private TowerUpgrade NextUpgrade;
    private int UpgradePathIndex;
    private int CurrentUpgradeLevel;

    public int MaxUpgradeLevelProp 
    { 
        get => MaxUpgradeLevel; 
        set { 
            if(value !=  MaxUpgradeLevel)
            {
                MaxUpgradeLevel = value;
                if (value < Three)
                {
                    DisableUpgradeTicks();
                }else
                    EnableUpgradeTicks();
            }
        } 
    }

    public delegate void UpgradePanelEvent();
    public static event UpgradePanelEvent UpdatePanel;

    public delegate void UpgradeLimit(int aUpgradePath);
    public static event UpgradeLimit SetUpgradeLimit;
  
    private void Start()
    {
        MaxUpgradeLevelProp = 5;
        UpgradePathIndex = GetUpgradePathIndex();
    }
    public void UpdateUpgradeSection(string aTowerName, int aTowerUpgradeLevel)
    {
        TowerName = aTowerName;
        if(aTowerUpgradeLevel < MaxUpgradeLevelProp)
            NextUpgrade = GetNextUpgrade(aTowerUpgradeLevel);

        UpgradeImage.sprite = NextUpgrade.upgradeSprite;
        UpgradeName.text = NextUpgrade.upgradeName;
        UpgradePrice.text = ($"${NextUpgrade.cost}");

        InfoPanelName.text = NextUpgrade.name;
        InfoPanelDescription.text = NextUpgrade.description;

        //tell panel to update tower image to update
        UpdatePanel?.Invoke();
    }
    /// <summary>
    /// Gets the next upgrade in the upgrade path.
    /// </summary>
    /// <param name="aIndex"></param>
    /// <returns></returns>
    public TowerUpgrade GetNextUpgrade(int aIndex)
    {
        return CurrentTower.upgradePath[UpgradePathIndex].upgrades[aIndex];
    }
    /// <summary>
    /// Sets the current tower, intialized in UpgradePanel.
    /// </summary>
    /// <param name="aTower">Currently selected tower data.</param>
    public void SetCurrentTower(TowerStats aTower)
    {
        CurrentTower = aTower;
        CurrentUpgradeLevel = aTower.upgradeLevelArray[UpgradePathIndex];
    }
    /// <summary>
    /// Called from UpgradePanel to initialize all potentially owned upgrades in the UI.
    /// </summary>
    /// <param name="aTowerName"></param>
    /// <param name="aTowerUpgradeLevel"></param>
    public void InitializeOwnedUpgrades(TowerStats aTower, int aTowerUpgradeLevel)
    {
        ResetUpgradeUI();

        if (aTowerUpgradeLevel <= 0) return;

        if (aTowerUpgradeLevel >= Three)
            SetUpgradeLimit?.Invoke(UpgradePathIndex);

        LoadAndSetUpgradeSprite(CurrentTower);

        if (aTowerUpgradeLevel == MaxUpgradeLevelProp)
            DisableUpgradeButton();
    }
    private void AddUpgradePips(GameObject aUpgradeContainer, int aTowerUpgradeLevel)
    {
        for (int i = 0; i < aTowerUpgradeLevel; i++)
        {
            Instantiate(UpgradeLevel, aUpgradeContainer.transform);
        }
        if(aTowerUpgradeLevel == MaxUpgradeLevelProp)
            DisableUpgradeButton();
        else
            EnableUpgradeButton();
    }
    /// <summary>
    /// Removes upgrade pips, used when resetting an upgrade tree (on loading a new tower).
    /// </summary>
    /// <param name="aUpgradeContainer"></param>
    private void RemoveUpgradePips(GameObject aUpgradeContainer)
    {
        for (int i = 0; i < aUpgradeContainer.transform.childCount; i++)
        {
            Transform lChild = aUpgradeContainer.transform.GetChild(i);
            Destroy(lChild.gameObject);
        }
    }
    /// <summary>
    /// When button pressed update the towers stats and show the next available upgrade.
    /// </summary>
    public void UpgradeSelectedOnClick()
    {
        int lUpgradeLevel = GetCurrentUpgradeLevel();
        
        //TODO: Make sure you have enough money                  
        UpdateOwnedUpgrade(UpgradeName.text, UpgradeImage.sprite);            
        //creates and array ex. 1-0-0 to add to the existing array (always a 1)
        int[] lUpgradeArray = GetUpgradeArray(this.gameObject.name, 1);
        UpgradePanel.UpgradeTowerStats(NextUpgrade, lUpgradeArray);
        //Add one pip to tracker
        Instantiate(UpgradeLevel, UpgradeContainer.transform);
        int lNewUpgradeLevel = lUpgradeLevel + 1;

        //Disable button at max level available
        if (lNewUpgradeLevel == MaxUpgradeLevelProp)
        {
            DisableUpgradeButton();
        }
        //Change image/ description to next level
        if (lNewUpgradeLevel != MaxUpgradeLevelProp)
            UpdateUpgradeSection(TowerName, lNewUpgradeLevel);
        else if(lNewUpgradeLevel == MaxUpgradeLevelProp)
            UpdatePanel?.Invoke();
            
        if (lNewUpgradeLevel == Three)
        {
            SetUpgradeLimit?.Invoke(UpgradePathIndex);
        }    
    }
    /// <summary>
    /// Updates the owned upgrade section of the UI with the name of the upgade and the image
    /// </summary>
    /// <param name="aUpgradeName"></param>
    /// <param name="aUpgradeSprite"></param>
    private void UpdateOwnedUpgrade(string aUpgradeName, Sprite aUpgradeSprite)
    {
        NotUpgradedText.SetActive(false);
        OwnedUpgrade.SetActive(true);
        OwnedUpgradeName.text = aUpgradeName;
        OwnedUpgradeImage.sprite = aUpgradeSprite;
    }
    private void UpdateOwnedUpgrade(TowerUpgrade aUpgrade)
    {
        NotUpgradedText.SetActive(false);
        OwnedUpgrade.SetActive(true);
        OwnedUpgradeName.text = aUpgrade.upgradeName;
        OwnedUpgradeImage.sprite = aUpgrade.upgradeSprite;
    }


    /// <summary>
    /// Creates the an upgrade array based on the objects name and current upgrade tier of the selected object.
    /// Ex. 0-2-0
    /// Used to update the BaseTower.
    /// </summary>
    /// <param name="aSlot"></param>
    /// <param name="aUpgradeLevel"></param>
    /// <returns>An upgrade array based on the slot Ex. 1-0-0</returns>
    public int[] GetUpgradeArray(string aSlot, int aUpgradeLevel)
    {
        int aSlotNumber;
        int.TryParse(aSlot, out aSlotNumber);
        int[] lUpgradeArray = new int[3];
        lUpgradeArray[aSlotNumber - 1] = aUpgradeLevel;
        return lUpgradeArray;
    }
    /// <summary>
    /// Gets the current upgrade level of the selected path based on the button this is attached to.
    /// </summary>
    /// <returns></returns>
    private int GetCurrentUpgradeLevel()
    {
        return CurrentTower.upgradeLevelArray[UpgradePathIndex];
    }
    private void ResetUpgradeUI()
    {
        NotUpgradedText.SetActive(true);
        OwnedUpgrade.SetActive(false);
        EnableUpgradeButton();
        RemoveUpgradePips(UpgradeContainer);
    }

    private void LoadAndSetUpgradeSprite(TowerStats aTower)
    {
        TowerUpgrade lUpgrade = aTower.upgradePath[UpgradePathIndex].upgrades[CurrentUpgradeLevel-1];
        UpdateOwnedUpgrade(lUpgrade);
        AddUpgradePips(UpgradeContainer, CurrentUpgradeLevel);        
    }
    /// <summary>
    /// Disable upgrade ticks past a certain level, based on the 2/3 rule.
    /// </summary>
    public void DisableUpgradeTicks()
    {
        foreach (Image tick in UpgradeTicks)
        {
            Color color = tick.color;
            color.a = 0.5f;
            tick.color = color;
        }
    }
    /// <summary>
    /// Enables upgrade ticks, used for resetting when loading a new tower.
    /// </summary>
    public void EnableUpgradeTicks()
    {
        foreach (Image tick in UpgradeTicks)
        {
            Color color = tick.color;
            color.a = 1f;
            tick.color = color;
        }
    }
    /// <summary>
    /// Turns off the images and texts over an upgrade button, used when max upgrades are reached.
    /// </summary>
    public void DisableUpgradeButton()
    {
        if (GetCurrentUpgradeLevel() >= MaxUpgradeLevelProp)
        {
            UpgradeImage.enabled = false;
            UpgradeName.enabled = false;
            UpgradePrice.enabled = false;
            //Change button to max image
            ButtonImage.sprite = MaxUpgradeSprite;
            //Disable button
            GetComponent<Button>().interactable = false;
        }
    }
    /// <summary>
    /// Turns on the images and texts over and upgrade button, used to reset upgrade buttons when loading a new tower.
    /// </summary>
    private void EnableUpgradeButton()
    {
        UpgradeImage.enabled = true;
        UpgradeName.enabled = true;
        UpgradePrice.enabled = true;
        ButtonImage.sprite = UpgradeButtonSprite;
        GetComponent<Button>().interactable = true;
    }
    private int GetUpgradePathIndex()
    {
        if (int.TryParse(this.gameObject.name[0].ToString(), out int lPath))
        {
            lPath = lPath - 1;
        }
        return lPath;
    }
}
