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

    private TowerDataObject UpgradeData = new TowerDataObject();
    private const string UPGRADEPATH = "Sprites/UI/Tower Upgrade Panel";
    public void UpdateUpgradeSection(string aTowerName, int aTowerUpgradeLevel)
    {
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
