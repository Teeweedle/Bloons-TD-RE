using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BloonMovement : MonoBehaviour
{
    [SerializeField] private List<Vector2> _path;
    private float _speed = 5f;//TODO: Change to get speed at run time based on the type of bloon
    private int _currentPosition;
    
    private void Start()
    {
        _currentPosition = 0;
    }
    void Update()
    {
        if (_currentPosition < _path.Count - 1)
        {
            Move(_path[_currentPosition + 1], _speed);
            if (Vector2.Distance(this.transform.position, _path[_currentPosition + 1]) < 0.006f)
            {
                GetNextTarget();
            }
        }
    }
    private void GetNextTarget()
    {
        _currentPosition++;
    }

    private void Move(Vector2 aTarget, float aSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, aTarget, Time.deltaTime * aSpeed);
    }
    public void SetPath(List<Vector2> aPath)
    {
        _path = aPath;
    }
    public void SetSpeed(float aSpeedModifier)
    {
        _speed *= aSpeedModifier;
    }
}
