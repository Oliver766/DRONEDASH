using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonLobbyCustomLobby : MonoBehaviourPunCallbacks
{
    #region fields
    /// <summary>
    /// creates reference to lobby
    /// </summary>
    public static PhotonLobbyCustomLobby lobby;
    /// <summary>
    /// room name 
    /// </summary>
    public string roomName;
    /// <summary>
    /// room size
    /// </summary>
    public int roomSize;
    /// <summary>
    /// room listing prefab
    /// </summary>
    public GameObject roomListingPrefab;
    /// <summary>
    /// room panel prefab
    /// </summary>
    public Transform roomsPanel;
    /// <summary>
    /// error text
    /// </summary>
    [SerializeField] Text errorText;
    /// <summary>
    /// room listing prefab
    /// </summary>
    public List<RoomInfo> roomlistings;
    #endregion


    #region methods
    private void Awake()
    {
        // lobby equals this
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Master");
        // player is connecting using setting
        PhotonNetwork.ConnectUsingSettings();
        // the room listing list is initialised
        roomlistings = new List<RoomInfo>();
     

    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        // player has joined lobby
        PhotonNetwork.JoinLobby(); 
        // the scenes have automatically synced
        PhotonNetwork.AutomaticallySyncScene = true;
        // nickname is equal to photon net work  nickname
        PhotonNetwork.NickName = PhotonNetwork.NickName;
    }


    /// <summary>
    /// override function on the room listing being updated when new room is created
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        int tempIndex;
        foreach (RoomInfo room in roomList)
        {
            if (roomlistings != null)
            {
                tempIndex = roomlistings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }
        
            if (tempIndex != -1)
            {
                roomlistings.RemoveAt(tempIndex);
                Destroy(roomsPanel.GetChild(tempIndex).gameObject);
                Debug.Log("Deleted");

            }
            else
            {
                roomlistings.Add(room);
                ListRoom(room);

            }



        }

    }



    /// <summary>
    /// instantiate a new room prefab with curren rooms listed on
    /// </summary>
    /// <param name="room"></param>
    void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.roomName = room.Name;
            tempButton.roomSize = room.MaxPlayers;
            tempButton.setRoom();
        }
    }

    /// <summary>
    /// returns rooms by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }
 
    /// <summary>
    /// removes room listing prefab
    /// </summary>
     void RemoveRoomListing()
    {
        int i = 0;
        while(roomsPanel.childCount !=0)
        {
            Destroy(roomsPanel.GetChild(1).gameObject);
            i++;
        }
    }
    /// <summary>
    /// creates new room
    /// </summary>
   public void createRoom()
   {
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte) roomSize};
        PhotonNetwork.CreateRoom(roomName,roomOps);
   }
    /// <summary>
    /// override function for joining room
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("in room");
    }

    /// <summary>
    /// override runction if creating room fails
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        Debug.LogError("Room Creation Failed: " + message);
        MenuManager.Instance.OpenMenu("error");
    }
   
    /// <summary>
    /// updated room list prefab
    /// </summary>
    /// <param name="nameIn"></param>
    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn;
    }
    /// <summary>
    /// updating size of room
    /// </summary>
    /// <param name="sizeIn"></param>
    public void OnRoomSizeChanged(string sizeIn)
    {
        roomSize = int.Parse(sizeIn);
    }
    /// <summary>
    /// joining lobby
    /// </summary>
    public void JoinLobbyOnClick()
    {
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("Joined game");
        }
    }

  

    /// <summary>
    ///  on application quit function
    /// </summary>
    public void onApplicationQuit()
    {
        onApplicationQuit();
    }
    #endregion

}
