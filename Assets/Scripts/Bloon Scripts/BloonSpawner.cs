using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BloonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gOPath;
    [SerializeField] private GameObject _bloon;
    [SerializeField] private BloonFactory _factory;
    [SerializeField] private BloonWaveManager _waveManager;

    // TEST INFO
    public int _setRoundNumber;
    public TextMeshProUGUI _time;
    private bool _roundStarted = false;
    // TEST INFO

    public static BloonSpawner _instance;
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
        _bloonPath = _gOPath.GetComponent<BloonPathCreator>().GetPathVectors();
    }
    /// <summary>
    /// Gets a bloon from a queue of already instantiated bloons, if there isn't one available it instantiates a new one.
    /// Attaches the passed behavior script to the game object from BloonFactory.
    /// </summary>
    /// <param name="aBloonType">Bloon type</param>
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
    /// <summary>
    /// Starts the wave which is delayed by the start time of the wave and is completed by the end time of the wave
    /// </summary>
    /// <param name="aRoundNumber">Current round</param>
    /// <returns></returns>
    private IEnumerator StartWave(int aRoundNumber)
    {
        if(_waveManager.GetWaveData(aRoundNumber) != null)
        {
            float lStartTime = Time.time;
            foreach (var wave in _waveManager.GetWaveData(aRoundNumber))
            {
                float lWaveStartTime = lStartTime + wave.startTime;
                float lDelay = lWaveStartTime - Time.time;

                if (lDelay > 0)
                    yield return new WaitForSeconds(lDelay);

                float lSpawnInterval = (wave.endTime - wave.startTime) / wave.count;
                StartCoroutine(SpawnBloons(wave.count, lSpawnInterval, wave.bloonType)); 
            }
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
            InitializeBloon(lBloon, _bloonPath[0], Quaternion.identity, 0f, 0);
            yield return new WaitForSeconds(aSpawnDelay);
        }
    }
    private void SpawnChildrenHandler(int aChildCount, float aDistance, int aPathPosition, Vector3 aPosition, string aBloonType,
        int aProjectileID, int aLeftOverDmg, BaseTower aParentTower)
    {
        SpawnChildren(aChildCount, aDistance, aPathPosition, aPosition, aBloonType, aProjectileID, aLeftOverDmg, aParentTower);
    }
    /// <summary>
    /// Spawns children bloons
    /// </summary>
    /// <param name="aChildCount">How many children</param>
    /// <param name="aDistance">How far long the path</param>
    /// <param name="aPathPosition">What position on that path is the bloon</param>
    /// <param name="aPosition">Game world position</param>
    /// <returns></returns>
    public void SpawnChildren(int aChildCount, float aDistance, int aPathPosition, Vector3 aPosition, string aBloonType, 
        int aProjectileID, int aLeftOverDmg, BaseTower aParentTower)
    {
        GameObject lBloon;
        for (int i = 0; i < aChildCount; i++)
        {
            lBloon = GetBloon(aBloonType);

            InitializeBloon(lBloon, aPosition, Quaternion.identity, aDistance, aPathPosition);
            BaseBloon lChildBloon = lBloon.GetComponent<BaseBloon>();
            lChildBloon.GrantImmunity(aProjectileID, _immunityDuration);
            
            if(aLeftOverDmg > 0)
            {
                //carries over left over damage to the next child
                aLeftOverDmg = ApplyLeftOverDamage(aLeftOverDmg, lChildBloon, aParentTower);
            }
        }
    }
    /// <summary>
    /// Applies left over damage to the next child. Calculates how much damage was left over and returns the remaining damage 
    /// that should be dealt to the next child
    /// </summary>
    /// <param name="aLeftOverDmg">How much damage was left over</param>
    /// <param name="aChildBloon">Target bloon</param>
    /// <param name="aParentTower">Tower that is doing the dmg</param>
    /// <returns>Remaining damage</returns>
    private int ApplyLeftOverDamage(int aLeftOverDmg, BaseBloon aChildBloon, BaseTower aParentTower)
    {
        int lInitialBloonHP = aChildBloon.GetHealth();
        //Need to get reference to the tower that this dmg comes from
        //pass 0 to ignore immunity for left over dmg
        aChildBloon.TakeDamage(aLeftOverDmg, 0, aParentTower);
        int lDmgApplied = lInitialBloonHP - aChildBloon.GetHealth();
        return aLeftOverDmg - lDmgApplied;
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

        BloonMovement lBloonMovement = aBloon.GetComponent<BloonMovement>();
        lBloonMovement.SetPath(_bloonPath);
        lBloonMovement.SetPathPosition(aPathPosition);

        BaseBloon lBaseBloon = aBloon.GetComponent<BaseBloon>();
        lBaseBloon.SetDistance(aDistance);
    }
    public void StartRound()
    {
        //TODO: Toggle button to increase game speed while round is active (not just bloons).
        //TODO: Change to get info passed from round info       
        StartCoroutine(StartWave(_setRoundNumber));
        _roundStarted = true;
    }
}
