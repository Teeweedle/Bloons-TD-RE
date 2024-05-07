using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerSalePrice : MonoBehaviour
{
    private const float SELLPRICEPERCENT = 0.7f;
    [SerializeField] private TextMeshProUGUI SalePrice;
    private void OnEnable()
    {
        BaseTower._onUpdatePrice += UpdateSalePrice;
    }
    private void OnDisable()
    {
        BaseTower._onUpdatePrice -= UpdateSalePrice;
    }

    private void UpdateSalePrice(int aPrice)
    {
        SalePrice.text = ($"${(GetSellPrice(aPrice))}");
    }
    private string GetSellPrice(int aCost)
    {
        return Mathf.Round((aCost * SELLPRICEPERCENT)).ToString();
    }
}
