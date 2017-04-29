using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RtsCamera : MonoBehaviour {

    public LayerMask groundLayer;

    [System.Serializable]
    public class PositionSettings
    {
        public bool invertPan = true;
        public float panSmooth = 7f;
        public float distanceFromGround = 40f;
        public bool allowZoom = true;
        public float zoomSmooth = 5f;
        public float zoomStep = 5f;
        public float maxZoom = 10f;
        public float minZoom = 30f;
        public float northLimit = 10f;
        public float eastLimit = 10f;
        public float southLimit = 10f;
        public float westLimit = 10f;

        [HideInInspector]
        public float newDistance = 40f;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = 50f;
        public float yRotation = 0f;
        public bool allowYOrbit = true;
        public float yOrbitSmooth = 10f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string PAN = "MousePan";
        public string ORBIT_Y = "MouseTurn";
        public string ZOOM = "Mouse ScrollWheel";
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    Vector3 destination = Vector3.zero;
    Vector3 camVel = Vector3.zero;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;
    float panInput, orbitInput, zoomInput;
    int panDirection = 0;

    void Start()
    {
        //init
        panInput = 0;
        orbitInput = 0;
        zoomInput = 0;
    }

    void GetInput()
    {
        //setting input variable
        panInput = Input.GetAxis(input.PAN);
        orbitInput = Input.GetAxis(input.ORBIT_Y);
        zoomInput = Input.GetAxis(input.ZOOM);

        previousMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //update input
            GetInput();

            //zooming
            if (position.allowZoom)
            {
                Zoom();
            }

            //rotating
            if (orbit.allowYOrbit)
            {
                Rotate();
            }

            //panning
            PanWorld();
        }
    }

    void FixedUpdate()
    {
        //handle camera distance (raycasting always in fixed update)
        HandleCameraDistance();
    }

    void PanWorld()
    {
        Vector3 targetPos = transform.position;

        if (position.invertPan)
        {
            panDirection = -1;
        }
        else
        {
            panDirection = 1;
        }

        if(panInput > 0)
        {
            targetPos += transform.right * (currentMousePos.x - previousMousePos.x) * position.panSmooth * panDirection * Time.deltaTime;
            targetPos += Vector3.Cross(transform.right, Vector3.up) * (currentMousePos.y - previousMousePos.y) * position.panSmooth * panDirection * Time.deltaTime;
            
            if(targetPos.z > position.northLimit)
            {
                targetPos.z = position.northLimit;
            }

            if (targetPos.x > position.eastLimit)
            {
                targetPos.x = position.eastLimit;
            }

            if (targetPos.z < -Mathf.Abs(position.southLimit))
            {
                targetPos.z = -Mathf.Abs(position.southLimit);
            }

            if (targetPos.x < -Mathf.Abs(position.westLimit))
            {
                targetPos.x = -Mathf.Abs(position.westLimit);
            }
        }
        transform.position = targetPos;
    }

    void HandleCameraDistance()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            destination = Vector3.Normalize(transform.position - hit.point) * position.distanceFromGround;
            destination += hit.point;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, 0.3f);
        }
    }

    void Zoom()
    {
        position.newDistance += position.zoomStep * -zoomInput;

        position.distanceFromGround = Mathf.Lerp(position.distanceFromGround, position.newDistance, position.zoomSmooth);

        if(position.distanceFromGround < position.maxZoom)
        {
            position.distanceFromGround = position.maxZoom;
            position.newDistance = position.maxZoom;
        }

        if(position.distanceFromGround > position.minZoom)
        {
            position.distanceFromGround = position.minZoom;
            position.newDistance = position.minZoom;
        }
    }

    void Rotate()
    {
        if(orbitInput > 0)
        {
            orbit.yRotation += (currentMousePos.x - previousMousePos.x) * orbit.yOrbitSmooth * Time.deltaTime;
        }

        transform.rotation = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0);
    }
}
