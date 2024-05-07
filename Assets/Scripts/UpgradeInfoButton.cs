using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInfoButton : MonoBehaviour
{
    [SerializeField] private GameObject[] UpgradeInfo = new GameObject[3];

    public void ToggleInfo()
    {
        foreach (var info in UpgradeInfo) { 
            info.SetActive(!info.activeSelf);
        }
    }
}
