using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CrateInstantiate : MonoBehaviourPun
{
    #region fields
/// <summary>
///  game object for crates
/// </summary>
    public GameObject crateObject;
    /// <summary>
    ///  current crate
    /// </summary>
    public GameObject currentCrate;
    /// <summary>
    /// preview crate
    /// </summary>
    public GameObject secondaryCrate;
    /// <summary>
    /// array of crates
    /// </summary>
    public GameObject[] crates;
    /// <summary>
    /// array of crate spawners
    /// </summary>
    public GameObject[] crateSpawners;
    /// <summary>
    /// time remaining for spawning
    /// </summary>
    public float remaining = 1;
    /// <summary>
    /// time is running bool
    /// </summary>
    public bool timer;
    /// <summary>
    /// crate spawn bool preset to false
    /// </summary>
    public bool spawned = false;
    /// <summary>
    /// crate to spawn
    /// </summary>
    public int crateToSpawn;

    #endregion



    #region start and update fucntion
    private void Start()
    {
        // Starts the timer automatically
        timer = true;
        spawned = false;
        
       
    }

    void Update()
    {
        // is this the masterclient
        if (PhotonNetwork.IsMasterClient) // checks if master client
        {
            if (timer == true) // if time is running
            {
                if (remaining > 0) // if time is not 0
                {
                    remaining -= Time.deltaTime; // start timer
                    
                }
                else
                {
                    remaining = 0; // timer set to 0
                    crateToSpawn = Random.Range(0, 4); // radomise crates to spawn
                    // function  for all other players
                    this.photonView.RPC("crateToSpawnRPC", RpcTarget.All, crateToSpawn);
                    // spawn crate function for masterclient
                    secondaryCrate = (GameObject)Instantiate(crates[crateToSpawn], this.transform); // instantiate crate
                    secondaryCrate.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);// position crate
                    CratePosition cratePos = secondaryCrate.GetComponent<CratePosition>();// gets crate position component
                    cratePos.held = false; // crate held is false
                    secondaryCrate.transform.localPosition = new Vector3(0.04f, -0.34f, -0.1f); // set position
                    spawned = true; // bool set to true
                    timer = false; // set to false
                }
            }
        }

    }
    #endregion

    #region methods
    /// <summary>
    /// on trigger enter for holding cube and spawwning more crates
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player") 
        {
            if (spawned == true) // crate spawned is true
            {
                PlayerMovementScript playerScript = other.GetComponent<PlayerMovementScript>(); // get component
                bool crateHeld = playerScript.holding; // gets holding reference
                if (crateHeld == false)
                {
                    currentCrate = (GameObject)Instantiate(crates[crateToSpawn], other.transform); // instantiate crate
                    crateHeld = true; // crate helf is true
                    playerScript.holding = crateHeld; // 
                    playerScript.CrateAccessesed(currentCrate); // get current crate
                    Destroy(secondaryCrate); // destroy crate
                    spawned = false; // crate respawned is false
                    remaining = 8; //  time remaining 
                    timer = true; //  time is now running
                }

            }





        }

    }
    #endregion

    #region RPC function

    /// <summary>
    /// function that will run to all players in the game over the network
    /// </summary>
    /// <param name="crateToSpawnRef"></param>
    [PunRPC]
    public void crateToSpawnRPC(int crateToSpawnRef)
    {
        if (!PhotonNetwork.IsMasterClient) // if it sin't the masterclient
        {
            crateToSpawn = crateToSpawnRef;

            secondaryCrate = (GameObject)Instantiate(crates[crateToSpawn], this.transform); // instantiate crate
            secondaryCrate.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f); // position crate
            CratePosition cratePos = secondaryCrate.GetComponent<CratePosition>(); // gets crate position component
            cratePos.held = false; // held equals false
            secondaryCrate.transform.localPosition = new Vector3(0.04f, -0.34f, -0.1f); // position the crate again
            spawned = true; // crate spawne is true
           
        }



    }
    #endregion

}
