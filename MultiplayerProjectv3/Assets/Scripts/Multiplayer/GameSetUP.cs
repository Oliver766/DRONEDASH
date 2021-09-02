using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameSetUP : MonoBehaviourPunCallbacks
{
    #region fields
    /// <summary>
    /// creating a singleton for this script
    /// </summary>
    public static GameSetUP GS;
    /// <summary>
    /// array of spawn points
    /// </summary>
    public Transform[] spawnPoints;
    /// <summary>
    /// player reference
    /// </summary>
    PhotonView pv;


    public bool loading;

    #endregion

    #region methods
    /// <summary>
    /// intialise singleton for game set up
    /// </summary>
    private void OnEnable()
    {
        if (GameSetUP.GS == null)
        {
            GameSetUP.GS = this;
        }
    }

    public void Start()
    {
       
        pv = GetComponent<PhotonView>(); // get component
    }

    /// <summary>
    /// disconnect player from game
    /// </summary>
    public void disconnectPlayer()
    {

        Destroy(photonRoom.room.gameObject); // destroy photonroom game object
        StartCoroutine(DsiconnectAndLoad()); // start couroutine
      


    }



    /// <summary>
    /// coroutine that will load menu scene
    /// </summary>
    /// <returns></returns>
    public IEnumerator DsiconnectAndLoad()
    {
       
        PhotonNetwork.Disconnect(); // disconnect player
       
        //while(PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom) // while photonnetwork is in room
            
            yield return null;
        SceneManager.LoadScene(Multiplayersettings.multiplayersettings.menuScene); // load scene
        





    }

    #endregion
}







