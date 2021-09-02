using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// script by Oliver Lancashire - Subject to change by other devs

public class Player3Score : MonoBehaviourPun
{
    #region fields
    /// <summary>
    /// adds float for player score
    /// </summary>
    public float player3Score;
    /// <summary>
    /// score mamager reference
    /// </summary>
    public ScoreManager scoreManager;
    #endregion

    #region methods
    void Start()
    {
        // player score set to 0 on start
        player3Score = 0;

    }

    /// <summary>
    /// player score is added by one each time a object collided with trigger ,script is attached to
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
       
            PlayerMovementScript playerScript = other.transform.parent.GetComponent<PlayerMovementScript>(); // gets component
            playerScript.score++; // adds score
            playerScript.score += playerScript.cratesStolen; // adds crate stollen score
            playerScript.cratesStolen = 0; //  score set to 00
         
        }
    }
    #endregion
}
