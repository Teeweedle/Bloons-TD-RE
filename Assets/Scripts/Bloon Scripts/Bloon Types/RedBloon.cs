using UnityEngine;

public class RedBloon : BaseBloon
{
    private Sprite redBloonSprite;
    private const string REDBLOON = "Red Bloon";
    void Start()
    {
        InitializeBloon();
    }
    public override void InitializeBloon()
    {
        speed = 1.0f;
        health = 1;
        cash = 1;
        childCount = 0;
        childType = null;
        SetSprite();
    }
    public override void SetSprite()
    {
        FindSprite();
        bloonSpriteRender.sprite = redBloonSprite;
    }
    private void FindSprite()
    {
        redBloonSprite = BloonSpawner._instance.GetSpriteByName(REDBLOON);
    }
}
