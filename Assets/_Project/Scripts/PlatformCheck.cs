using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCheck : MonoBehaviour {

	public bool IsOnPlatform()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 15))
        {
            Debug.Log("Raycast hit "+ hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag == "Platform")
             {
                return true;
             }
        }
        return false;
    }
}
