using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    private LineRenderer laser; //for laser
    public GameObject teleportAimerObject; //object at end of laser
    public Vector3 teleportLocation; //location  to teleport to
    public GameObject player;
    public LayerMask laserMask; //which layers can the raycast collide with
    public float yNudgeAmount = 1f;
	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();

        
	}
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            laser.gameObject.SetActive(true);
            teleportAimerObject.SetActive(true);

            //set laser start position
            laser.SetPosition(0, gameObject.transform.position);
            //set laser end position
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 15, laserMask))
            {
                //if laser hits something
                teleportLocation = hit.point;
                laser.SetPosition(1, teleportLocation);
                teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);
            }
            else
            {
                //teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, transform.forward.y * 15 + transform.position.y, transform.forward.z * 15 + transform.position.z);
                teleportLocation = transform.position + transform.forward * 15;
                RaycastHit groundRay;
                if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, laserMask))
                {
                    teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, groundRay.point.y, transform.forward.z * 15 + transform.position.z);

                }
                laser.SetPosition(1, transform.position + transform.forward * 15);
                teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudgeAmount, 0);
            }
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            laser.gameObject.SetActive(false);
            teleportAimerObject.SetActive(false);
            player.transform.position = teleportLocation;
        }
	}
}
