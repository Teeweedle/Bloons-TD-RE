using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] private TowerSO[] towerSO;
    [SerializeField] private GameObject towerPrefab;
    private Dictionary<string, TowerSO> towersDictionary; 
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
        towersDictionary = new Dictionary<string, TowerSO>();
        foreach (var tower in towerSO)
        {
            towersDictionary.Add(tower.name, tower);
        }
    }
    /// <summary>
    /// Gets a tower from the dictionary based on towerName.
    /// </summary>
    /// <param name="towerName"></param>
    /// <returns></returns>
    public GameObject GetTower(string towerName)
    {
        if (towersDictionary.ContainsKey(towerName))
        {
            GameObject lTower = Instantiate(towerPrefab, Input.mousePosition, Quaternion.identity);
            lTower.name = towerName;
            lTower.GetComponent<SpriteRenderer>().sprite = towersDictionary[towerName].towerSprite;
            return lTower;
        }
        return null;
    }
}
