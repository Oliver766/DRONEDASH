using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Cinemachine;

public class avatarsetUp : MonoBehaviour
{
    #region fields
    /// <summary>
    /// reference to photon view
    /// </summary>
    private PhotonView PV;
    /// <summary>
    /// character value int
    /// </summary>
    public int characterValue;
    /// <summary>
    /// game object of selected character
    /// </summary>
    public GameObject myCharacter;


    #endregion

    #region methods
    void Start()
    {
        // gets photon view component at start
        PV = GetComponent<PhotonView>();
        // creates randome spawn point fot player
        int spawnPicker = Random.Range(0, GameSetUP.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
        
            // instatiate player
            myCharacter = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "Player"), GameSetUP.GS.spawnPoints[spawnPicker].position, GameSetUP.GS.spawnPoints[spawnPicker].rotation, 0);
            // main camera attaching 
            Camera.main.GetComponent<CameraFollowTest>().target = myCharacter.transform;

        }
    }


}

#endregion


