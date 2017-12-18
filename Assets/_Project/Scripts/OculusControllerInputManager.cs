using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusControllerInputManager : MonoBehaviour {


    private LineRenderer laser; //for laser
    public GameObject teleportAimerObject; //object at end of laser
    public Vector3 teleportLocation; //location  to teleport to
    public GameObject player;
    public LayerMask laserMask; //which layers can the raycast collide with
    public float yNudgeAmount = 0f;
    public float moveDistance = 5f;
    private OVRInput.Controller thisController;
    public bool leftHand = false;
    public float throwForce = 1.5f;
    public ObjectMenuManager objectMenuManager;
    public GameObject objectMenu;
    private bool menuIsSwipable;
    private float menuStickX;
    private bool isGrabbingObject = false;
    public PlatformCheck platformCheck;
    private bool isOnPlatform = false;

    // Use this for initialization
    void Start () {
        laser = GetComponentInChildren<LineRenderer>();     
        if (leftHand)
        {
            thisController = OVRInput.Controller.LTouch;
        }
        else
        {
            thisController = OVRInput.Controller.RTouch;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (leftHand)
        {
            handleTeleportation();
        }     
        else
        {

            menuStickX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, thisController).x;
            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, thisController))
            {
                enableObjectMenu();
                if (menuStickX < 0.45f && menuStickX > -0.45f)
                {
                    menuIsSwipable = true;
                }
                if (menuIsSwipable)
                {
                    if (menuStickX >= 0.45f)
                    {
                        swipeRight();
                        menuIsSwipable = false;
                    }
                    else if (menuStickX <= -0.45f)
                    {
                        swipeLeft();
                        menuIsSwipable = false;
                    }
                }
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController))
                {
                    SpawnObject();
                }
            }

            if (OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, thisController))
            {
                disableObjectMenu();
            }



            //if press button A 
        }
	}

    void SpawnObject()
    {
        objectMenuManager.SpawnCurrentObject();
    }

    public void swipeLeft()
    {
        objectMenuManager.MenuLeft();
       // Debug.Log("Swipe Left");
    }

    public void swipeRight()
    {
        objectMenuManager.MenuRight();
       // Debug.Log("Swipe Right");
    }

    public void enableObjectMenu()
    {
        objectMenu.SetActive(true);
    }

    public void disableObjectMenu()
    {
        objectMenu.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("On Trigger Enter " + col.gameObject.name);
        if (col.gameObject.name == "Platform")
        {
            isOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        Debug.Log("On Trigger Enter " + col.gameObject.name);
        if (col.gameObject.name == "Platform")
        {
            isOnPlatform = false;
        }
    }
    private void OnCollisionStay(Collision col)
    {
        Debug.Log("On Colliding " + col.gameObject.name);
    }
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("On Collision Enter " + col.gameObject.name);
    }
    private void OnCollisionExit(Collision col)
    {
        Debug.Log("On Collision Exit " + col.gameObject.name);
    }
    private void OnTriggerStay(Collider col)
    {
        
        if (col.gameObject.CompareTag("Throwable") || col.gameObject.CompareTag("Structure"))
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController) < 0.3f)
            {
                //release
                ThrowObject(col);
            }

            else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController) > 0.3f)
            {
                //grab
                if (!isGrabbingObject)
                {
                    GrabObject(col);
                }
               
            }

        }
    }

    private void GrabObject(Collider col)
    {
        col.transform.SetParent(gameObject.transform);
        col.GetComponent<Rigidbody>().isKinematic = true; //to avoid gravity
        isGrabbingObject = true;
        //haptic feedback
       // Debug.Log("Grabbed the object");
    }

    private void ThrowObject(Collider col)
    {
        col.transform.SetParent(null);
        Rigidbody rigidBody = col.GetComponent<Rigidbody>();
       // Debug.Log("Throwing " + col.gameObject.name);
        if (rigidBody == null )
        {
            return;
        }
        if (col.gameObject.CompareTag("Throwable"))
        {
            rigidBody.isKinematic = false;
           // col.gameObject.GetComponent<Collider>().isTrigger = true;
            rigidBody.velocity = OVRInput.GetLocalControllerVelocity(thisController) * throwForce;
            rigidBody.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(thisController);

            //set the corresponding material depending on where the player threw
            //
            if (isGrabbingObject )
            {
                BallReset ballScript = col.gameObject.GetComponent<BallReset>();
                bool IsStandingOnPlatform = platformCheck.IsOnPlatform();
                if (IsStandingOnPlatform)
                {
                    ballScript.ApplyGreenGlow();
                }
                else
                {
                    ballScript.ApplyRedGlow();
                }
            }
           
        }
        isGrabbingObject = false;

       // Debug.Log("Released Object");
    }

    private void handleTeleportation()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
           // Debug.Log("Trigger Down");
            laser.gameObject.SetActive(true);
            teleportAimerObject.SetActive(true);

            //set laser start position
            laser.SetPosition(0, gameObject.transform.position);
            //set laser end position
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, moveDistance))
            {
                //hit.collider.gameObject.layer
               // Debug.Log("Hit Object");
                //if laser hits something
                //if ( hit.collider.tag == "floor" )
                // {
                teleportLocation = hit.point;
                laser.SetPosition(1, teleportLocation);
                teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);
                // }
            }
            else
            {
               // Debug.Log("Didn't Hit");
                //teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, transform.forward.y * 15 + transform.position.y, transform.forward.z * 15 + transform.position.z);
                teleportLocation = transform.position + (transform.forward * moveDistance);
                RaycastHit groundRay;
                if (Physics.Raycast(teleportLocation, Vector3.down, out groundRay, 17))
                {
                    //Debug.Log("Hit Ground!!!");
                    //teleportLocation = groundRay.point;
                    teleportLocation = new Vector3((transform.forward.x * moveDistance) + transform.position.x, groundRay.point.y, (transform.forward.z * moveDistance) + transform.position.z);
                }
                else
                {
                    teleportLocation = Vector3.zero;
                }
               // Debug.Log("Teleport Location" + teleportLocation);
                laser.SetPosition(1, (transform.forward * moveDistance) + transform.position);
                teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudgeAmount, 0);

            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {

            if (teleportLocation != Vector3.zero)
            {
                laser.gameObject.SetActive(false);
                teleportAimerObject.SetActive(false);
                //player.transform.position = teleportLocation;
                player.transform.position = new Vector3(teleportLocation.x, teleportLocation.y+1.2f, teleportLocation.z); ;
            }
            else
            {
                laser.gameObject.SetActive(false);
                teleportAimerObject.SetActive(false);

                player.transform.position = player.transform.position;
            }
            // Debug.Log("Trigger Up");
            /*laser.gameObject.SetActive(false);
            teleportAimerObject.SetActive(false);
            player.transform.position = new Vector3(teleportLocation.x, 1, teleportLocation.z);*/
            //player.transform.position = teleportLocation;
        }
    }
}
