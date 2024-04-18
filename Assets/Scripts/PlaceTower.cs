using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    private bool _canPlace = true;
    void Update()
    {        
        FollowMousePosition();

        if (_canPlace)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.gameObject.AddComponent<BaseTower>();
                Destroy(this);
            } 
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
        _canPlace = false;
        _spriteRenderer.color = Color.red;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _canPlace = true;
        _spriteRenderer.color = Color.white;
    }
}
