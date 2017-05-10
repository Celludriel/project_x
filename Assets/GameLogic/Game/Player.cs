using System;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    public string playerName = "";
    public float startRotation = 0f;
    public Material material;
    
    private List<Transform> playerShips = new List<Transform>();

    public Player(string playerName, float startRotation, Material material)
    {
        this.playerName = playerName;
        this.startRotation = startRotation;
        this.material = material;
    }

    public void AddShip(Transform ship)
    {
        playerShips.Add(ship);
        Ship shipScript = ship.GetComponent<Ship>();
        shipScript.owner = this;
    }
    
    public void UntargetAllShips()
    {
        foreach(Transform ship in playerShips)
        {
            ship.GetComponent<Ship>().SetTargeted(false);
        }
    }

    public override string ToString()
    {
        return playerName;
    }

    public void RemoveShip(Ship currentActiveShip)
    {
        playerShips.Remove(currentActiveShip.transform);
    }
}
