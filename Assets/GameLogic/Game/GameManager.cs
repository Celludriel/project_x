using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public enum GameState { INIT, TURN, BUSY, SWITCH, GAME_OVER };
    public enum TurnState { ACTION_ONE, ACTION_TWO };

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
        turnState = TurnState.ACTION_ONE;
        CreatePlayers();
        CreateShipsForPlayers();
        SwitchControlPanelColor();
        DisableHoverPanel();
    }

    // Update is called once per frame
    void Update () {
        if (gameState != GameState.GAME_OVER)
        {
            switch (gameState)
            {
                case GameState.INIT: SetFirstPlayer(); break;
                case GameState.SWITCH: DoSwitch(); break;
                default: break;
            }
        }
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
        gameContext.canvas.transform.FindChild("HoverInfoPanel").gameObject.SetActive(false);
    }

    private void CreatePlayers()
    {
        players.Add(new Player("player one", 0f, gameContext.playerOneMaterial));
        players.Add(new Player("player two", 180f, gameContext.playerTwoMaterial));
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
        for (var x = 0; x < 5; x++)
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
        Transform controlPanel = gameContext.canvas.transform.FindChild("ControlPanel");
        Image image = controlPanel.GetComponent<Image>();
        image.color = owner.material.color;
    }

    private void DoSwitch()
    {
        switch (turnState)
        {
            case TurnState.ACTION_TWO: SwitchTurn(); break;
            case TurnState.ACTION_ONE: SwitchAction(); break;            
        }
        gameState = GameState.TURN;
    }

    private void SwitchAction()
    {
        turnState = TurnState.ACTION_TWO;
        Transform currentActive = gameContext.initiativeManager.GetCurrentActiveShip();
        Ship shipComponent = currentActive.GetComponent<Ship>();
        RemoveAllTargets();
        shipComponent.CalculateTargetArc();
    }

    private void SwitchTurn()
    {
        Transform currentActive = gameContext.initiativeManager.MoveToNextShipInLine();
        Ship shipComponent = currentActive.GetComponent<Ship>();

        RemoveAllTargets();
        shipComponent.CalculateTargetArc();

        ResetTurn();
    }

    private void ResetTurn()
    {
        SwitchControlPanelColor();
        turnState = TurnState.ACTION_ONE;
        gameContext.buttonManager.ToggleButtons(true);
    }
}