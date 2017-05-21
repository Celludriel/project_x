using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player
{
    public NetworkConnection conn;
    public short playerControllerId;
    public string playerName = "";
    public float startRotation = 0f;
    public Material material;
    
    private List<Transform> playerShips = new List<Transform>();

    public Player(NetworkConnection conn, short playerControllerId, string playerName, float startRotation, Material material)
    {
        this.conn = conn;
        this.playerControllerId = playerControllerId;
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
