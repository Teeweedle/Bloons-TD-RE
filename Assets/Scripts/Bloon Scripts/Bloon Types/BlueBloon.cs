using UnityEngine;

public class BlueBloon : BaseBloon
{
    private Sprite blueBloonSprite;
    private const string BLUEBLOON = "Blue Bloon";
    private const string REDBLOON = "Red Bloon";

    private void Start()
    {
        InitializeBloon();
    }
    public override void InitializeBloon()
    {
        speed = 1.4f;
        health = 1;
        cash = 2;
        childCount = 1;
        child = REDBLOON;
        SetSprite();
    }
    public override void SetSprite()
    {
        FindSprite();
        bloonSprite = blueBloonSprite;
    }
    private void FindSprite()
    {
        blueBloonSprite = BloonSpawner._instance.GetSpriteByName(BLUEBLOON);
    }
}
