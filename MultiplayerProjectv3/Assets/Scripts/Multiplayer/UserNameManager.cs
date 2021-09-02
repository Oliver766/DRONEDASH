using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UserNameManager : MonoBehaviourPunCallbacks
{
    #region fields
    /// <summary>
    ///  input field for username
    /// </summary>
    [SerializeField] public InputField inputName;
    /// <summary>
    /// continue buton
    /// </summary>
    [SerializeField] public Button setNameBtn;

    #endregion

    #region methods
    /// <summary>
    /// entering text function where button will become interactable if text is entered
    /// </summary>
    public void onEnterText()
    {
        if (inputName.text.Length > 2)
        {
            setNameBtn.interactable = true;
        }

    }

    /// <summary>
    /// set user name that will be displayed throughout the game
    /// </summary>
    public void OnClick_SetName()
    {
        PhotonNetwork.NickName = inputName.text;
    
        Debug.Log(inputName);
        Debug.Log(PhotonNetwork.NickName);
    }

    #endregion
}
