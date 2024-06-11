using System.IO;
using Unity.Jobs;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _towerSpriteRenderer;
    [SerializeField] private SpriteRenderer _rangeSpriteRenderer;
    [SerializeField] private GameObject _towerRange;
    [SerializeField] private Collider2D _rangeCollider;

    private const string _NOBUILDTAG = "No Build";
    private const string _TOWERTAG = "Tower";

    private bool _canPlace = true;

    private void Start()
    {
        DisplayTowerRange();
    }
    void Update()
    {        
        FollowMousePosition();

        if (_canPlace && Input.GetMouseButtonDown(0))
        {
            TowerSelected._towerInstance = null;
            _rangeSpriteRenderer.enabled = false;
            _rangeCollider.enabled = true;
            Destroy(this);
        } 
    }
    private void FollowMousePosition()
    {
        transform.position = Camera.main.ScreenToWorldPoint(GetMousePosition());
    }

    private Vector3 GetMousePosition()
    {
        Vector3 lMousePosition = Input.mousePosition;
        lMousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        return lMousePosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case _NOBUILDTAG:
            case _TOWERTAG:
                _canPlace = false;
                _towerSpriteRenderer.color = Color.red;
                break;
            default: break;
        }        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case _NOBUILDTAG:
            case _TOWERTAG:
                _canPlace = true;
                _towerSpriteRenderer.color = Color.white;
                break; 
            default: break;
        }
    }
    private void DisplayTowerRange()
    {
        _towerRange.transform.localScale *= TowerSelected._towerInstance.GetComponent<BaseTower>().GetTowerStats().range;
    }
}
