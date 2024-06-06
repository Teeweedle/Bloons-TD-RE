using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonFactory : MonoBehaviour
{
    private Dictionary<string, Type> bloonScriptDictionary;
    private void Awake()
    {
        bloonScriptDictionary = new Dictionary<string, Type>
        {
            { "Red Bloon", typeof(RedBloon) },
            { "Blue Bloon", typeof(BlueBloon) },
            { "Green Bloon", typeof(GreenBloon) },
            { "Yellow Bloon", typeof(YellowBloon) },
            { "Pink Bloon", typeof(PinkBloon) },
            { "Black Bloon", typeof(BlackBloon) },
            { "White Bloon", typeof(WhiteBloon) },
            { "Purple Bloon", typeof(PurpleBloon) }
            //TODO: Add more bloons
        };
    }
    /// <summary>
    /// Checks a dictionary for the behavior script, if found it attaches it to the game object.
    /// </summary>
    /// <param name="aBloon">Target game object</param>
    /// <param name="aBloonTypeName">Type of behavior</param>
    public void AssignBloonScirpt(GameObject aBloon, string aBloonTypeName)
    {
        if(bloonScriptDictionary.TryGetValue(aBloonTypeName, out Type lBloonType))
        {
            aBloon.AddComponent(lBloonType);
        }
        else
        {
            Debug.LogError($"Bloon type {aBloonTypeName} not found.");
        }
    }
    /// <summary>
    /// Removes BaseBloon script from game object if there is one.
    /// </summary>
    /// <param name="aBloon">A game object</param>
    public void RemoveBloonScript(GameObject aBloon)
    {
        BaseBloon lCurrentBloonScript = aBloon.GetComponent<BaseBloon>();
        if(lCurrentBloonScript != null)
        {
            Destroy(lCurrentBloonScript);
        }
    }
}
