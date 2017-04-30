using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour {

    public enum ButtonState { IDLE, MOVE, CAST_EFFECT }
    public GameContext gameContext;

    private ButtonState currentState = ButtonState.IDLE;
    private CastButtonProperties.CAST_EFFECTS effectToCast = CastButtonProperties.CAST_EFFECTS.NONE;

    public void ButtonClicked()
    {
        if (EventSystem.current != null 
            && EventSystem.current.currentSelectedGameObject != null
            && currentState == ButtonState.IDLE)
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
        currentState = ButtonState.IDLE;
        effectToCast = CastButtonProperties.CAST_EFFECTS.NONE;
    }

    private void HandleMovementButton()
    {
        MovementButtonProperties movementButton = EventSystem.current.currentSelectedGameObject.GetComponent<MovementButtonProperties>();
        if (movementButton != null)
        {
            currentState = ButtonState.MOVE;
            ShipMover shipMover = GetActiveShipMover();
            shipMover.Move(movementButton.degrees, 4f);
        }        
    }

    private void HandleCastEffectButton()
    {
        CastButtonProperties castEffectButton = EventSystem.current.currentSelectedGameObject.GetComponent<CastButtonProperties>();
        if (castEffectButton != null)
        {
            currentState = ButtonState.CAST_EFFECT;
            effectToCast = castEffectButton.castEffect;
        }
    }

    private ShipMover GetActiveShipMover()
    {
        Transform ship = gameContext.initiativeManager.GetCurrentActiveShip();
        ShipMover shipMover = ship.GetComponent<ShipMover>();
        return shipMover;
    }
}