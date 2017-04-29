using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour {

    public void UpdateShipInfoPanel(Ship ship)
    {
        UnityEngine.UI.Text healthValue = GameObject.Find("HealthText").GetComponent<UnityEngine.UI.Text>();
        healthValue.text = ship.health.ToString();
    }

    public void UpdateHoverInfoPanel(Ship ship)
    {
        UnityEngine.UI.Text healthValue = GameObject.Find("HoverHealthText").GetComponent<UnityEngine.UI.Text>();
        healthValue.text = ship.health.ToString();
    }
}
