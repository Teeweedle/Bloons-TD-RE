using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextAsset roundInfoJSONFile;
    [SerializeField] private RoundsDictionary roundsData;
    // Start is called before the first frame update
    void Start()
    {
        if(roundInfoJSONFile != null)
        {
            string jsonString = roundInfoJSONFile.text;
            roundsData = JsonConvert.DeserializeObject<RoundsDictionary>(jsonString);
            CheckRound(3);
        }else
        {
            Debug.LogError("No JSON file found");
        }
    }

    private void CheckRound(int aRoundNumber)
    {
        if (roundsData.rounds.ContainsKey(aRoundNumber))  // Check if round 3 exists
        {
            List<BloonWave> wavesInRound3 = roundsData.rounds[aRoundNumber];
            foreach (var wave in wavesInRound3)
            {
                Debug.Log($"Bloon Type: {wave.bloonType}, Count: {wave.count}");
                Debug.Log($"Start Time: {wave.startTime}, End Time: {wave.endTime}");
            }
        }
    }
}
