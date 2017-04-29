using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour {

    public GameContext gameContext;

    public void ButtonClicked()
    {
        HandleMovementButton();
    }

    public void ToggleButtons(bool toggle)
    {
        UnityEngine.UI.Button[] buttons = gameContext.canvas.GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (var button in buttons)
        {
            button.interactable = toggle;
        }
    }

    private void HandleMovementButton()
    {
        if(EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            MovementButtonProperties movementButton = EventSystem.current.currentSelectedGameObject.GetComponent<MovementButtonProperties>();
            if (movementButton != null)
            {
                ShipMover shipMover = GetActiveShipMover();
                shipMover.Move(movementButton.degrees, 4f);
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