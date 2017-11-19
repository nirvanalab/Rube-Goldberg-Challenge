using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

    public Transform originalPosition;
    private int collectibleHitCount = 0;
    public int totalCollectibleToCollect;
    private List<GameObject> collectibles = new List<GameObject>();
    public SteamVR_LoadLevel loadLevel;
   
	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collission Enter With " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Entered Collision Ground");
            //set above the pedestal position
            gameObject.transform.position = new Vector3(1.162f,0.569f,0f);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            if (collectibleHitCount < totalCollectibleToCollect)
            {
                //reset the collectible count
                collectibleHitCount = 0;
                //enable all the collectibles
                foreach(GameObject collectible in collectibles)
                {
                    collectible.SetActive(true);
                }
                //remove all collectibles
                collectibles.Clear();
            }

        }
        if (collision.gameObject.CompareTag("Collectible"))
        {
            Debug.Log("Total Aim: " + LevelManager.collectibleTargetCount);
            Debug.Log("Entered Collision Collectible");
            //hide the collectible
            collision.gameObject.SetActive(false);
            //store the collectibles
            collectibles.Add(collision.gameObject);
            //increase  the count of the collectible
            collectibleHitCount += 1;

            if ( collectibleHitCount == totalCollectibleToCollect)
            {
                //load next level
                Debug.Log("Hit Collectibles " + collectibleHitCount);
                Debug.Log("Load Next Level");
                loadLevel.Trigger();
            }
        }
       
    }

    // Update is called once per frame
    void Update () {
		
	}
}
