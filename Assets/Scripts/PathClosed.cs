using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathClosed : MonoBehaviour
{
    [SerializeField] private Button Button;

    public void DisableButton()
    {
        Button.interactable = false;
    }
    public void EnableButton()
    {
        Button.interactable = true;
    }
}
