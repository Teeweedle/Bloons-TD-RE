using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gOPath;
    [SerializeField] private GameObject _bloon;
    private List<Vector2> _bloonPath;
    void Start()
    {
        _bloonPath = _gOPath.GetComponent<BloonPathCreator>().GetPathVectors();
        StartCoroutine(SpawnBloons());
    }
    private IEnumerator SpawnBloons()
    {
        GameObject lBloon;
        for(int i = 0; i < 20;  i++)
        {
            lBloon = Instantiate(_bloon, _bloonPath[0], Quaternion.identity);
            lBloon.GetComponent<BloonMovement>().SetPath(_bloonPath);
            yield return new WaitForSeconds(1);
        }
    }
}
