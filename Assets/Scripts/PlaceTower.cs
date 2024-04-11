using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    void Update()
    {        
        FollowMousePosition();
        
        if(Input.GetMouseButtonDown(0))
        {
            this.gameObject.AddComponent<BaseTower>();
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
}
