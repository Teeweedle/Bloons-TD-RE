using UnityEngine;

public class BloonInRange : MonoBehaviour
{
    [SerializeField] private BaseTower baseTower;
    private const string BLOON = "Bloon";
    // Start is called before the first frame update
    void Start()
    {
        baseTower = GetComponentInParent<BaseTower>();
    }
    /// <summary>
    /// Sends BaseTower the gameObject that recently entered the towers range.
    /// </summary>
    /// <param name="collider">New bloon that entered the towers range.</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(BLOON))
            baseTower.TargetAquired(collider);
    }
    /// <summary>
    /// Tells tower a target moved out of range.
    /// </summary>
    /// <param name="collider">Target that moved out of range.</param>
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(BLOON))
            baseTower.TargetLost(collider);
    }
}
