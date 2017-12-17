using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSpeed : MonoBehaviour {

    public int fanSpeed = 30;
	// Use this for initialization
	void Start () {
		
	}

    private void onTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Throwable"))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.forward * fanSpeed, ForceMode.Acceleration);
        }
    }
}
