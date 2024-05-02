using UnityEngine;
using UnityEngine.EventSystems;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private Material _outlineShader, _defaultShader;
    [SerializeField] private SpriteRenderer _towerSpriteRenderer;
    
    public string _type, _targetType;
    public int _dmg, _pierce, _range, _xp;
    public float _attackSpeed;
    public bool _hasCamoDetection;

    private bool _isPlaced, _isSelected;
    private void Start()
    {
        _isPlaced = false;
        _isSelected = false;
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.back);
        //    if (hit.transform != null)
        //    {
        //        Debug.Log("Hit object: " + hit.collider.gameObject.name);
        //        HighLight();
        //    }
        //    else
        //    {
        //        // Log if no object is hit
        //        Debug.Log("No object hit.");
        //        UnHighLight();
        //    }
        //}
    }
    private void Fire(GameObject aProjectile, int aAmount)
    {
        for (int i = 0; i < aAmount; i++)
        {
            Instantiate(aProjectile, this.transform.position, Quaternion.identity);
        }
    }
    private void TargetBloon(string aTargetType)
    {

    }

    public void HighLight()
    {
        _towerSpriteRenderer.material = _outlineShader;
        _isSelected = true;
    }
    public void UnHighLight()
    {
        _towerSpriteRenderer.material = _defaultShader;
        _isSelected = false;
    }
    public void AssignStats(TowerDataObject aData)
    {
        _type = aData.type;
        _attackSpeed = aData.attackSpeed;
        _dmg = aData.damage;
        _pierce = aData.pierce;
        _range = aData.range;
        _hasCamoDetection = aData.hasCamoDetection;
    }

    public void OnMouseDown()
    {
        if(_isPlaced)
        {
            HighLight();
        }
        _isPlaced = true;
    }
}
