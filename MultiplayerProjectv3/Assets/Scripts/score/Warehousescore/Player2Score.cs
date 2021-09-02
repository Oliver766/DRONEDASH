using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Player2Score : MonoBehaviourPun
{
    #region fields
    /// <summary>
    /// adds float for player score
    /// </summary>
    public float player2Score;
    /// <summary>
    /// score manager reference
    /// </summary>
    public ScoreManager scoreManager;
    #endregion
  
    #region methods
    void Start()
    {
        // player score set to 0 on start
        player2Score = 0;

    }


    /// <summary>
    /// player score is added by one each time a object collided with trigger ,script is attached to
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            PlayerMovementScript playerScript = other.transform.parent.GetComponent<PlayerMovementScript>(); //  gets component
            playerScript.score++; // add score
            playerScript.score += playerScript.cratesStolen; // adds crates stolen score
            playerScript.cratesStolen = 0; // score set to 0


        }
    }
    #endregion
}
