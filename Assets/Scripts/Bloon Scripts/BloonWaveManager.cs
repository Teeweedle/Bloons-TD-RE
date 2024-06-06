using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonWaveManager : MonoBehaviour
{
    [SerializeField] private TextAsset roundInfoJSONFile;
    [SerializeField] private RoundsDictionary roundsData;
    // Start is called before the first frame update
    void Start()
    {
        LoadRoundData();
    }
    /// <summary>
    /// Returns a list of bloon waves in the specified round
    /// </summary>
    /// <param name="aRoundNumber"></param>
    /// <returns></returns>
    public List<BloonWave> GetWaveData(int aRoundNumber)
    {
        if (roundsData.rounds.ContainsKey(aRoundNumber))  // Check if round 3 exists
        {
            return roundsData.rounds[aRoundNumber];
        }
        else
        {
            Debug.LogError($"Round {aRoundNumber} does not exist");
            return null;
        }
    }
    /// <summary>
    /// Loads the round data from a JSON file
    /// </summary>
    private void LoadRoundData()
    {
        if (roundInfoJSONFile != null)
        {
            string jsonString = roundInfoJSONFile.text;
            roundsData = JsonConvert.DeserializeObject<RoundsDictionary>(jsonString);
        }
        else
        {
            Debug.LogError("No JSON file found");
        }
    }
}
