using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloon : BaseBloon
{
    private Sprite redBloonSprite;
    void Start()
    {
        IntializeBloon();
        bloonSprite = GetComponent<SpriteRenderer>().sprite;
    }
    public override void IntializeBloon()
    {
        speed = 1.0f;
        health = 1;
        cash = 1;
        childCount = 0;
        child = null;
        SetSprite();
    }
    public override void SetSprite()
    {
        bloonSprite = redBloonSprite;
    }
    private void FindSprite()
    {
        //redBloonSprite = Resources.Load("Bloon Types");
    }
}
