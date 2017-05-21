using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameContextAwareNetworkManager : NetworkManager {

    public GameContext gameContext;

    public override void OnStartHost()
    {
        Debug.Log("Host has been started");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        Debug.Log("Player added to server");
        gameContext.gameManager.playerConnected(conn, playerControllerId);

    }
}
