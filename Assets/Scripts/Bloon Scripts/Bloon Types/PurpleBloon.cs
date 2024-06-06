using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBloon : BaseBloon
{
    private Sprite purpleBloonSprite;
    private const string PURPLEBLOON = "Purple Bloon";
    private const string PINKBLOON = "Pink Bloon";

    private void Start()
    {
        InitializeBloon();
    }
    public override void InitializeBloon()
    {
        mySpeed = 3.0f;
        health = 1;
        cash = 1;
        xp = 1;
        childCount = 2;
        childType = PINKBLOON;
        SetSprite();
        //TODO: Add immunity to energy, fire and plasma
    }
    public override void SetSprite()
    {
        FindSprite();
        bloonSpriteRender.sprite = purpleBloonSprite;
    }
    private void FindSprite()
    {
        purpleBloonSprite = BloonSpawner._instance.GetSpriteByName(PURPLEBLOON);
    }
}
