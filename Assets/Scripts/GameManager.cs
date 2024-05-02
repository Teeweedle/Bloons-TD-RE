using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _upgradePanel;
    private GameObject _selectedTower;

    // Start is called before the first frame update
    private void Start()
    {
    }
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
        _selectedTower = aTowerSelected;
        _upgradePanel.SetActive(true);
        //TODO: upgrade panel animation
        //Update update panel based on passed object
    }
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
