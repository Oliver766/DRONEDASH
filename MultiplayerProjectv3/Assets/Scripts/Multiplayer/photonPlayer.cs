using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class photonPlayer : MonoBehaviour
{
    #region fields
    /// <summary>
    /// reference to photon view
    /// </summary>
    private PhotonView PV;
    /// <summary>
    /// game object for player avatar
    /// </summary>
    public GameObject myAvatar;
    #endregion

    #region methods
    void Start()
    {
        // gets photon view component at start
        PV = GetComponent<PhotonView>();
        // randomly chooses spawn point
        int spawnPicker = Random.Range(0, GameSetUP.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
            // instantiates photon prefab over the network
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "playerAvatar"), GameSetUP.GS.spawnPoints[spawnPicker].position, GameSetUP.GS.spawnPoints[spawnPicker].rotation, 0);
        }
    }
    #endregion
}


