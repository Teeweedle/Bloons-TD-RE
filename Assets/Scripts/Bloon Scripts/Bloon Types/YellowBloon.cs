using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBloon : BaseBloon
{
    private Sprite yellowBloonSprite;
    private const string YELLOWBLOON = "Yellow Bloon";
    private const string GREENBLOON = "Green Bloon";

    private void Start()
    {
        InitializeBloon();
    }
    public override void InitializeBloon()
    {
        mySpeed = 3.2f;
        health = 1;
        cash = 1;
        xp = 1;
        childCount = 1;
        childType = GREENBLOON;
        SetSprite();
    }
    public override void SetSprite()
    {
        FindSprite();
        bloonSpriteRender.sprite = yellowBloonSprite;
    }
    private void FindSprite()
    {
        yellowBloonSprite = BloonFactory.Instance.GetSpriteByName(YELLOWBLOON);
    }
}
