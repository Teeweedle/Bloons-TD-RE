using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] private TowerStats[] towerStats;
    [SerializeField] private GameObject towerPrefab;
    private Dictionary<string, TowerStats> towersDictionary; 
    public static TowerFactory Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            InitializeTowerDictionary();
        }
    }
    /// <summary>
    /// Initializes the dictionary from towerSO assigned in the inspector.
    /// </summary>
    private void InitializeTowerDictionary()
    {
        towersDictionary = new Dictionary<string, TowerStats>();
        foreach (var tower in towerStats)
        {
            towersDictionary.Add(tower.name, tower);
        }
    }
    /// <summary>
    /// Gets a tower from the dictionary based on towerName. Assigns stats to it, assigns sprite, and returns it.
    /// </summary>
    /// <param name="towerName"></param>
    /// <returns></returns>
    public GameObject GetTower(string towerName)
    {
        if (towersDictionary.ContainsKey(towerName))
        {
            GameObject lTower = Instantiate(towerPrefab, Input.mousePosition, Quaternion.identity);
            lTower.name = towerName;
            TowerStats lNewTowerStats = ScriptableObjectUtility.Clone(towersDictionary[towerName]);
            lTower.GetComponent<BaseTower>().SetTowerStats(lNewTowerStats);
            lTower.GetComponent<SpriteRenderer>().sprite = towersDictionary[towerName].towerGOSprite;
            return lTower;
        }
        return null;
    }
}
