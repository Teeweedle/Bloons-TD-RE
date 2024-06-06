using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBloon : BaseBloon
{
    private Sprite blackBloonSprite;
    private const string BLACKBLOON = "Black Bloon";
    private const string PINKBLOON = "Pink Bloon";

    private void Start()
    {
        InitializeBloon();
    }
    public override void InitializeBloon()
    {
        mySpeed = 1.8f;
        health = 1;
        cash = 1;
        xp = 1;
        childCount = 2;
        childType = PINKBLOON;
        SetSprite();
        //TODO: Add immunity to explosions
    }
    public override void SetSprite()
    {
        FindSprite();
        bloonSpriteRender.sprite = blackBloonSprite;
    }
    private void FindSprite()
    {
        blackBloonSprite = BloonFactory.Instance.GetSpriteByName(BLACKBLOON);
    }
}
