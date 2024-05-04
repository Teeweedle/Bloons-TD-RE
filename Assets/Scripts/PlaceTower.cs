using System.IO;
using Unity.Jobs;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _towerSpriteRenderer;
    [SerializeField] private SpriteRenderer _rangeSpriteRenderer;
    [SerializeField] private GameObject _towerRange;
    [SerializeField] private Collider2D _rangeCollider;

    private const string _DELIMITER = "Prefab";
    private const string _TOWERTYPE = "No Upgrades";
    private const string _NOBUILDTAG = "No Build";
    private const string _TOWERTAG = "Tower";

    private bool _canPlace = true;
    private BaseTower _baseTower;
    private TowerDataObject _towerData = new();

    private void Start()
    {
        _towerData = LoadTowerData(GetTowerName(this.gameObject.name, _DELIMITER), _TOWERTYPE);
        _baseTower = GetComponent<BaseTower>();
        DisplayTowerRange();
    }
    void Update()
    {        
        FollowMousePosition();

        if (_canPlace && Input.GetMouseButtonDown(0))
        {
            _baseTower.AssignStats(_towerData);
            TowerSelected._towerInstance = null;
            _rangeSpriteRenderer.enabled = false;
            _rangeCollider.enabled = true;
            Destroy(this);
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
        switch (collision.tag)
        {
            case _NOBUILDTAG:
            case _TOWERTAG:
                _canPlace = false;
                _towerSpriteRenderer.color = Color.red;
                break;
            default: break;
        }        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case _NOBUILDTAG:
            case _TOWERTAG:
                _canPlace = true;
                _towerSpriteRenderer.color = Color.white;
                break; 
            default: break;
        }
    }
    private void DisplayTowerRange()
    {
        _towerRange.transform.localScale *= _towerData.range;
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
    /// <summary>
    /// Loads a base tower GO based on the tower name
    /// </summary>
    /// <param name="aTowerName"></param>
    /// <param name="aTowerType"></param>
    /// <returns></returns>
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
