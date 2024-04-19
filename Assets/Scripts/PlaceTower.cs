using System.IO;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _towerRange;

    private readonly string _DELIMITER = "Prefab";
    private readonly string _TOWERTYPE = "No Upgrades";

    private bool _canPlace = true;
    private GameObject _towerRangeGO;
    private BaseTower _tower;
    private TowerDataObject _towerData = new();

    private void Start()
    {
        _towerData = LoadTowerData(GetTowerName(this.gameObject.name, _DELIMITER), _TOWERTYPE);
        _tower = GetComponent<BaseTower>();
        DisplayTowerRange();
    }
    void Update()
    {        
        FollowMousePosition();

        if (_canPlace)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _tower.AssignStats(_towerData);
                Destroy(_towerRangeGO);
                Destroy(this);
            } 
        }
    }
    private void FollowMousePosition()
    {
        transform.position = Camera.main.ScreenToWorldPoint(GetMousePosition());
    }

    private Vector3 GetMousePosition()
    {
        Vector3 lMousePosition = Input.mousePosition;
        lMousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        return lMousePosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _canPlace = false;
        _spriteRenderer.color = Color.red;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _canPlace = true;
        _spriteRenderer.color = Color.white;
    }
    private void DisplayTowerRange()
    {
        _towerRangeGO = Instantiate(_towerRange, this.transform);
        _towerRangeGO.transform.position = this.transform.position;
        _towerRangeGO.transform.localScale = _towerRangeGO.transform.localScale * _towerData.range;
    }
    private string GetTowerName(string aPrefabName, string aDelimiter)
    {
        int lDelimiterIndex = aPrefabName.IndexOf(aDelimiter);
        if (lDelimiterIndex != -1)
        {
            return aPrefabName.Substring(0, lDelimiterIndex).Trim();
        }
        else
        {
            return aPrefabName;
        }
    }
    private TowerDataObject LoadTowerData(string aTowerName, string aTowerType)
    {
        TowerDataObject lTowerObject = new();
        try
        {
            string lTowerPath = Path.Combine(Application.dataPath, $"Tower Data/{aTowerName}/{aTowerType}.json");
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
