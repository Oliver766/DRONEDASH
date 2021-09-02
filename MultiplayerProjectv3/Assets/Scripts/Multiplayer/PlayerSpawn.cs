using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.IO;

public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    #region fields
    /// <summary>
    /// Game Object that marks player spawning
    /// </summary>
    [SerializeField] GameObject graphics;
    #endregion
    #region methods
    void Awake()
    {
        // set game object to false on start
        graphics.SetActive(false);
    }
    #endregion
}
