using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSelected : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _selectedGameObject;

    private static TowerSelected _instance;
    public static GameObject _towerInstance;
    private void Start()
    {
        DeselectImage();
    }
    /// <summary>
    /// Hide item selected highlight image
    /// </summary>
    private void DeselectImage()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    /// <summary>
    /// Hide item selected highlight overlay
    /// </summary>
    /// <param name="aHighlight"></param>
    private void SelectTower(GameObject aHighlight)
    {
        aHighlight.SetActive(true);
        _instance = this;
        if(_towerInstance != null)
            Destroy(_towerInstance);

        _towerInstance = Instantiate(_selectedGameObject, Input.mousePosition, Quaternion.identity);
    }
    /// <summary>
    /// Checks if an image is selected, if so deselect
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(_instance != null)
            _instance.DeselectImage();

        SelectTower(this.transform.GetChild(0).gameObject);       
    }
}
