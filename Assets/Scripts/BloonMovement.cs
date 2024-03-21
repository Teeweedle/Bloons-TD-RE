using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> _path;//TODO: Update to get path dynamically

    private float _speed = 5f;
    private float _duration, _startTime, _fraction;
    private int _currentPosition;
    
    private void Start()
    {
        _currentPosition = 0;
        _duration = GetDuration(_path[_currentPosition], _path[_currentPosition + 1]);
        _startTime = Time.time;
        this.transform.position = _path[_currentPosition].transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (_currentPosition < _path.Count - 1)
        {
            //TODO: Update to move @ constant speeds
            Move(_path[_currentPosition], _path[_currentPosition + 1]);
            if (Vector2.Distance(this.transform.position, _path[_currentPosition + 1].transform.position) < 0.006f)
            {
                GetNextTarget();
            }

        }
    }
    private void GetNextTarget()
    {
        _startTime = Time.time;
        _currentPosition++;
        Debug.Log(_currentPosition);
    }

    private void Move(GameObject aCurrent, GameObject aTarget)
    {
        _fraction = (Time.time - _startTime) / _duration;
        _fraction = Mathf.Clamp01(_fraction);
        this.transform.position = Vector2.Lerp(aCurrent.transform.position, aTarget.transform.position, _fraction);
    }
    private float GetDuration(GameObject aCurrent, GameObject aTarget)
    {
        float lDistance = Vector2.Distance(aCurrent.transform.position, aTarget.transform.position);
        return lDistance / _speed;
    }
}
