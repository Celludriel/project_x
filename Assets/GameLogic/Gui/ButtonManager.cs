using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public void DisableActionButtons()
    {
        UnityEngine.UI.Button[] buttons = gameContext.canvas.GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (var button in buttons)
        {
            if(button.name == "ShootButton" || button.name == "SpecialButton")
            {
                button.interactable = false;
            }            
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
        foreach (GameObject buttonObject in activeButtons)
        {
            buttonObject.GetComponent<Image>().color = Color.white;
        }
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
                GameObject buttonObject = castEffectButton.gameObject;
                buttonObject.GetComponent<Image>().color = Color.green;
                currentState = ButtonState.CAST_EFFECT;
                effectToCast = castEffectButton.castEffect;
                activeButtons.Add(buttonObject);
            }
        }
        else if (currentState == ButtonState.CAST_EFFECT)
        {
            CancelButtonAction();
        }
    }

    private ShipMover GetActiveShipMover()
    {
        Transform ship = gameContext.initiativeManager.GetCurrentActiveShip();
        ShipMover shipMover = ship.GetComponent<ShipMover>();
        return shipMover;
    }
}