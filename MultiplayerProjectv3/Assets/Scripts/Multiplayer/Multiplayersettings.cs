using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// script by Oliver Lancashire
public class Multiplayersettings : MonoBehaviour
{
    #region fields
    /// <summary>
    /// created singleton for this script
    /// </summary>
    public static Multiplayersettings multiplayersettings;
    /// <summary>
    /// checking if there is a delayed start
    /// </summary>
    public bool delaystart;
    /// <summary>
    /// maxplayers
    /// </summary>
    public int maxPlayers;
    /// <summary>
    /// scene that are checked for in buid index
    /// </summary>
    public int menuScene;
    public int multiplayerScene;

    #endregion
    #region method
    /// <summary>
    /// initialise singleton for this script and checks if this game oject alread exists in current game
    /// </summary>
    private void Awake()
    {
        if(Multiplayersettings.multiplayersettings == null)
        {
            Multiplayersettings.multiplayersettings = this;
        }
        else
        {
            if(Multiplayersettings.multiplayersettings != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }


    #endregion
}
