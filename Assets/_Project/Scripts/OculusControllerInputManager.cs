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
	// Use this for initialization
	void Start () {
        laser = GetComponentInChildren<LineRenderer>();       
	}
	
	// Update is called once per frame
	void Update () {

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("Trigger Down");
            laser.gameObject.SetActive(true);
            teleportAimerObject.SetActive(true);

            //set laser start position
            laser.SetPosition(0, gameObject.transform.position);
            //set laser end position
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, moveDistance,laserMask))
            {
                Debug.Log("Hit Object");
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
                Debug.Log("Didn't Hit");
                //teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, transform.forward.y * 15 + transform.position.y, transform.forward.z * 15 + transform.position.z);
                teleportLocation = transform.position + (transform.forward * moveDistance);
                RaycastHit groundRay;
                if (Physics.Raycast(teleportLocation, Vector3.down, out groundRay, 17,laserMask))
                {
                    Debug.Log("Hit Ground!!!");
                    //teleportLocation = groundRay.point;
                    teleportLocation = new Vector3((transform.forward.x * moveDistance) + transform.position.x, groundRay.point.y, (transform.forward.z * moveDistance) + transform.position.z);
                 }
                laser.SetPosition(1, (transform.forward * moveDistance) + transform.position);
                teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudgeAmount, 0);

            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("Trigger Up");
            laser.gameObject.SetActive(false);
            teleportAimerObject.SetActive(false);
           // player.transform.position = new Vector3(teleportLocation.x, 1, teleportLocation.z);
            player.transform.position = teleportLocation;
        }
	}
}
