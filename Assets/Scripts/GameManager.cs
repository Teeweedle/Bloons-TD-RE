using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private TextMeshProUGUI _playerHPText, _playerMoneyText;
    
    private GameObject _selectedTower;
    private int _playerHP, _playerMoney;

    public delegate void UpdatePanel(TowerDataObject aTower, GameObject aCurrentTower);
    public static event UpdatePanel _updatePanel;

    private void OnEnable()
    {
        BaseTower._onTowerSelected += TowerSelected;
        BaseBloon.bloonRewards += UpdatePlayerMoney;
        UpgradePanel._onCloseWindow += UnselectTower;
        UpgradePanel._changeSprite += ChangeTowerSprite;
    }
    private void OnDisable()
    {
        BaseTower._onTowerSelected -= TowerSelected;
        BaseBloon.bloonRewards -= UpdatePlayerMoney;
        UpgradePanel._onCloseWindow -= UnselectTower;
        UpgradePanel._changeSprite -= ChangeTowerSprite;
    }    
    private void Start()
    {
        _playerMoney = 100;
        _playerHP = 100;

        UpdatePlayerHPText();
        UpdatePlayerMoneyText();
    }
    private void TowerSelected(GameObject aTowerSelected)
    {
        //keeps track of current selected tower
        _selectedTower = aTowerSelected;
        //show upgrade panel
        _upgradePanel.SetActive(true);
        //load selected tower into Upgrade Panel UI
        _updatePanel?.Invoke(aTowerSelected.GetComponent<BaseTower>().GetTowerData(), _selectedTower);
        //TODO: upgrade panel animation
    }
    /// <summary>
    /// Called from event (clicking the 'x') or pressing ESC
    /// TODO: Implement calling when clicking anywhere but a tower
    /// </summary>
    private void UnselectTower()
    {
        if (_selectedTower != null)
        {
            _selectedTower.GetComponent<BaseTower>().UnHighLight();
            _upgradePanel.SetActive(false);
        }
        _selectedTower = null;
        //TODO: Hide upgrade panel/ animation
    }
    /// <summary>
    /// Event triggered from UpgadePanel to change the tower sprite to the highest upgrade level.
    /// </summary>
    /// <param name="aTowerSprite">Highest upgrade level sprite.</param>
    private void ChangeTowerSprite(Sprite aTowerSprite)
    {
        _selectedTower.GetComponent<BaseTower>().SetTowerSprite(aTowerSprite);
    }
    /// <summary>
    /// Updates player health, ends game if HP if <= 0
    /// </summary>
    /// <param name="aHealthChange">Amnount to change the player health</param>
    private void UpdatePlayerHP(int aHealthChange)
    {
        _playerHP -= aHealthChange;
        UpdatePlayerHPText();
        if (_playerHP <= 0)
        {
            //TODO: End game
        }
    }
    /// <summary>
    /// Updates player HP on the UI
    /// </summary>
    /// <param name="aHealth">Current value of player HP</param>
    private void UpdatePlayerHPText()
    {
        _playerHPText.text += _playerHP.ToString();
    }
    /// <summary>
    /// Updates player money 
    /// </summary>
    /// <param name="aMoneyChange">Amount to change player money, can be negative</param>
    private void UpdatePlayerMoney(int aMoneyChange)
    {
        _playerMoney += aMoneyChange;
        UpdatePlayerMoneyText();
    }
    /// <summary>
    /// Updates player money on the UI
    /// </summary>
    /// <param name="aMoney">Current value of player money</param>
    private void UpdatePlayerMoneyText()
    {
        _playerMoneyText.text = ($"${_playerMoney}");
    }
}
