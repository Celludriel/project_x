using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public enum GameState { INIT, WAITING_FOR_PLAYER_ONE, PLAYER_ONE_CONNECTED, WAITING_FOR_PLAYER_TWO, PLAYER_TWO_CONNECTED, SETUP, TURN, BUSY, SWITCH, GAME_OVER };
    public enum TurnState { IDLE, ACTION_ONE, ACTION_TWO, FORCED_TURN_SWITCH };

    public GameContext gameContext;

    [HideInInspector]
    public GameState gameState;

    [HideInInspector]
    public TurnState turnState;

    private List<Player> players = new List<Player>(2);

    // Use this for initialization
    void Start ()
    {
        gameState = GameState.INIT;
        turnState = TurnState.IDLE;
        // do stuff
        gameState = GameState.WAITING_FOR_PLAYER_ONE;
    }

    // Update is called once per frame
    void Update () {
        if (gameState != GameState.GAME_OVER)
        {
            switch (gameState)
            {
                case GameState.SETUP: SetupBattle(); break;
                case GameState.SWITCH: DoSwitch(); break;
                default: break;
            }
        }
    }

    private void SetupBattle()
    {
        Debug.Log("Setting up the battle");
        gameState = GameState.TURN;
    }

    public void playerConnected(NetworkConnection conn, short playerControllerId)
    {
        switch (gameState)
        {
            case GameState.WAITING_FOR_PLAYER_ONE: CreateFirstPlayer(conn, playerControllerId); break;
            case GameState.WAITING_FOR_PLAYER_TWO: CreateSecondPlayer(conn, playerControllerId); break;
            default: break;
        }
    }

    private void CreateSecondPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log(string.Format("Creating second player {0}, {1}", conn, playerControllerId));
        gameState = GameState.PLAYER_TWO_CONNECTED;
        players.Add(new Player(conn, playerControllerId, "player two", 180f, gameContext.playerTwoMaterial));
        gameState = GameState.SETUP;
    }

    private void CreateFirstPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log(string.Format("Creating first player {0}, {1}", conn, playerControllerId));
        gameState = GameState.PLAYER_ONE_CONNECTED;
        players.Add(new Player(conn, playerControllerId, "player one", 0f, gameContext.playerOneMaterial));
        gameState = GameState.WAITING_FOR_PLAYER_TWO;
    }

    public void RemoveAllTargets()
    {
        foreach (Player player in players)
        {
            player.UntargetAllShips();
        }
    }

    private void DisableHoverPanel()
    {
        gameContext.canvas.transform.Find("HoverInfoPanel").gameObject.SetActive(false);
    }

    private void CreateShipsForPlayers()
    {
        float offset = -10f;
        foreach (var player in players)
        {
            SpawnShips(player, offset, player.startRotation);
            offset = offset + 20f;
        }
    }

    private void SpawnShips(Player player, float offset, float rotation)
    {
        float space = 0f;
        float xOrigin = -8f;
        for (var x = 0; x < 2; x++)
        {
            Quaternion qRotation = Quaternion.AngleAxis(rotation, Vector3.up);
            Transform ship = SpawnShip(player, offset, space, xOrigin, qRotation);
            player.AddShip(ship);
            gameContext.initiativeManager.AddToInitiativeList(ship);
            space += 2f;
            xOrigin += 2f;
        }
    }

    private Transform SpawnShip(Player player, float offset, float space, float x, Quaternion qRotation)
    {
        Transform ship = Instantiate(gameContext.shipPrefab, new Vector3(x + space, 1, offset), qRotation);

        ship.GetComponent<Renderer>().material = player.material;

        Ship shipComponent = ship.GetComponent<Ship>();
        shipComponent.health = UnityEngine.Random.Range(100, 200);
        shipComponent.selectionCirclePrefab = gameContext.selectionCirclePrefab;
        shipComponent.targetCirclePrefab = gameContext.targetCirclePrefab;
        shipComponent.gameContext = gameContext;

        ShipMover shipMover = ship.GetComponent<ShipMover>();
        shipMover.gameContext = gameContext;

        return ship;
    }

    private void SetFirstPlayer()
    {
        Transform currentActive = gameContext.initiativeManager.GetCurrentActiveShip();
        Ship shipComponent = currentActive.GetComponent<Ship>();
        shipComponent.SetSelected(true);
        shipComponent.CalculateTargetArc();
        gameState = GameState.TURN;
        ResetTurn();
    }

    private void SwitchControlPanelColor()
    {
        Ship currentActiveShip = gameContext.initiativeManager.GetCurrentActiveShip().GetComponent<Ship>();
        Player owner = currentActiveShip.owner;
        Transform controlPanel = gameContext.canvas.transform.Find("ControlPanel");
        Image image = controlPanel.GetComponent<Image>();
        image.color = owner.material.color;
    }

    private void DoSwitch()
    {        
        switch (turnState)
        {
            case TurnState.ACTION_TWO: SwitchTurn(); break;
            case TurnState.ACTION_ONE: SwitchAction(); break;
            case TurnState.FORCED_TURN_SWITCH: SwitchTurn(); break;
        }        
    }

    private void SwitchAction()
    {
        Transform currentActive = gameContext.initiativeManager.GetCurrentActiveShip();
        Ship shipComponent = currentActive.GetComponent<Ship>();

        if (shipComponent.isFleeing)
        {
            turnState = TurnState.FORCED_TURN_SWITCH;
            gameState = GameState.SWITCH;
        }
        else
        {
            turnState = TurnState.ACTION_TWO;
            RemoveAllTargets();
            shipComponent.CalculateTargetArc();
            gameState = GameState.TURN;
        }
    }

    private void SwitchTurn()
    {
        Transform currentActiveShipTransform = gameContext.initiativeManager.GetCurrentActiveShip();
        Ship currentActiveShip = currentActiveShipTransform.GetComponent<Ship>();
        Transform nextActiveShipTransform = gameContext.initiativeManager.MoveToNextShipInLine();
        Ship nextActiveShip = nextActiveShipTransform.GetComponent<Ship>();

        RemoveAllTargets();
        nextActiveShip.CalculateTargetArc();

        HandleFleeingIfAvailable(currentActiveShip);

        ResetTurn();
    }

    private void HandleFleeingIfAvailable(Ship currentActiveShip)
    {
        if (currentActiveShip.isFleeing)
        {
            gameContext.initiativeManager.RemoveShip(currentActiveShip);
            currentActiveShip.owner.RemoveShip(currentActiveShip);
            Destroy(currentActiveShip.gameObject);
        }
    }

    private void ResetTurn()
    {
        SwitchControlPanelColor();
        gameState = GameState.TURN;
        turnState = TurnState.ACTION_ONE;
        gameContext.buttonManager.ToggleButtons(true);
    }
}