using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CratePosition : MonoBehaviour
{
    #region fields
    /// <summary>
    /// setting held bool to true
    /// </summary>
    public bool held = true;
    #endregion
    #region method
    public void Start()
    {
        
        if(held == true)
        {
            // moves gameobject to local position
            transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        
        
    }

    #endregion


}
