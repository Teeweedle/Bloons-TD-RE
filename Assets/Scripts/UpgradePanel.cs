using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Image _towerImg;
    [SerializeField] private TextMeshProUGUI _towerName;
    [SerializeField] private TextMeshProUGUI _towerXP, _sellPrice;
    [SerializeField] private UpgradeButton[] _upgrade = new UpgradeButton[3];

    public delegate void CloseWindowCallBack();
    public static event CloseWindowCallBack _onCloseWindow;

    private const float _SELLPRICEPERCENT = 0.7f;
        
    private void OnEnable()
    {
        GameManager._updatePanel += InitPanelData;
    }
    private void OnDisable()
    {
        GameManager._updatePanel -= InitPanelData;
    }
    // Start is called before the first frame update
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
    public void InitPanelData(TowerDataObject aTower)
    {
        //TODO: Init data based on sprite name or GO name
        _towerName.text = aTower.name;
        _towerXP.text = aTower.xp.ToString();
        _towerImg.sprite = Resources.Load<Sprite>($"Sprites/UI/Towers/{aTower.name}/No upgrades");
        UpdateUpgradeGrid(_upgrade, aTower);
        _sellPrice.text = ($"${GetSellPrice(aTower.cost)}");//TODO: Update to scale with difficulty
    }
    /// <summary>
    /// Iterates through an array of scripts (UpgradeButton) to update the UI based on the tower data
    /// </summary>
    /// <param name="aUpgrade"></param>
    /// <param name="aTower"></param>
    private void UpdateUpgradeGrid(UpgradeButton[] aUpgrade, TowerDataObject aTower)
    {
        for(int i =  0; i < _upgrade.Length; i++)
        {
            aUpgrade[i].UpdateUpgradeSection(aTower.name, aTower.upgradeLevelArray[i]);
            //if tower has an upgrade already update owned upgrade section
            aUpgrade[i].InitializeOwnedUpgrades(aTower.name, aTower.upgradeLevelArray[i]);
            //TODO: Tower upgrade levels aren't getting updated properly.
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
}
