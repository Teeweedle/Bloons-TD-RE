using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BloonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gOPath;
    [SerializeField] private GameObject _bloon;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private List<GameObject> _bloonScripts;
    [SerializeField] private BloonFactory _factory;

    // TEST INFO
    public TextMeshProUGUI _time;
    private bool _roundStarted = false;
    // TEST INFO

    public static BloonSpawner _instance;
    private int _numBloons;
    private float _spawnDelay;
    private Dictionary<string, Sprite> _spritesDictionary;
    private Dictionary<string, GameObject> _bloonScriptDictionary = new Dictionary<string, GameObject>();
    private List<Vector2> _bloonPath;
    private float _immunityDuration = 0.5f;
    [SerializeField] private Queue<GameObject> _bloonPool = new Queue<GameObject>();
    private void OnEnable()
    {
        BloonMovement._endOfPath += ReturnObjectToPool;
        BaseBloon.spawnChildren += SpawnChildrenHandler;
    }
    private void OnDisable()
    {
        BloonMovement._endOfPath -= ReturnObjectToPool;
        BaseBloon.spawnChildren -= SpawnChildrenHandler;
    }
    private void Update()
    {
        if (_roundStarted)
            _time.text = $"{Time.time}";
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
        _factory.RemoveBloonScript(aBloon);
        _bloonPool.Enqueue(aBloon);
        aBloon.SetActive(false);
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
            //Debug.Log($"Parent {i} instance ID {lBloon.GetInstanceID()}");
            InitializeBloon(lBloon, _bloonPath[0], Quaternion.identity, 0f, 0);
            yield return new WaitForSeconds(aSpawnDelay);
        }
    }
    private void SpawnChildrenHandler(int aChildCount, float aDistance, int aPathPosition, Vector3 aPosition, string aBloonType,
        int aProjectileID)
    {
        StartCoroutine(SpawnChildren(aChildCount, aDistance, aPathPosition, aPosition, aBloonType, aProjectileID));
    }
    /// <summary>
    /// Spawns children bloons
    /// </summary>
    /// <param name="aChildCount">How many children</param>
    /// <param name="aDistance">How far long the path</param>
    /// <param name="aPathPosition">What position on that path is the bloon</param>
    /// <param name="aPosition">Game world position</param>
    /// <returns></returns>
    public IEnumerator SpawnChildren(int aChildCount, float aDistance, int aPathPosition, Vector3 aPosition, string aBloonType, 
        int aProjectileID)
    {
        GameObject lBloon;
        for (int i = 0; i < aChildCount; i++)
        {
            lBloon = GetBloon(aBloonType);
            //Debug.Log($"Child instance ID {lBloon.GetInstanceID()}");
            InitializeBloon(lBloon, aPosition, Quaternion.identity, aDistance, aPathPosition);
            lBloon.GetComponent<BaseBloon>().GrantImmunity(aProjectileID, _immunityDuration);
            yield return new WaitForSeconds(0.1f);
        }
    }
    /// <summary>
    /// Initializes a newly spawned bloon.
    /// </summary>
    /// <param name="aBloon">The Game Object</param>
    /// <param name="aBloonPosition">Game world coords</param>
    /// <param name="aRotation">Rotation of Game Object</param>
    /// <param name="aDistance">How far along the Game Object is on the path</param>
    /// <param name="aPathPosition">What node is the Game Object traveling too next</param>
    private void InitializeBloon(GameObject aBloon, Vector3 aBloonPosition, Quaternion aRotation, float aDistance, int aPathPosition)
    {
        aBloon.transform.position = aBloonPosition;
        aBloon.transform.rotation = aRotation;

        var lBloonMovement = aBloon.GetComponent<BloonMovement>();
        lBloonMovement.SetPath(_bloonPath);
        lBloonMovement.SetPathPosition(aPathPosition);

        var lBaseBloon = aBloon.GetComponent<BaseBloon>();
        lBaseBloon.SetDistance(aDistance);
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
        _roundStarted = true;
    }
    public void SetSpawnInfo(int aNumOfBloons, float aSpawnDelay)
    {
        _numBloons = aNumOfBloons;
        _spawnDelay = aSpawnDelay;
    }
}
