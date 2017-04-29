﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeManager : MonoBehaviour {

    private LinkedList<Transform> initiativeList = new LinkedList<Transform>();

    public Transform GetCurrentActiveShip()
    {
        return initiativeList.First.Value;
    }

    public void AddToInitiativeList(Transform transform)
    {
        initiativeList.AddLast(transform);
    }

    public Transform MoveToNextShipInLine()
    {
        Transform currentActiveShip = GetCurrentActiveShip();
        currentActiveShip.GetComponent<Ship>().SetSelected(false);
        initiativeList.RemoveFirst();
        initiativeList.AddLast(currentActiveShip);
        currentActiveShip = GetCurrentActiveShip();
        currentActiveShip.GetComponent<Ship>().SetSelected(true);
        return currentActiveShip;
    }
}
