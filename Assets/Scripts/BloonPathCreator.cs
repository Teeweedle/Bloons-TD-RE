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
    [SerializeField] private List<Vector2> _pathVectors;

    private void Awake()
    {
        _pathVectors = GeneratePathVectors();
    }

    #region EDITOR GIZMOS
    public void CreateNewPoint()
    {
        GameObject lTempObj = Instantiate(_waypointPrefab, _bloonPath.transform);
        lTempObj.name = ("Waypoint " + (_pathList.Count + 1).ToString());
        EditorGUIUtility.SetIconForObject(_waypointPrefab, (Texture2D)EditorGUIUtility
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
        for (int i = _bloonPath.transform.childCount - 1; i >= 0; i--)
        {
            GameObject lChild = _bloonPath.transform.GetChild(i).gameObject;
            DestroyImmediate(lChild);
        }
    }
    private void OnDrawGizmos()
    {
        if (_pathList.Count > 1)
        {
            for (int i = 0; i < _pathList.Count - 1; i++)
            {
                if (_pathList[i].transform.childCount > 0)
                {
                    ConnectAnchors(_pathList[i].transform);
                }
                if (_pathList[i].GetComponent<Waypoint>()._isBezier && _pathList[i + 1].GetComponent<Waypoint>()._isBezier)
                {
                    DrawBezierCurve(_pathList[i].transform, _pathList[i + 1].transform);
                }
                else
                {
                    DrawLine(_pathList[i].transform, _pathList[i + 1].transform);
                }
            }
            //checks the last item in a list for children(anchors)
            if (_pathList[^1].transform.childCount > 0)
                ConnectAnchors(_pathList[^1].transform);
        }
    }
    private void DrawBezierCurve(Transform aStartPoint, Transform aEndPoint)
    {
        Vector2 lGizmoPosition;
        for (float i = 0.2f; i < 1; i += 0.2f)
        {
            lGizmoPosition = CalculateBezierPoint(i, aStartPoint, aEndPoint);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lGizmoPosition, 0.05f);
        }
    }
    private void DrawLine(Transform aWaypointA, Transform aWaypointB)
    {
        Gizmos.color = Color.white;
        if (aWaypointB != null)
            Gizmos.DrawLine(aWaypointA.position, aWaypointB.position);
    } 
    private void ConnectAnchors(Transform aParent)
    {
        for (int i = aParent.childCount - 1; i >= 0; i--)
            DrawLine(aParent, aParent.GetChild(i).transform);
    }
    #endregion

    private List<Vector2> GeneratePathVectors()
    {
        List<Vector2> lPath = new();
        for(int i = 0;  i < _pathList.Count; i++)
        {
            lPath.Add(_pathList[i].transform.position);
            if(_pathList[i].GetComponent<Waypoint>()._isBezier && _pathList[i + 1].GetComponent<Waypoint>()._isBezier)
            {
                lPath = AddBezierVectors(lPath, _pathList[i].transform, _pathList[i + 1].transform);
            }
        }
        return lPath;
    }
    private List<Vector2> AddBezierVectors(List<Vector2> aPath, Transform aStartPoint, Transform aEndPoint)
    {
        List<Vector2> lBezierPath = aPath;
        for (float i = 0.2f; i < 1; i += 0.2f)
        {
            lBezierPath.Add(CalculateBezierPoint(i, aStartPoint, aEndPoint));
        }
        return lBezierPath;
    }
    private Vector2 CalculateBezierPoint(float aStep, Transform aWaypointA, Transform aWaypointB)
    {
        Vector2 lGizmoPosition = Mathf.Pow(1 - aStep, 3) * aWaypointA.position + 
            3 * Mathf.Pow(1 - aStep, 2) * aStep * aWaypointA.GetChild(0).transform.position + 
            3 * (1 - aStep) * Mathf.Pow(aStep, 2) * aWaypointB.GetChild(1).transform.position + 
            Mathf.Pow(aStep, 3) * aWaypointB.position;
        return lGizmoPosition;
    }
    public List<Vector2> GetPathVectors()
    {
        return _pathVectors;
    }
}
