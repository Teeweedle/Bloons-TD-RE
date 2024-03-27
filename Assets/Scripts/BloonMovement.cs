using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonMovement : MonoBehaviour
{
    [SerializeField] private List<Vector2> _path;
    private float _speed = 5f;
    private int _currentPosition;
    
    private void Start()
    {
        //TODO: Change so path is passed during instantiation rather than searched for
        _path = GameObject.Find("Bloon Path").GetComponent<BloonPathCreator>().GetPathVectors();
        _currentPosition = 0;
        this.transform.position = _path[_currentPosition];
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
}
