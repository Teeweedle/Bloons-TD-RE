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
    [SerializeField] private Sprite MaxUpgradeSprite;

    private string TowerName;
    private TowerDataObject UpgradeData = new TowerDataObject();
    private const string UPGRADEPATH = "Sprites/UI/Tower Upgrade Panel";
    private int MaxUpgradeLevel;

    public delegate void UpgradePanelEvent();
    public static event UpgradePanelEvent UpdatePanel;

    private void Start()
    {
        MaxUpgradeLevel = 5;
    }
    public void UpdateUpgradeSection(string aTowerName, int aTowerUpgradeLevel)
    {
        TowerName = aTowerName;
        UpgradeData = LoadUpgrade(aTowerName, this.gameObject.name, aTowerUpgradeLevel + 1);
        UpgradeImage.sprite = LoadUpgradeSprite(aTowerName, aTowerUpgradeLevel);
        UpgradeName.text = UpgradeData.name;
        UpgradePrice.text = ($"${UpgradeData.cost}");

        InfoPanelName.text = UpgradeData.name;
        InfoPanelDescription.text = UpgradeData.description;

        //tell panel image to update
        UpdatePanel?.Invoke();
    }
    /// <summary>
    /// Called from UpgradePanel to initialize all potentially owned upgrades in the UI.
    /// </summary>
    /// <param name="aTowerName"></param>
    /// <param name="aTowerUpgradeLevel"></param>
    public void InitializeOwnedUpgrades(string aTowerName, int aTowerUpgradeLevel)
    {
        NotUpgradedText.SetActive(true);
        OwnedUpgrade.SetActive(false);
        RemoveUpgradePips(UpgradeContainer);

        if (aTowerUpgradeLevel > 0)
        {
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

    private void AddUpgradePips(GameObject aUpgradeContainer, int aTowerUpgradeLevel)
    {
        for (int i = 0; i < aTowerUpgradeLevel; i++)
        {
            Instantiate(UpgradeLevel, aUpgradeContainer.transform);
        }
    }

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
    /// When button pressed update to the next available upgrade
    /// </summary>
    public void UpgradeSelected()
    {
        int lUpgradeLevel = UpgradeContainer.transform.childCount;
        //TODO: Make sure you have enough money
        if (lUpgradeLevel < MaxUpgradeLevel)
        {            
            UpdateOwnedUpgrade(UpgradeName.text, UpgradeImage.sprite);
            
            //creates and array ex. 1-0-0 to add to the existing array (always a 1)
            int[] lUpgradeArray = GetUpgradeArray(this.gameObject.name, 1);

            //Update BaseTower with new stats and upgrade level
            //pass data to the upgrade panel
            UpgradePanel.UpgradeTower(UpgradeData, lUpgradeArray);

            //Add one pip to tracker
            Instantiate(UpgradeLevel, UpgradeContainer.transform);
            //Change image/ description to next level
            //          Make sure it doesn't break the 2/3 rule
            UpdateUpgradeSection(TowerName, UpgradeContainer.transform.childCount);          
        }
        lUpgradeLevel = UpgradeContainer.transform.childCount;
        if(lUpgradeLevel == MaxUpgradeLevel)
        {
            //Change button to max image
            ButtonImage.sprite = MaxUpgradeSprite;
            //Disable button
            GetComponent<Button>().interactable = false;
            //TODO: Disable 'left over upgrades'
            //Initialize buttons to default state
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
    /// <returns></returns>
    public int[] GetUpgradeArray(string aSlot, int aUpgradeLevel)
    {
        int aSlotNumber;
        int.TryParse(aSlot, out aSlotNumber);
        int[] lUpgradeArray = new int[3];
        lUpgradeArray[aSlotNumber - 1] = aUpgradeLevel;
        return lUpgradeArray;
    }
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
    public void SetMaxUpgradeLimit(int aMaxUpgradeLimit) 
    {
        MaxUpgradeLevel = aMaxUpgradeLimit;
    }
    public void DisableUpgradeTicks()
    {
        foreach(Image tick in UpgradeTicks)
        {
            Color color = tick.color;
            color.a = 0.5f;
            tick.color = color;
        }
    }
    public void EnableUpgradeTicks()
    {
        foreach (Image tick in UpgradeTicks)
        {
            Color color = tick.color;
            color.a = 1f;
            tick.color = color;
        }
    }
}
