using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PurchaseZoneData
{
    public PurchaseZoneData(int _id, bool _purchased, int _currentPrice)
    {
        id = _id;
        purchased = _purchased;
        currentPrice = _currentPrice;
    }

    int id;
    bool purchased;
    int currentPrice;
}
