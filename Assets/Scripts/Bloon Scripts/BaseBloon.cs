using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBloon : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private bool isStrong;
    // Start is called before the first frame update
    void Start()
    {
        distance = 0.0f;
        isStrong = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance += Time.deltaTime;
    }
    public float GetBloonDistance()
    {
        return distance;
    }
    public bool GetIsStrong()
    {
        return isStrong;
    }
}
