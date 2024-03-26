using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BloonPathCreator : MonoBehaviour
{
    [SerializeField] private GameObject _bloonPath;
    [SerializeField] private GameObject _waypointPrefab;
    [SerializeField] private List<GameObject> _pathList;

    public void CreateNewPoint()
    {
        GameObject lTempObj = Instantiate(_waypointPrefab, _bloonPath.transform);
        lTempObj.name = ("Waypoint " + (_pathList.Count + 1).ToString());
        EditorGUIUtility.SetIconForObject(_waypointPrefab, (Texture2D) EditorGUIUtility
                .IconContent("Packages/com.unity.timeline/Editor/StyleSheets/Images/DarkSkin/TimelineAutokey.png").image);
        _pathList.Add(lTempObj);
    }    
    public void CreateNewPoint(GameObject aGameObject, Vector2 aPostion)
    {
        GameObject lTempObj = Instantiate(_waypointPrefab, aGameObject.transform);
        lTempObj.name = (aGameObject.name + " " + "anchor");
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
                if (_pathList[i].transform.childCount > 0)//draw lines to anchor
                {
                    for (int j = _pathList[i].transform.childCount - 1; j >= 0; j--)
                        DrawLine(_pathList[i].transform, _pathList[i].transform.GetChild(j));
                }
                if (_pathList[i].GetComponent<Waypoint>()._isBezier && _pathList[i + 1].GetComponent<Waypoint>()._isBezier)
                {
                    //draw curve
                    DrawBezierCurve(_pathList[i].transform, _pathList[i + 1].transform);
                }
                else
                {
                    //draw line
                    DrawLine(_pathList[i].transform, _pathList[i + 1].transform);
                }
            }
        }
    }
    private void DrawBezierCurve(Transform aWaypointA, Transform aWaypointB)
    {
        Vector2 lGizmoPosition;
        for(float i = 0; i <= 1; i += 0.1f)
        {
            lGizmoPosition = CalculateBezierPoint(i, aWaypointA, aWaypointB);
            Gizmos.DrawSphere(lGizmoPosition, 0.05f);
        }
    }
    private void DrawLine(Transform aWaypointA, Transform aWaypointB)
    {
        if (aWaypointB != null)
            Gizmos.DrawLine(aWaypointA.position, aWaypointB.position);
    }

    private Vector2 CalculateBezierPoint(float aStep, Transform aWaypointA, Transform aWaypointB)
    {
        Vector2 lGizmoPosition = Mathf.Pow(1 - aStep, 3) * aWaypointA.position + 
            3 * Mathf.Pow(1 - aStep, 2) * aStep * aWaypointA.GetChild(0).transform.position + 
            3 * (1 - aStep) * Mathf.Pow(aStep, 2) * aWaypointB.GetChild(1).transform.position + 
            Mathf.Pow(aStep, 3) * aWaypointB.position;
        return lGizmoPosition;
    }
}
