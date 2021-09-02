using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    #region fields
    /// <summary>
    /// menu name by string
    /// </summary>
    public string menuName;
	/// <summary>
	/// is menu open bool
	/// </summary>
	public bool open;
    #endregion
    #region methods
    /// <summary>
    /// function that will set open to true and set menu gameobject active
    /// </summary>
    public void Open()
	{
		open = true;
		gameObject.SetActive(true);
	}

	/// <summary>
	/// function that will see open to false and sset menu gameobject to not active
	/// </summary>
	public void Close()
	{
		open = false;
		gameObject.SetActive(false);
	}
    #endregion
}