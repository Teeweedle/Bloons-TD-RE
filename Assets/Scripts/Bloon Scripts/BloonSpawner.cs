using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gOPath;
    [SerializeField] private GameObject _bloon;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private List<GameObject> _bloonScripts;
    [SerializeField] private BloonFactory _factory;
    
    public static BloonSpawner _instance;
    private int _numBloons;
    private float _spawnDelay;
    private Dictionary<string, Sprite> _spritesDictionary;
    private Dictionary<string, GameObject> _bloonScriptDictionary = new Dictionary<string, GameObject>();
    private List<Vector2> _bloonPath;
    [SerializeField] private Queue<GameObject> _bloonPool = new Queue<GameObject>();
    private void OnEnable()
    {
        BloonMovement._endOfPath += ReturnObjectToPool;
        BaseBloon.bloonDeath += SpawnChildren;
    }
    private void OnDisable()
    {
        BloonMovement._endOfPath -= ReturnObjectToPool;
        BaseBloon.bloonDeath -= SpawnChildren;
    }
    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        //default rnd 1 20, 0.85
        SetSpawnInfo(20, 0.85f);
        LoadBloonScripts();
        LoadSprites();
        _bloonPath = _gOPath.GetComponent<BloonPathCreator>().GetPathVectors();
    }
    /// <summary>
    /// Loads a list of scripts (for bloon type behaviour) into a dictionary for 0(n) look uptime to attach them to
    /// repurposed game objects.
    /// </summary>
    private void LoadBloonScripts()
    {
        string lScriptName;
        foreach (var script in _bloonScripts)
        {
            lScriptName = script.name;
            _bloonScriptDictionary[lScriptName] = script;
        }
    }
    /// <summary>
    /// Gets the script for the behaviour of the passed Name
    /// </summary>
    /// <param name="aScriptName">Name of the script</param>
    /// <returns>The script.</returns>
    public MonoBehaviour GetBloonScript(string aScriptName)
    {
        if(_bloonScriptDictionary.TryGetValue(aScriptName, out var bloonScript))
        {
            var lScriptInstance = bloonScript.GetComponent<MonoBehaviour>();
            return lScriptInstance;
        }
        Debug.LogError($"Script not found{aScriptName}");
        return null;
    }
    /// <summary>
    /// Gets a bloon from a queue of already instantiated bloons, if there isn't one available it instantiates a new one.
    /// </summary>
    /// <returns>A Bloon prefab</returns>
    public GameObject GetBloon(string aBloonType)
    {
        GameObject lBloon;
        if(_bloonPool.Count > 0)
        {
            lBloon = _bloonPool.Dequeue();
            lBloon.SetActive(true);            
        }
        else
        {
            lBloon = Instantiate(_bloon);
        }
        _factory.AssignBloonScirpt(lBloon, aBloonType);
        return lBloon;
    }
    /// <summary>
    /// Returns a bloon to the pool, deactivates it, and removes behavior script.
    /// </summary>
    /// <param name="aBloon"></param>
    public void ReturnObjectToPool(GameObject aBloon)
    {
        _factory.RemoveBloonScript(_bloon);
        aBloon.SetActive(false);
        _bloonPool.Enqueue(aBloon);
    }

    public Sprite GetSpriteByName(string aSpriteName)
    {
        if (_spritesDictionary.TryGetValue(aSpriteName, out var sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogWarning($"Sprite not found {aSpriteName}");
            return null;
        }
    }
    /// <summary>
    /// Spawn bloons at the start of the path
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnBloons(int aNumBloons, float aSpawnDelay, string aBloonType)
    {
        GameObject lBloon;
        for(int i = 0; i < aNumBloons;  i++)
        {
            lBloon = GetBloon(aBloonType);
            lBloon.transform.position = _bloonPath[0];
            lBloon.transform.rotation = Quaternion.identity;
            lBloon.GetComponent<BloonMovement>().SetPath(_bloonPath);
            lBloon.GetComponent<BloonMovement>().SetPathPosition(0);
            lBloon.GetComponent<BaseBloon>().SetDistance(0);
            yield return new WaitForSeconds(aSpawnDelay);
        }
    }
    /// <summary>
    /// Spawns children bloons
    /// </summary>
    /// <param name="aChildCount">How many children</param>
    /// <param name="aDistance">How far long the path</param>
    /// <param name="aPathPosition">What position on that path is the bloon</param>
    /// <param name="aPosition">Game world position</param>
    /// <returns></returns>
    public IEnumerator SpawnChildren(int aChildCount, float aDistance, int aPathPosition, Vector3 aPosition, string aBloonType)
    {
        GameObject lBloon;
        for (int i = 0; i < aChildCount; i++)
        {
            lBloon = GetBloon(aBloonType);
            lBloon.transform.position = aPosition;
            lBloon.transform.rotation = Quaternion.identity;
            lBloon.GetComponent<BloonMovement>().SetPathPosition(aPathPosition);
            lBloon.GetComponent<BaseBloon>().SetDistance(aDistance);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void LoadSprites()
    {
        _spritesDictionary = new Dictionary<string, Sprite>();
        foreach(var sprite in _sprites)
        {
            _spritesDictionary[sprite.name] = sprite;
        }
    }
    public void StartRound()
    {
        //TODO: Toggle button to increase game speed while round is active (not just bloons).
        //TODO: Change to get info passed from round info
        StartCoroutine(SpawnBloons(_numBloons, _spawnDelay, "Blue Bloon"));
    }
    public void SetSpawnInfo(int aNumOfBloons, float aSpawnDelay)
    {
        _numBloons = aNumOfBloons;
        _spawnDelay = aSpawnDelay;
    }
}
