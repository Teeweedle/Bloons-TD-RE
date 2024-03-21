using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BloonPathCreator : MonoBehaviour
{
    [SerializeField] private GameObject _bloonPath;
    [SerializeField] private GameObject _waypointPrefab;
    [SerializeField] private List<GameObject> _pathList;

    private Vector2 _gizmoPosition;

    public void CreateNewPoint()
    {
        GameObject lTempObj = Instantiate(_waypointPrefab, _bloonPath.transform);
        lTempObj.name = ("Waypoint " + (_pathList.Count + 1).ToString());
        EditorGUIUtility.SetIconForObject(_waypointPrefab, (Texture2D) EditorGUIUtility
                .IconContent("Packages/com.unity.timeline/Editor/StyleSheets/Images/DarkSkin/TimelineAutokey.png").image);
        _pathList.Add(lTempObj);
    }    
    public void CreateNewPoint(Vector2 aPostion)
    {
        GameObject lTempObj = Instantiate(_waypointPrefab, _bloonPath.transform);
        lTempObj.name = ("Anchor " + (_pathList.Count + 1).ToString());
        lTempObj.transform.position = aPostion;
        EditorGUIUtility.SetIconForObject(_waypointPrefab, (Texture2D)EditorGUIUtility
                .IconContent("Packages/com.unity.timeline/Editor/StyleSheets/Images/DarkSkin/TimelineAutokey.png").image);
    }
    public void DeletePath()
    {
        _pathList = new List<GameObject>();
        for(int i = _bloonPath.transform.childCount - 1; i >= 0; i--)
        {
            GameObject lChild = _bloonPath.transform.GetChild(i).gameObject;
            DestroyImmediate(lChild);
        }
    }

    private void OnDrawGizmos()
    {
        if(_pathList.Count > 1)
        {
            for (int i = 0; i < _pathList.Count - 1; i++)
            {
                if (!_pathList[i].GetComponent<Waypoint>()._isBezier)
                {
                    if (_pathList[i + 1] != null)
                        Gizmos.DrawLine(_pathList[i].transform.position, _pathList[i + 1].transform.position);
                }
                else//if point is turned into a bezier curve
                {                    
                    _gizmoPosition = Mathf.Pow(1 - i, 3) * _pathList[i].transform.position +
                    3 * Mathf.Pow(1 - i, 2) * i * _pathList[i+1].transform.position;
                }
            }

        }



        //Gizmos.DrawLine(_pathList[0].transform.position, _pathList[1].transform.position);
        //for(float i = 0; i<= 1; i += 0.05f)
        //{
        //    _gizmoPosition = Mathf.Pow(1 - i, 3) * _pathList[0].transform.position +
        //        3 * Mathf.Pow(1 - i, 2) * i * _pathList[1].transform.position;
        //}
    }
}
