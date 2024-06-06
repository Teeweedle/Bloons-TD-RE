using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BloonFactory : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    public static BloonFactory Instance { get; private set; }

    private Dictionary<string, Sprite> spritesDictionary;
    private Dictionary<string, Type> bloonScriptDictionary;
    private void Awake()
    {
        Instance = this;
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
    private void Start()
    {
        LoadSprites();
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
    /// <summary>
    /// Loads all sprites into a dictionary
    /// </summary>
    private void LoadSprites()
    {
        spritesDictionary = new Dictionary<string, Sprite>();
        foreach (var sprite in sprites)
        {
            spritesDictionary[sprite.name] = sprite;
        }
    }
    /// <summary>
    /// Gets a sprite from a dictionary
    /// </summary>
    /// <param name="aSpriteName"></param>
    /// <returns></returns>
    public Sprite GetSpriteByName(string aSpriteName)
    {
        if (spritesDictionary.TryGetValue(aSpriteName, out var sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogError($"Sprite not found {aSpriteName}");
            return null;
        }
    }
}
