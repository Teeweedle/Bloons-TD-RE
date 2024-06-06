using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBloon : BaseBloon
{
    private Sprite whiteBloonSprite;
    private const string WHITEBLOON = "White Bloon";
    private const string PINKBLOON = "Pink Bloon";

    private void Start()
    {
        InitializeBloon();
    }
    public override void InitializeBloon()
    {
        mySpeed = 2.0f;
        health = 1;
        cash = 1;
        xp = 1;
        childCount = 2;
        childType = PINKBLOON;
        SetSprite();
        //TODO: Add immunity to freezing
    }
    public override void SetSprite()
    {
        FindSprite();
        bloonSpriteRender.sprite = whiteBloonSprite;
    }
    private void FindSprite()
    {
        whiteBloonSprite = BloonSpawner._instance.GetSpriteByName(WHITEBLOON);
    }
}
