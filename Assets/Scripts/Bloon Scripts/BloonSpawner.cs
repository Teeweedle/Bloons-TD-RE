using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gOPath;
    [SerializeField] private GameObject _bloon;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private List<GameObject> _bloonScripts;
    
    public static BloonSpawner _instance;
    private Dictionary<string, Sprite> _spritesDictionary;
    private Dictionary<string, GameObject> _bloonScriptDictionary = new Dictionary<string, GameObject>();
    private List<Vector2> _bloonPath;
    [SerializeField] private Queue<GameObject> _bloonPool = new Queue<GameObject>();
    private void OnEnable()
    {
        BloonMovement._endOfPath += ReturnObjectToPool;
    }
    private void OnDisable()
    {
        BloonMovement._endOfPath -= ReturnObjectToPool;
    }
    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        LoadBloonScirpts();
        LoadSprites();
        _bloonPath = _gOPath.GetComponent<BloonPathCreator>().GetPathVectors();
        StartCoroutine(SpawnBloons());
    }
    private void LoadBloonScirpts()
    {
        string lScriptName;
        foreach (var script in _bloonScripts)
        {
            lScriptName = script.name;
            _bloonScriptDictionary[lScriptName] = script;
        }
    }
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
    /// Gets a bloon from a queue of already instantiated bloons, if there isn't one available it instansiates a new one.
    /// </summary>
    /// <returns>A Bloon prefab</returns>
    public GameObject GetBloon()
    {
        if(_bloonPool.Count > 0)
        {
            GameObject lBloon = _bloonPool.Dequeue();
            lBloon.SetActive(true);
            return lBloon;
        }
        else
        {
            return Instantiate(_bloon);
        }
    }
    /// <summary>
    /// Returns a bloon to the pool and deactivates it.
    /// </summary>
    /// <param name="aBloon"></param>
    public void ReturnObjectToPool(GameObject aBloon)
    {
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
    private IEnumerator SpawnBloons()
    {
        GameObject lBloon;
        for(int i = 0; i < 100;  i++)
        {
            lBloon = GetBloon();
            //lBloon = Instantiate(_bloon, _bloonPath[0], Quaternion.identity);
            lBloon.transform.position = _bloonPath[0];
            lBloon.transform.rotation = Quaternion.identity;
            lBloon.GetComponent<BloonMovement>().SetPath(_bloonPath);
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
}
