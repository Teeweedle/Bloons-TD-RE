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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(BLOON))
            baseTower.TargetAquired(collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(BLOON))
            baseTower.TargetLost(collider);
    }
}
