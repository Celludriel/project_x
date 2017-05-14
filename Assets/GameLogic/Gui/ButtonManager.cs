using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour {

    public enum ButtonState { IDLE, MOVE, CAST_EFFECT }
    public GameContext gameContext;

    [HideInInspector]
    public CastEffectEnum effectToCast = CastEffectEnum.NONE;

    [HideInInspector]
    public ButtonState currentState = ButtonState.IDLE;

    private List<GameObject> activeButtons = new List<GameObject>();

    public void ButtonClicked()
    {
        if (EventSystem.current != null 
            && EventSystem.current.currentSelectedGameObject != null)
        {
            HandleMovementButton();
            HandleCastEffectButton();
        }
    }

    public void ToggleButtons(bool toggle)
    {
        UnityEngine.UI.Button[] buttons = gameContext.canvas.GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (var button in buttons)
        {
            button.interactable = toggle;
        }
    }

    public void EndButtonAction()
    {
        CancelButtonAction();
        gameContext.gameManager.gameState = GameManager.GameState.SWITCH;
    }

    private void CancelButtonAction()
    {
        currentState = ButtonState.IDLE;
        effectToCast = CastEffectEnum.NONE;
        activeButtons.Clear();
    }

    private void HandleMovementButton()
    {
        if(currentState == ButtonState.IDLE)
        {
            MovementButtonProperties movementButton = EventSystem.current.currentSelectedGameObject.GetComponent<MovementButtonProperties>();
            if (movementButton != null)
            {
                currentState = ButtonState.MOVE;
                ShipMover shipMover = GetActiveShipMover();
                shipMover.Move(movementButton.degrees, 4f);
            }
        }
    }

    private void HandleCastEffectButton()
    {
        if (currentState == ButtonState.IDLE)
        {
            CastButtonProperties castEffectButton = EventSystem.current.currentSelectedGameObject.GetComponent<CastButtonProperties>();
            if (castEffectButton != null)
            {
                currentState = ButtonState.CAST_EFFECT;
                GameObject buttonObject = castEffectButton.gameObject;              
                effectToCast = castEffectButton.castEffect;
                Ship origin = gameContext.initiativeManager.GetCurrentActiveShip().GetComponent<Ship>();
                CastEffectResolver castEffectResolver = gameContext.castEffectFactory.GetCastEffectResolver(effectToCast, origin);
                gameContext.castEffectPlayer.AddCastEffectResolver(castEffectResolver);
            }
        }
    }

    private ShipMover GetActiveShipMover()
    {
        Transform ship = gameContext.initiativeManager.GetCurrentActiveShip();
        ShipMover shipMover = ship.GetComponent<ShipMover>();
        return shipMover;
    }
}