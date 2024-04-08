using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSelected : MonoBehaviour, IPointerClickHandler
{
    private static TowerSelected _instance;

    private void Start()
    {
        DeselectImage();
    }
    /// <summary>
    /// Hide item selected image
    /// </summary>
    private void DeselectImage()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_instance != null)
            _instance.DeselectImage();
        
        this.transform.GetChild(0).gameObject.SetActive(true);
        _instance = this;
       
    }
}
