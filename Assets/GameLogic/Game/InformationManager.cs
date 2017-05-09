using UnityEngine;

public class InformationManager : MonoBehaviour {

    public void UpdateShipInfoPanel(Ship ship)
    {
        GameObject HealthText = GameObject.Find("HealthText");
        if(HealthText != null)
        {
            UnityEngine.UI.Text healthValue = HealthText.GetComponent<UnityEngine.UI.Text>();
            healthValue.text = ship.health.ToString();
        }
    }

    public void UpdateHoverInfoPanel(Ship ship)
    {
        GameObject hoverHealthText = GameObject.Find("HoverHealthText");
        if(hoverHealthText != null)
        {
            UnityEngine.UI.Text healthValue = hoverHealthText.GetComponent<UnityEngine.UI.Text>();
            healthValue.text = ship.health.ToString();
        }
    }
}
