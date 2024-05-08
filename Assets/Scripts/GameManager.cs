using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _upgradePanel;
    private GameObject _selectedTower;

    public delegate void UpdatePanel(TowerDataObject aTower);
    public static event UpdatePanel _updatePanel;

    private void OnEnable()
    {
        BaseTower._onTowerSelected += TowerSelected;
        UpgradePanel._onCloseWindow += UnselectTower;
    }
    private void OnDisable()
    {
        BaseTower._onTowerSelected -= TowerSelected;
        UpgradePanel._onCloseWindow -= UnselectTower;
    }
    private void TowerSelected(GameObject aTowerSelected)
    {
        //keeps track of current selected tower
        _selectedTower = aTowerSelected;
        //show upgrade panel
        _upgradePanel.SetActive(true);
        //load selected tower into Upgrade Panel UI
        _updatePanel?.Invoke(aTowerSelected.GetComponent<BaseTower>().GetTowerData());
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
}
