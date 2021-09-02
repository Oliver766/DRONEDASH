using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.IO;


#region Codes

/// <summary>
/// event codes used for timer function
/// </summary>
public enum EventCodes : byte
{
    endofGame,
    RefreshTimer
}
#endregion

#region enum
/// <summary>
/// game state list
/// </summary>
public enum GameState
{
    Waiting = 0,
    Starting = 1,
    Playing = 2,
    Ending = 3
}
#endregion
public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region Fields
    /// <summary>
    /// current match time
    /// </summary>
    private int currentMatchTime;
    /// <summary>
    /// getting coroutine for time
    /// </summary>
    private Coroutine timerCoroutine;
    /// <summary>
    /// Text for timer
    /// </summary>
    public Text ui_timer;
    /// <summary>
    /// match lenght
    /// </summary>
    public int matchLength = 180;
    /// <summary>
    /// photon view reference
    /// </summary>
    private PhotonView PV;
    /// <summary>
    /// bool to checked if game paused
    /// </summary>
    public static bool paused = false;
    /// <summary>
    /// checks if game is disconnecting
    /// </summary>
    public bool disconnecting;
    /// <summary>
    /// pause menu
    /// </summary>
    public GameObject pauseMenu;
    /// <summary>
    /// outcome panel
    /// </summary>
    public GameObject outcome1;
    /// <summary>
    /// reference to player controller
    /// </summary>
    public PlayerMovementScript pc;
    /// <summary>
    /// all references to player outcome text
    /// </summary>
    [SerializeField] public Text outcomeText1;
    [SerializeField] public Text playerscoreout1;
    [SerializeField] public Text playerscoreout2;
    [SerializeField] public Text playerscoreout3;
    [SerializeField] public Text playerscoreout4;
    [SerializeField] public Text playerText1;
    [SerializeField] public Text playerText2;
    [SerializeField] public Text playerText3;
    [SerializeField] public Text playerText4;

    /// <summary>
    /// all references to player scores
    /// </summary>
    public Player1Score score1;
    //public Player2Score score2;
    //public Player3Score score3;
    //public Player4Score score4;

    public GameObject scoreManager;
    // note - will be upated
    /// <summary>
    /// button to start next level
    /// </summary>
    public GameObject nextLevel;

    public static GameManager manager;

    // game state starting as waiting
    private GameState state = GameState.Waiting;


    #endregion


    #region methods





    public void Start()
    {
        // timer is intialised at start
        InitializeTimer();
        // photon view component is accessed at start
        PV = GetComponentInParent<PhotonView>();
        // player controller is accessed at start
        pc = gameObject.GetComponent<PlayerMovementScript>();

    }
    /// <summary>
    /// toggle pause button
    /// </summary>
    public void TogglePause()
    {


        if (disconnecting) return;
        // paused is equal to not paused
        paused = !paused;
        // pause menu set active
        pauseMenu.SetActive(paused);
        // curser is set to active
        Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = paused;
        Cursor.visible = true;

        if (paused == true)
        {
            // player movement is stopped
            pc.speed = 0;


        }
        else if (paused == false)
        {
            pc.speed = 6;

        }



    }




    public void Update()
    {
        // game state will be returned if it equals ending
        if (state == GameState.Ending)
        {
            return;
        }
    }

    /// <summary>
    /// refreshes timer and displays it on screen
    /// </summary>
    public void RefreshTimerUI()
    {
        string minutes = (currentMatchTime / 60).ToString("00"); // display minutes
        string seconds = (currentMatchTime % 60).ToString("00"); // display seconds
        ui_timer.text = $"{minutes}:{seconds}"; // display text
    }
    /// <summary>
    /// creates  refresh timer event that will be sent over the network to all players
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code >= 200) return; // syncing timer

        EventCodes e = (EventCodes)photonEvent.Code; // sets event code
        object[] o = (object[])photonEvent.CustomData; // sets object array

        switch (e)
        {

            case EventCodes.RefreshTimer: // create refresh timer
                RefreshTimer_R(o);
                break;

        }
    }

    /// <summary>
    /// intializes timer for all players in game
    /// </summary>
    public void InitializeTimer()
    {
        currentMatchTime = matchLength; // sets timer
        RefreshTimerUI(); // refresh timer 

        if (PhotonNetwork.IsMasterClient)
        {
            timerCoroutine = StartCoroutine(Timer());
            Debug.Log(timerCoroutine);
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            timerCoroutine = StartCoroutine(Timer());
            Debug.Log(timerCoroutine);
        }
    }

    /// <summary>
    /// sets event that will be started if timer has ended.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);

        currentMatchTime -= 1; // timer goes down

        if (currentMatchTime <= 0)
        {
            timerCoroutine = null;
            Debug.Log(currentMatchTime);

            state = GameState.Ending; // states is ending

            // set timer to 0
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            currentMatchTime = 0;
            RefreshTimerUI(); // refresh timer


            if (PV.IsMine)
            {
                PV.RPC("RPC_EndState", RpcTarget.All); // run fucntion
                pc.speed = 0; // speed is 0
                if (PhotonNetwork.IsMasterClient)
                {
                    nextLevel.SetActive(true); // set active
                }
            }
            else if (!PV.IsMine)
            {
                PV.RPC("RPC_EndState", RpcTarget.All); // run function
                pc.speed = 0; // speed is 0
                if (PhotonNetwork.IsMasterClient)
                {
                    nextLevel.SetActive(true);
                }
            }

        }
        else
        {
            RefreshTimer_S(); // refresh timer
            timerCoroutine = StartCoroutine(Timer());
        }
    }


    /// <summary>
    /// sends current match time to all players in game and allows late joiners to recieve current mathc time
    /// </summary>
    public void RefreshTimer_S()
    {
        object[] package = new object[] { currentMatchTime }; // sets object

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.RefreshTimer,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true } // sets timer over the network and received by all
        );
    }
    /// <summary>
    /// gets current match time and refreshes timer ui
    /// </summary>
    /// <param name="data"></param>
    public void RefreshTimer_R(object[] data)
    {
        currentMatchTime = (int)data[0]; // updates curreny match timer
        RefreshTimerUI(); // refresj timer uI
    }

    /// <summary>
    /// function that will be shown to all players when game has finished
    /// </summary>
    [PunRPC]
    public void RPC_EndState()
    {
        ScoreManager scores = scoreManager.GetComponent<ScoreManager>(); // get component
        if (photonRoom.room.playersInRoom == 1)
        {
            outcome1.gameObject.SetActive(true); // set active
            outcomeText1.text = "Here are the scores!" + ":".ToString(); // display text
            playerscoreout1.text = scores.playerScores[0].ToString(); // display score
            playerText1.text = scores.playerNames[0].ToString(); // display name
            playerscoreout2.gameObject.SetActive(false);// set not active
            playerscoreout3.gameObject.SetActive(false);// set not active
            playerscoreout4.gameObject.SetActive(false);// set not active
            playerText2.gameObject.SetActive(false);// set not active
            playerText3.gameObject.SetActive(false);// set not active
            playerText4.gameObject.SetActive(false);// set not active

        }
        else if (photonRoom.room.playersInRoom == 2)
        {
            outcome1.gameObject.SetActive(true); // set active
            outcomeText1.text = "Here are the scores!" + ":";// display text
            playerscoreout1.text = scores.playerScores[0].ToString(); // display score
            playerscoreout2.text = scores.playerScores[1].ToString(); // display score
            playerText1.text = scores.playerNames[0].ToString(); // display name
            playerText2.text = scores.playerNames[1].ToString();// display name
            playerscoreout3.gameObject.SetActive(false);// set not active
            playerscoreout4.gameObject.SetActive(false);// set not active
            playerText3.gameObject.SetActive(false);// set not active
            playerText4.gameObject.SetActive(false);// set not active
           
        }
        else if (photonRoom.room.playersInRoom == 3)
        {
            outcome1.gameObject.SetActive(true);// set active
            outcomeText1.text = "Here are the scores!" + ":";// display text
            playerscoreout1.text = scores.playerScores[0].ToString();// display score
            playerscoreout2.text = scores.playerScores[1].ToString();// display score
            playerscoreout3.text = scores.playerScores[2].ToString();// display score
            playerText1.text = scores.playerNames[0].ToString();// display name
            playerText2.text = scores.playerNames[1].ToString();// display name
            playerText3.text = scores.playerNames[2].ToString();// display name
            playerscoreout4.gameObject.SetActive(false);// set not active
            playerText4.gameObject.SetActive(false);// set not active
        }
        else if (photonRoom.room.playersInRoom == 4)
        {
            outcome1.gameObject.SetActive(true);// set active
            outcomeText1.text = "Here are the scores!" + ":";// display text
            playerscoreout1.text = scores.playerScores[0].ToString(); // display score
            playerscoreout2.text = scores.playerScores[1].ToString(); // display score
            playerscoreout3.text = scores.playerScores[2].ToString();// display score
            playerscoreout4.text = scores.playerScores[3].ToString();// display score
            playerText1.text = scores.playerNames[0].ToString();// display name
            playerText2.text = scores.playerNames[1].ToString();// display name
            playerText3.text = scores.playerNames[2].ToString();// display name
            playerText4.text = scores.playerNames[3].ToString();// display name
            
        }


    }

    #endregion
}







































