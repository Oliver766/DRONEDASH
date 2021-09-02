using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    #region fields
    /// <summary>
    /// isDestroyed bool set to false at start
    /// </summary>
    public bool isDestroyed = false;
    /// <summary>
    ///  
    /// </summary>
    public GameObject Object;
    #endregion
   
    
    void LateUpdate()
    {
        StartCoroutine(Destroy()); // start coroutine
    }

    /// <summary>
    /// coroutine for destroying object
    /// </summary>
    /// <returns></returns>
    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(15);
        Destroy(Object);
        isDestroyed = true;
        yield return new WaitForSeconds(15);
    }

}
