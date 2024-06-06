using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBloon : BaseBloon
{
    private Sprite pinkBloonSprite;
    private const string PINKBLOON = "Pink Bloon";
    private const string YELLOWBLOON = "Yellow Bloon";

    private void Start()
    {
        InitializeBloon();
    }
    public override void InitializeBloon()
    {
        mySpeed = 3.5f;
        health = 1;
        cash = 1;
        xp = 1;
        childCount = 1;
        childType = YELLOWBLOON;
        SetSprite();
    }
    public override void SetSprite()
    {
        FindSprite();
        bloonSpriteRender.sprite = pinkBloonSprite;
    }
    private void FindSprite()
    {
        pinkBloonSprite = BloonSpawner._instance.GetSpriteByName(PINKBLOON);
    }
}
