using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Player1Score : MonoBehaviourPun
{
    #region fields
    /// <summary>
    /// adds float for player score
    /// </summary>
    public float player1Score;
    /// <summary>
    /// score reference
    /// </summary>
    public GameObject scoreRef;
    #endregion
    #region methods
   
    void Start()
    {
        // player score set to 0 on start
        player1Score = 0;

    }
    /// <summary>
    /// player score is added by one each time a object collided with trigger ,script is attached to
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
        
            PlayerMovementScript playerScript = other.transform.parent.GetComponent<PlayerMovementScript>(); // gets player movement component
            playerScript.score++; // add score
            playerScript.score += playerScript.cratesStolen; //  adds crates stolen score
            playerScript.cratesStolen = 0;  // crates stolen score is set to 0
        
        }
    }
    #endregion
}
