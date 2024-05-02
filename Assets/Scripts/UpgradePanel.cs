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
    [SerializeField] private Image[] _upgradeImg = new Image[3];
    [SerializeField] private TextMeshProUGUI[] _upgradeName = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] _upgradePrice = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] _upgradeText = new TextMeshProUGUI[3];

    public delegate void CloseWindowCallBack();
    public static event CloseWindowCallBack _onCloseWindow;
    
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
    public void InitPanelData(BaseTower aTower)
    {
        //TODO: Init data based on sprite name or GO name
    }
}
