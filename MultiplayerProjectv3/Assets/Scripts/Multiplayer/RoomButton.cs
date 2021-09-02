using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomButton : MonoBehaviour
{
    #region fields
    /// <summary>
    /// name text
    /// </summary>
    public Text nameText;
    /// <summary>
    /// size text
    /// </summary>
    public Text sizeText;
    /// <summary>
    /// room name
    /// </summary>
    public string roomName;
    /// <summary>
    /// room size
    /// </summary>
    public int roomSize;
    #endregion
    #region methods
    /// <summary>
    /// set room for room prefab
    /// </summary>
    public void setRoom()
    {
        nameText.text = roomName;
        sizeText.text = roomSize.ToString();
    }

    /// <summary>
    /// Joining room by name
    /// </summary>
    public void joinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    #endregion

}
