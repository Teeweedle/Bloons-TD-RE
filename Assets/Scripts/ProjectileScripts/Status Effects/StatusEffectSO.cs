using UnityEngine;

[CreateAssetMenu(fileName = "NewStatus", menuName = "Status Effect")]
public class StatusEffectSO : ScriptableObject
{
    public string type;
    public int damage;
    public float duration;
    public float amount;
    public float tickRate;
}
