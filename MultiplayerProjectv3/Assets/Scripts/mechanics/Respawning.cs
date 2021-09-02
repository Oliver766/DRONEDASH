using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Respawning : MonoBehaviourPunCallbacks
{
    #region fields
    /// <summary>
    /// avatatar set up reference
    /// </summary>
    private avatarsetUp setup;
    /// <summary>
    /// phontonview reference
    /// </summary>
    PhotonView PV;
    /// <summary>
    /// crate reference
    /// </summary>
    public GameObject crate;
    /// <summary>
    /// player reference
    /// </summary>
    private PlayerMovementScript player;
    #endregion

    #region methods
    void Start()
    {
        // get component of avatar setup 
        setup = gameObject.GetComponent<avatarsetUp>();
        // get poton view component
        PV = GetComponent<PhotonView>();
        // get player reference
        player = GetComponent<PlayerMovementScript>();
    }

    /// <summary>
    ///  if player enters trigger then function is called
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {

            PV.RPC("RespawnRPC", RpcTarget.All); // function is sent to all players over the network
   
        }
    }

    /// <summary>
    /// function that will be called to all players
    /// </summary>
    [PunRPC]
    public void RespawnRPC()
    {
     
        bool crateHeld = player.holding; // crateheld bool equals holding bool
        if (crateHeld == true) // if crate held is true
        {
            player.currentCrate.SetActive(false); // set inactive
            Destroy(player.currentCrate); // destroy crate
            crateHeld = false; // crateheld bool is set to false
            player.holding = crateHeld; // holding  bool equals crate held
            player.currentCrate = null;  // curren crate equals null
            
        }
        
      
    }
    #endregion
}
