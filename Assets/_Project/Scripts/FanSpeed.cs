using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSpeed : MonoBehaviour {

    public int fanSpeed = 30;
	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerStay(Collider Col)
    {
        Debug.Log("Collision with Fan");
        if (Col.gameObject.name == "Ball")
        {
            Debug.Log("Collision Entered with Fan by Ball");
            Col.GetComponent<Rigidbody>().AddForce(-transform.forward * fanSpeed, ForceMode.Acceleration);
        }
    }
}
