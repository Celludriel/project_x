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
        Quaternion startingAngle = Quaternion.AngleAxis(50, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(2, Vector3.up);

        CalculateArcHalf(startingAngle, stepAngle);

        startingAngle = Quaternion.AngleAxis(-50, Vector3.up);
        stepAngle = Quaternion.AngleAxis(-2, Vector3.up);

        CalculateArcHalf(startingAngle, stepAngle);
    }

    private void CalculateArcHalf(Quaternion startingAngle, Quaternion stepAngle)
    {
        RaycastHit hit;
        Quaternion angle = transform.rotation * startingAngle;
        Vector3 direction = angle * Vector3.forward;
        Vector3 pos = transform.position;
        for (var i = 0; i < 40; i++)
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
            direction = stepAngle * direction;
        }
    }

    public void OnMouseEnter()
    {
        GameObject canvas = gameContext.canvas.gameObject;
        canvas.transform.FindChild("HoverInfoPanel").gameObject.SetActive(true);
        gameContext.informationManager.UpdateHoverInfoPanel(this);
    }

    public void OnMouseExit()
    {
        GameObject canvas = gameContext.canvas.gameObject;
        canvas.transform.FindChild("HoverInfoPanel").gameObject.SetActive(false);
    }

    public void OnMouseUpAsButton()
    {
        ButtonManager buttonManager = gameContext.buttonManager;        
        if (buttonManager.currentState == ButtonManager.ButtonState.CAST_EFFECT
            && isTargeted)
        {
            CastEffectEnum effectToCast = buttonManager.effectToCast;
            CastEffectResolver resolver = gameContext.castEffectFactory.GetCastEffectResolver(effectToCast);
            Ship origin = gameContext.initiativeManager.GetCurrentActiveShip().GetComponent<Ship>();
            resolver.ResolveCastEffect(origin, this);
            gameContext.informationManager.UpdateHoverInfoPanel(this);
            buttonManager.EndButtonAction();
            buttonManager.DisableActionButtons();
        }        
    }
}
