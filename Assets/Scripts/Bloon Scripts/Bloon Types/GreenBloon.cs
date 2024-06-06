using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBloon : BaseBloon
{
    private Sprite greenBloonSprite;
    private const string GREENBLOON = "Green Bloon";
    private const string BLUEBLOON = "Blue Bloon";

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
        childCount = 1;
        childType = BLUEBLOON;
        SetSprite();
    }
    public override void SetSprite()
    {
        FindSprite();
        bloonSpriteRender.sprite = greenBloonSprite;
    }
    private void FindSprite()
    {
        greenBloonSprite = BloonFactory.Instance.GetSpriteByName(GREENBLOON);
    }
}
