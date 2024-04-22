using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class NoBuildZone : MonoBehaviour
{
    [SerializeField] GameObject _bloonPathGO;
    [SerializeField] GameObject _noBuildPrefab;
    private List<Vector2> _bloonPathList;
    void Start()
    {
        _bloonPathList = _bloonPathGO.GetComponent<BloonPathCreator>().GetPathVectors();
       
        for(int i = 0; i < _bloonPathList.Count - 1; i++)
        {
            CreateNoBuildZone(_bloonPathList[i], _bloonPathList[i+1]);
        }
    }
    /// <summary>
    /// Creates a squre at a specified point. Then scales it the distance between two points and rotates it 
    /// to be perpendicular to those points.
    /// </summary>
    /// <param name="aPointA"></param>
    /// <param name="aPointB"></param>
    private void CreateNoBuildZone(Vector2 aPointA, Vector2 aPointB)
    {
        float lDistance, lAngle;
        Vector3 lCenter;

        lDistance = Vector3.Distance(aPointA, aPointB);

        lCenter = (aPointA + aPointB) / 2;

        lAngle = Mathf.Atan2(aPointB.y - aPointA.y,
            aPointB.x - aPointA.x) * Mathf.Rad2Deg;

        GameObject ltempNoBuild = Instantiate(_noBuildPrefab, this.transform);
        ltempNoBuild.transform.position = lCenter;
        ltempNoBuild.transform.localScale = new Vector3(lDistance, 0.15f, 1f);
        ltempNoBuild.transform.rotation = Quaternion.Euler(0f, 0f, lAngle);
    }
}
