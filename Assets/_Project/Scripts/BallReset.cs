using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour
{

    public Transform originalPosition;
    private int collectibleHitCount = 0;
    public int totalCollectibleToCollect;
    private List<GameObject> collectibles = new List<GameObject>();
    public SteamVR_LoadLevel loadLevel;
    public bool didHitGoal = false;
    Vector3 ballOriginalPosition;
    public Material greenGlow;
    public Material redGlow;
    public Material originalMat;
    public bool isOnPlatform = true;

    // Use this for initialization
    void Start()
    {
        ballOriginalPosition = transform.position;
        originalMat = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collission Enter With " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Entered Collision Ground");
            //set above the pedestal position
            gameObject.transform.position = ballOriginalPosition;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            ApplyOriginal();

            //if did not collect all collectibles OR if did not hit goal OR if not is on Platform
            if ((collectibleHitCount < totalCollectibleToCollect) || !didHitGoal || !isOnPlatform)
            {
                //reset the collectible count
                collectibleHitCount = 0;
                //enable all the collectibles
                foreach (GameObject collectible in collectibles)
                {
                    collectible.SetActive(true);
                }
                //remove all collectibles
                collectibles.Clear();
                didHitGoal = false;
            }

        }
        if (collision.gameObject.CompareTag("Collectible") && isOnPlatform)
        {
            Debug.Log("Total Aim: " + LevelManager.collectibleTargetCount);
            Debug.Log("Entered Collision Collectible");
            //hide the collectible
            collision.gameObject.SetActive(false);
            //store the collectibles
            collectibles.Add(collision.gameObject);
            //increase  the count of the collectible
            collectibleHitCount += 1;

            /* if ( collectibleHitCount == totalCollectibleToCollect)
             {
                 //load next level
                 Debug.Log("Hit Collectibles " + collectibleHitCount);
                 GoToNextLevel();
             } */
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("Entered Collision Goal!!");
            if ((collectibleHitCount == totalCollectibleToCollect) && isOnPlatform)
            {
                //load next level
                Debug.Log("Hit Goal");
                GoToNextLevel();
                didHitGoal = true;
            }
        }

    }

    public void ApplyGreenGlow()
    {
        GetComponent<Renderer>().material = greenGlow;
        isOnPlatform = true;
    }
    public void ApplyRedGlow()
    {
        GetComponent<Renderer>().material = redGlow;
        isOnPlatform = false;
    }

    public void ApplyOriginal()
    {
        GetComponent<Renderer>().material = originalMat;
        isOnPlatform = false;
    }

    void GoToNextLevel()
    {
        Debug.Log("Load Next Level");
        loadLevel.Trigger();
    }

}
