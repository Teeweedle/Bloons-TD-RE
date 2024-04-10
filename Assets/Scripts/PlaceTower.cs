using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {        
        FollowMousePosition();
        
        if(Input.GetMouseButtonDown(0))
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(GetMousePosition());
            Destroy(GetComponent <PlaceTower>());
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
