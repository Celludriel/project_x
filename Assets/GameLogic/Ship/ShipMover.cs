using UnityEngine;

public class ShipMover : MonoBehaviour {

    public float speed = 1f;

    [HideInInspector]
    public GameContext gameContext;

    [HideInInspector]
    public bool moving = false;

    private Vector3 destination;

    // Use this for initialization
    void Start () {
        destination = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position != destination)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
        }
        else if (moving)
        {
            moving = false;
            gameContext.buttonManager.ToggleButtons(true);
            gameContext.buttonManager.EndButtonAction();
            gameContext.gameManager.gameState = GameManager.GameState.SWITCH;
        }
	}

    public void Move(float degrees, float distance)
    {
        // local coordinate rotation around the Y axis to the given angle
        Quaternion rotation = Quaternion.AngleAxis(degrees, Vector3.up);        
        // add the desired distance to the direction
        Vector3 addDistanceToDirection = rotation * transform.forward * distance;
        // add the distance and direction to the current position to get the final destination
        this.destination = transform.position + addDistanceToDirection;
        Debug.DrawRay(transform.position, addDistanceToDirection, Color.red, 10.0f);
        transform.LookAt(this.destination);

        // moving is going to start disable buttons
        moving = true;
        gameContext.buttonManager.ToggleButtons(false);

        // notify GameManager moving is going on
        gameContext.gameManager.gameState = GameManager.GameState.BUSY;
    }

    void OnCollisionEnter(Collision col)
    {
        if (moving)
        {
            destination = transform.position + (transform.forward * -0.2f);
        }        
    }
}