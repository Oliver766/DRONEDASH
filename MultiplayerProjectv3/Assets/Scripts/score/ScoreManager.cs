using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;




public class ScoreManager : MonoBehaviourPunCallbacks
{
 
   
    #region fields
    /// <summary>
    /// static game object reference
    /// </summary>
    public static ScoreManager SM;
    /// <summary>
    /// array of player names
    /// </summary>
    public string[] playerNames;
    /// <summary>
    /// array of playername text
    /// </summary>
    public Text[] playerNameTexts;
    /// <summary>
    /// array of player score
    /// </summary>
    public float[] playerScores;
    /// <summary>
    /// array of score total
    /// </summary>
    public float scoreTotal;
    /// <summary>
    /// array of playerscore text
    /// </summary>
    public Text[] playerScoreText;

    #endregion

    #region methods

    /// <summary>
    /// allows object not to be destroyed on load
    /// </summary>
    private void OnEnable()
    {
        if (ScoreManager.SM == null)
        {
            ScoreManager.SM = this;
        }
        else
        {
            if (ScoreManager.SM != this)
            {
                Destroy(ScoreManager.SM.gameObject);
                ScoreManager.SM = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
  

    void Update()
    {
        // update score function
        UpdateScores();
    }

    /// <summary>
    /// update score everytime
    /// </summary>
    public void UpdateScores()
    {

        float tempTotal = 0; // create a temporary total
        for (int i = 0; i < playerScores.Length; i++) // counts player score list
        {
            tempTotal += playerScores[i]; // temp total is added and equal too player score
        }

        if (tempTotal != scoreTotal) // checks if tem total is not equal to score total
        {
            scoreTotal = tempTotal; // score total equals temptotal
            for (int i = 0; i < playerScores.Length; i++) // count players score
            {
                playerScoreText[i].text = playerScores[i].ToString(); // output player score text equals player scores
            }
        }


    }

    #endregion
}









