using System.Collections.Generic;
using UnityEngine;

public class BloonMovement : MonoBehaviour
{
    [SerializeField] private List<Vector2> _path;
    private float _speed = 1.5f;//TODO: Change to get speed at run time based on the type of bloon
    private int _currentPathPosition;

    public delegate void BloonMovementDelegate(GameObject aGameObject);
    public static event BloonMovementDelegate _endOfPath;
    private void Start()
    {
        _currentPathPosition = 0;
    }
    void Update()
    {
        if (_currentPathPosition < _path.Count - 1)
        {
            Move(_path[_currentPathPosition + 1], _speed);
            if (Vector2.Distance(this.transform.position, _path[_currentPathPosition + 1]) < 0.006f)
            {
                GetNextTarget();
            }
        }else
        {
            _endOfPath?.Invoke(gameObject);//tells BloonSpawner to deactive this bloon and add it to the pool
        }
    }
    private void GetNextTarget()
    {
        _currentPathPosition++;
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
    public void SetPathPosition(int aCurrentPosition)
    {
        _currentPathPosition = aCurrentPosition;
    }
    public int GetPathPostion()
    {
        return _currentPathPosition;
    }
}
