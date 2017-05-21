using UnityEngine;

public class Ship : WorldObject {

    public int health;

    [HideInInspector]
    public Player owner;

    [HideInInspector]
    public GameObject selectionCirclePrefab;

    [HideInInspector]
    public GameObject targetCirclePrefab;

    [HideInInspector]
    public bool isTargeted = false;

    [HideInInspector]
    public bool isFleeing = false;

    [HideInInspector]
    public GameContext gameContext;
	
    public void SetSelected(bool select)
    {
        SelectableUnitComponent selectableUnitComponent = gameObject.GetComponent<SelectableUnitComponent>();
        if (select)
        {            
            selectableUnitComponent.selectionCircle = Instantiate(selectionCirclePrefab);
            selectableUnitComponent.selectionCircle.transform.SetParent(gameObject.transform, false);
            gameContext.informationManager.UpdateShipInfoPanel(this);
        }
        else
        {
            Destroy(selectableUnitComponent.selectionCircle.gameObject);
            selectableUnitComponent.selectionCircle = null;
        }
    }

    public void SetTargeted(bool targeted)
    {
        SelectableUnitComponent selectableUnitComponent = gameObject.GetComponent<SelectableUnitComponent>();
        if (targeted)
        {
            selectableUnitComponent.targetCircle = Instantiate(targetCirclePrefab);
            selectableUnitComponent.targetCircle.transform.SetParent(gameObject.transform, false);
            isTargeted = true;
        }
        else
        {
            GameObject targetCircle = selectableUnitComponent.targetCircle;
            if (targetCircle != null)
            {
                Destroy(targetCircle.gameObject);
                selectableUnitComponent.targetCircle = null;
            }
            isTargeted = false;
        }
    }

    public void CalculateTargetArc()
    {
        CalculateArcHalf(90);
        CalculateArcHalf(-90);
    }

    public void SetFleeing()
    {
        isFleeing = true;
    }

    public void OnMouseEnter()
    {
        GameObject canvas = gameContext.canvas.gameObject;
        canvas.transform.Find("HoverInfoPanel").gameObject.SetActive(true);
        gameContext.informationManager.UpdateHoverInfoPanel(this);
    }

    public void OnMouseExit()
    {
        GameObject canvas = gameContext.canvas.gameObject;
        canvas.transform.Find("HoverInfoPanel").gameObject.SetActive(false);
    }

    private void CalculateArcHalf(int targetAngleDegrees)
    {
        Quaternion targetAngle = Quaternion.AngleAxis(targetAngleDegrees, Vector3.up);
        RaycastHit hit;
        Quaternion angle = transform.rotation * targetAngle;
        Vector3 direction = angle * Vector3.forward;

        Vector3 pos = transform.position;
        pos = pos - (transform.forward * 0.7f);
        for (var i = 0; i < 17; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, 10))
            {
                Collider collidedObject = hit.collider;
                Ship enemy = collidedObject.GetComponent<Ship>();
                if (enemy != this)
                {
                    Debug.DrawRay(pos, direction * 10, Color.red, 20f);
                    if (!enemy.isTargeted && enemy.owner != this.owner)
                    {
                        enemy.SetTargeted(true);
                    }
                }
            }
            else
            {
                Debug.DrawRay(pos, direction * 10, Color.green, 20f);
            }
            pos = pos + (transform.forward * 0.1f);
        }
    }
}
