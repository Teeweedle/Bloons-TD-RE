using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Image UpgradeImage;
    [SerializeField] private TextMeshProUGUI UpgradeName;
    [SerializeField] private TextMeshProUGUI UpgradePrice;
    [SerializeField] private TextMeshProUGUI UpgradeDescription;
    [SerializeField] private GameObject UpgradeLevel;
    [SerializeField] private GameObject UpgradeContainer;

    public delegate void UpdateBaseTower(TowerDataObject UpgradeData, int[] UpgradeLevel);
    public static event UpdateBaseTower UpdateTower;

    private string TowerName;
    private TowerDataObject UpgradeData = new TowerDataObject();
    private const string UPGRADEPATH = "Sprites/UI/Tower Upgrade Panel";
    public void UpdateUpgradeSection(string aTowerName, int aTowerUpgradeLevel)
    {
        TowerName = aTowerName;
        UpgradeData = LoadUpgrade(aTowerName, this.gameObject.name, aTowerUpgradeLevel + 1);
        UpgradeImage.sprite = Resources.Load<Sprite>($"{UPGRADEPATH}/{aTowerName}/{this.gameObject.name}/" +
            $"{aTowerUpgradeLevel + 1}_{UpgradeData.name}");
        UpgradeName.text = UpgradeData.name;
        UpgradePrice.text = UpgradeData.cost.ToString();
        UpgradeDescription.text = UpgradeData.description;
    }
    /// <summary>
    /// When button pressed update to the next available upgrade
    /// </summary>
    public void UpgradeSelected()
    {
        int lUpgradeLevel = UpgradeContainer.transform.childCount;
        //TODO: Make sure you have enough money
        if (lUpgradeLevel < 5)
        {
            //Add one pip to tracker
            Instantiate(UpgradeLevel, UpgradeContainer.transform);
            //Change image/ description to next level
            //          Make sure it doesn't break the 2/3 rule
            UpdateUpgradeSection(TowerName, UpgradeContainer.transform.childCount);
            //Update BaseTower with new stats and upgrade level
            int[] lUpgradeArray = GetUpgradeArray(this.gameObject.name, lUpgradeLevel + 1);
            UpdateTower?.Invoke(UpgradeData, lUpgradeArray);
        }
        //TODO: Update sell price

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
    private TowerDataObject LoadUpgrade(string aTowerName, string aUpgradePath, int aCurretUpgrade)
    {
        TowerDataObject lTowerObject = new();
        try
        {
            string lTowerPath = Path.Combine(Application.dataPath, $"Tower Data/{aTowerName}/{aUpgradePath}/{aCurretUpgrade}.json");
            string lJsonString = File.ReadAllText(lTowerPath);

            lTowerObject = JsonUtility.FromJson<TowerDataObject>(lJsonString);
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"File not found: {ex.Message}");
        }
        return lTowerObject;
    }
}
