using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeTarget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetPriorityText;

    public delegate void SetTargetPriorityDelegate(string aPriority);
    public static event SetTargetPriorityDelegate setTargetPriority;

    private int targetIndex;
    private readonly List<string> targetPriorityList = new List<string> { 
        "First",
        "Last",
        "Close",
        "Strong"
    };
    private void Start()
    {
        targetIndex = 0;
        SetPriorityText();
    }
    public void ChangeTargetForward()
    {
        targetIndex = (targetIndex + 1) % targetPriorityList.Count;
        SetPriorityText();

    }
    public void ChangeTargetBackward() 
    {
        targetIndex = (targetIndex - 1 + targetPriorityList.Count) % targetPriorityList.Count;        
        SetPriorityText();
    }
    private void SetPriorityText()
    {
        SetTowerTargetPriority(targetPriorityList[targetIndex]);
        targetPriorityText.text = targetPriorityList[targetIndex];
    }

    private void SetTowerTargetPriority(string aPriority)
    {
        setTargetPriority?.Invoke(aPriority);
    }
}
