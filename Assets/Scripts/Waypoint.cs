using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [field: SerializeField] public bool _isBezier { get; set; }
    [field: SerializeField] public Vector2 _myPostion { get; private set; }

    private BloonPathCreator _creator;

    [ContextMenu("Toggle Bezier Curve")]
    private void ToggleBezierCurve()
    {
        _isBezier = !_isBezier;
        if (_isBezier)
        {
            _creator = GameObject.FindAnyObjectByType<BloonPathCreator>();
            for(int i = -1 ; i < 1; i++) {
                _creator.CreateNewPoint(this.gameObject, new Vector2(this.transform.position.x + i, this.transform.position.y + 0.5f));
            }
        }
        else
        {
            for(int i = this.gameObject.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(this.gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}
