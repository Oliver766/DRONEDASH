using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class photonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    #region fields
    //room info
    /// <summary>
    /// creates singleton for this script
    /// </summary>
    public static photonRoom room;
    /// <summary>
    /// gets reference to photon view 
    /// </summary>
    private PhotonView PV;
    /// <summary>
    /// checks if game is loaded
    /// </summary>
    public bool isGameLoaded;
    /// <summary>
    /// gets current game scene for index
    /// </summary>
    public int currentScene;
    /// <summary>
    /// creates array of players 
    /// </summary>
    Player[] photonPlayers;
    /// <summary>
    /// shows players in room
    /// </summary>
    public int playersInRoom;
    /// <summary>
    /// shows my number in room
    /// </summary>
    public int myNumberInRoom;
    /// <summary>
    /// shows players in game
    /// </summary>
    public int playerInGame;
    /// <summary>
    /// checks if ready to count
    /// </summary>
    private bool readyToCount;
    /// <summary>
    /// checks if game is ready to start
    /// </summary>
    private bool readyToStart;
    /// <summary>
    /// starting time
    /// </summary>
    public float startingTime;
    /// <summary>
    /// less than max amount of players
    /// </summary>
    private float lessThanMaxPlayer;
    /// <summary>
    ///  at max  amount of plauers
    /// </summary>
    private float atmaxPlayer;
    /// <summary>
    /// time to start
    /// </summary>
    private float timeToStart;
    /// <summary>
    /// lobby game object
    /// </summary>
    public GameObject lobbyGp;
    /// <summary>
    /// room game object
    /// </summary>
    public GameObject roomGo;
    /// <summary>
    /// player panel prefab
    /// </summary>
    public Transform playersPanel;
    /// <summary>
    /// player listing prefab
    /// </summary>
    public GameObject playerListingPrefab;
    /// <summary>
    /// start button
    /// </summary>
    public GameObject startButton;
    #endregion

    #region create instance
    /// <summary>
    /// intialise singleton
    /// </summary>
    private void Awake()
    {
        if (photonRoom.room == null)
        {
            photonRoom.room = this;

        }
        else
        {
            if (photonRoom.room != this)
            {
                Destroy(photonRoom.room.gameObject);
                


            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region onenabled
    /// <summary>
    /// on enable function for scene loading
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading; // load scene in back geound


    }
    #endregion

    #region ondisable
    /// <summary>
    /// on disable function foor scene loading
    /// </summary>
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading; // unload scene
      
    }
    #endregion

    #region update and start

    void Start()
    {
        //get photon view component at start
        PV = GetComponent<PhotonView>();
        //ready tp count is set to false
        readyToCount = false;
        //ready to start set to false
        readyToStart = false;
        //less than max player is equal to starting time
        lessThanMaxPlayer = startingTime;
        //max players equal to 4
        atmaxPlayer = 6;
        //time to start is equal to starting time
        timeToStart = startingTime;
    }


    void Update()
    {
        //checks how many players are in room and check if required to restart timer again
        if (Multiplayersettings.multiplayersettings.delaystart)
        {
            if (playersInRoom == 1)
            {
                RestartTimer(); // reset timer
            }
            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atmaxPlayer = Time.deltaTime; // timer
                    lessThanMaxPlayer = atmaxPlayer; // less max player equals max player
                    timeToStart = atmaxPlayer; // time to start equals at max player
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayer -= Time.deltaTime; // being timer
                    timeToStart = lessThanMaxPlayer; // checks timer
                }
                Debug.Log("time left" + timeToStart);
                if (timeToStart <= 0)
                {
                    startGame(); // start game
                }
            }
        }
    }
    #endregion

    #region restarttimer
    /// <summary>
    /// updates starting variables and restart timer 
    /// </summary>
    public void RestartTimer()
    {
        lessThanMaxPlayer = startingTime; // checks start time
        timeToStart = startingTime; // chekcs time to start with starting time
        atmaxPlayer = 6; // max playerts
        readyToCount = false; // bool  set to false
        readyToStart = false;// bool  set to false
    }
    #endregion


    #region on finished loading all scenes
    /// <summary>
    /// makes current scene the build index and will create players in that scene.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == Multiplayersettings.multiplayersettings.multiplayerScene) //  checks this is current scene
        {
            isGameLoaded = true; // boot is true
            if (Multiplayersettings.multiplayersettings.delaystart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient); // run function
            }
            else
            {
                RPC_CreatePlayer(); // run function
            }
        }
    }



    #endregion
    #region start game function
    /// <summary>
    /// loads game level. will be different for each level and the other two functions are in game manager
    /// </summary>
    public void startGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient) // if it isn't master client
            return;
        if (Multiplayersettings.multiplayersettings.delaystart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; // current room is not open
        }
        PhotonNetwork.LoadLevel(Multiplayersettings.multiplayersettings.multiplayerScene); // load scene
    }
    #endregion
    #region on joined room
    /// <summary>
    /// override function for  on joined room
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("in room");
        lobbyGp.SetActive(false); // game object set false
        roomGo.SetActive(true);// game object set false
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);// game object set true
        }
        else
        {
            startButton.SetActive(false);// game object set false
        }

        ClearPlayerListings(); //  clear player listing function
        listPlayers(); // list players

        photonPlayers = PhotonNetwork.PlayerList; // players go into list
        playersInRoom = photonPlayers.Length; // players in room go into list
        myNumberInRoom = playersInRoom; // number of players in room is counted

        if (Multiplayersettings.multiplayersettings.delaystart)
        {
            Debug.Log("Display players in room of max players possible(" + playersInRoom + ":" + Multiplayersettings.multiplayersettings.maxPlayers + "(");
            if (playersInRoom > 1) // checks whos in room
            {
                readyToCount = true; // bool set to true
            }
            if (playersInRoom == Multiplayersettings.multiplayersettings.maxPlayers) // checks player max
            {
                readyToStart = true; // bool is true
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = true; // room is open
            }

        }
    }
    #endregion on joined room
    #region clear player listings

    /// <summary>
    /// clearing player list function
    /// </summary>
    void ClearPlayerListings()
    {
        for (int i = playersPanel.childCount - 1; i >= 0; i--) // checks list
        {
            Destroy(playersPanel.GetChild(i).gameObject); // deletes game object
        }
    }
    #endregion
    #region list players
    /// <summary>
    /// list of players fucntion that will be displayed on player list prefab
    /// </summary>
    void listPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (Player player in PhotonNetwork.PlayerList) // checks players
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel); // instantiate panel
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>(); // sets transform
                tempText.text = player.NickName; // sets text
            }



        }
    }
    #endregion
    #region on player entered room
    /// <summary>
    /// override function for players that enter room
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("new player joined" + PhotonNetwork.NickName.ToString());
        ClearPlayerListings(); // run function
        listPlayers(); // run fucntion
        photonPlayers = PhotonNetwork.PlayerList; // gets player list
        playersInRoom++; // adds players
        if (Multiplayersettings.multiplayersettings.delaystart)
        {
            Debug.Log("displayer players in room outr of max players possible(" + playersInRoom + ":" + Multiplayersettings.multiplayersettings.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true; // bool is true
            }
            if (playersInRoom == Multiplayersettings.multiplayersettings.maxPlayers) // checks max players
            {
                readyToStart = true; // set to true
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false; // room is not open
            }
        }
    }
    #endregion
    #region loaded game

    /// <summary>
    /// loaded scene function that will be sent to all players in game
    /// </summary>
    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length) // checls player list
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All); // run function
        }
    }
    #endregion
    #region create player
    /// <summary>
    /// player will be created and sent to all players in game
    /// </summary>
    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "photonnetworkplayer"), transform.position, transform.rotation, 0); // creates player
    }
    #endregion
    #region on player left room
    /// <summary>
    /// override function for players that have left room
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("You have left room");
        playersInRoom--; // players get removed from room

    }
    #endregion
    #region on left room
    /// <summary>
    /// override function for leaving room
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(Multiplayersettings.multiplayersettings.menuScene); // overide to leave to menu scene
        base.OnLeftRoom();
    }
    #endregion


}
