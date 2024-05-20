using System;
using System.IO;
using TMPro;
using Unity.VisualScripting;
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
    private TowerDataObject UpgradeData = new TowerDataObject();
    private const string UPGRADEPATH = "Sprites/UI/Tower Upgrade Panel";
    private int MaxUpgradeLevel, Three = 3;

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
    }
    public void UpdateUpgradeSection(string aTowerName, int aTowerUpgradeLevel)
    {
        TowerName = aTowerName;
        if(aTowerUpgradeLevel < MaxUpgradeLevelProp)
            UpgradeData = LoadUpgrade(aTowerName, this.gameObject.name, aTowerUpgradeLevel + 1);
        UpgradeImage.sprite = LoadUpgradeSprite(aTowerName, aTowerUpgradeLevel);
        UpgradeName.text = UpgradeData.name;
        UpgradePrice.text = ($"${UpgradeData.cost}");

        InfoPanelName.text = UpgradeData.name;
        InfoPanelDescription.text = UpgradeData.description;

        //tell panel to update tower image to update
        UpdatePanel?.Invoke();
    }
    /// <summary>
    /// Called from UpgradePanel to initialize all potentially owned upgrades in the UI.
    /// </summary>
    /// <param name="aTowerName"></param>
    /// <param name="aTowerUpgradeLevel"></param>
    public void InitializeOwnedUpgrades(string aTowerName, int aTowerUpgradeLevel)
    {
        ResetUpgradeUI();

        if (aTowerUpgradeLevel <= 0) return;

        if (aTowerUpgradeLevel >= Three)
            SetUpgradeLimit?.Invoke(int.Parse(gameObject.name));

        LoadAndSetUpgradeSprite(aTowerName, aTowerUpgradeLevel);

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

    private Sprite LoadUpgradeSprite(string aTowerName, int aTowerUpgradeLevel)
    {
        return Resources.Load<Sprite>($"{UPGRADEPATH}/{aTowerName}/{this.gameObject.name}/" +
            $"{aTowerUpgradeLevel + 1}_{UpgradeData.name}");
    }

    /// <summary>
    /// When button pressed update the towers stats and show the next available upgrade.
    /// </summary>
    public void UpgradeSelected()
    {
        int lUpgradeLevel = GetCurrentUpgradeLevel();
        
        //TODO: Make sure you have enough money                  
        UpdateOwnedUpgrade(UpgradeName.text, UpgradeImage.sprite);            
        //creates and array ex. 1-0-0 to add to the existing array (always a 1)
        int[] lUpgradeArray = GetUpgradeArray(this.gameObject.name, 1);
        UpgradePanel.UpgradeTowerStats(UpgradeData, lUpgradeArray);
        //Add one pip to tracker
        Instantiate(UpgradeLevel, UpgradeContainer.transform);
        int lNewUpgradeLevel = lUpgradeLevel + 1;
        //Change image/ description to next level
        if (lNewUpgradeLevel != MaxUpgradeLevelProp)
            UpdateUpgradeSection(TowerName, lNewUpgradeLevel);
        else if(lNewUpgradeLevel == MaxUpgradeLevelProp)
            UpdatePanel?.Invoke();
            
        if (lNewUpgradeLevel == Three)
        {
            SetUpgradeLimit?.Invoke(int.Parse(gameObject.name));
        }
        //Disable button at max level available
        if (lNewUpgradeLevel == MaxUpgradeLevelProp)
        {
            DisableUpgradeButton();
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
    /// Loads an upgrade from a JSON file based on the name and the upgrade level.
    /// </summary>
    /// <param name="aTowerName"></param>
    /// <param name="aUpgradePath"></param>
    /// <param name="aCurrentUpgrade"></param>
    /// <returns></returns>
    private TowerDataObject LoadUpgrade(string aTowerName, string aUpgradePath, int aCurrentUpgrade)
    {
        TowerDataObject lTowerObject = new();
        try
        {
            string lTowerPath = Path.Combine(Application.dataPath, $"Tower Data/{aTowerName}/{aUpgradePath}/{aCurrentUpgrade}.json");
            string lJsonString = File.ReadAllText(lTowerPath);

            lTowerObject = JsonUtility.FromJson<TowerDataObject>(lJsonString);
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"File not found: {ex.Message}");
        }
        return lTowerObject;
    } 
    /// <summary>
    /// Disable upgrade ticks past a certain level, based on the 2/3 rule.
    /// </summary>
    public void DisableUpgradeTicks()
    {
        foreach(Image tick in UpgradeTicks)
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
        if(GetCurrentUpgradeLevel() >= MaxUpgradeLevelProp)
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
    private int GetCurrentUpgradeLevel()
    {
        return UpgradeContainer.transform.childCount;
    }
    private void ResetUpgradeUI()
    {
        NotUpgradedText.SetActive(true);
        OwnedUpgrade.SetActive(false);
        EnableUpgradeButton();
        RemoveUpgradePips(UpgradeContainer);
    }
    private void LoadAndSetUpgradeSprite(string aTowerName, int aTowerUpgradeLevel)
    {
        //Loads tower upgrades for this tree (based on GO name)
        Sprite[] UpgradeSprites = Resources.LoadAll<Sprite>($"{UPGRADEPATH}/{aTowerName}/{this.gameObject.name}");

        foreach (Sprite sprite in UpgradeSprites)
        {
            if (sprite.name[0].ToString() == aTowerUpgradeLevel.ToString())
            {
                int lDelimiterIndex = sprite.name.IndexOf('_');
                string lUpgradeName = sprite.name.Substring(lDelimiterIndex + 1);
                UpdateOwnedUpgrade(lUpgradeName, sprite);
                AddUpgradePips(UpgradeContainer, aTowerUpgradeLevel);
                break;
            }
        }
    }
}
