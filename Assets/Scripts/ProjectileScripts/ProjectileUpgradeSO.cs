using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileUpgrade", menuName = "NewProjectileUpgrade")]
public class ProjectileUpgradeSO : ScriptableObject
{
    public string collisionType;
    public int damage;
    public int pierce;
    public float speed;
    public float lifeSpan;
    public StatusEffectSO statusEffect;
    public Sprite sprite;
}
