using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region fields
	/// <summary>
	/// creates instance of menu class
	/// </summary>
    public static MenuManager Instance;
	/// <summary>
	/// creates array of menus
	/// </summary>
	[SerializeField] Menu[] menus;

	
    #endregion

    #region methods

    void Awake()
	{
		// instances equals this
		Instance = this;
	}



	public void Start()
	{
		// sets paused bool to false
		GameManager.paused = false;
		// cursor is not locked
		Cursor.lockState = CursorLockMode.None;
		//cursor is visible
		Cursor.visible = true;

	}
    #endregion

    #region openmenu(string)
    /// <summary>
    /// open menu function using strings
    /// </summary>
    /// <param name="menuName"></param>
    public void OpenMenu(string menuName)
	{
		for (int i = 0; i < menus.Length; i++)
		{
			if (menus[i].menuName == menuName)
			{
				menus[i].Open();
			}
			else if (menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
	}
	#endregion

	#region openmenu(object)
	/// <summary>
	/// open menus with the game object
	/// </summary>
	/// <param name="menu"></param>
	public void OpenMenu(Menu menu)
	{
		for (int i = 0; i < menus.Length; i++)
		{
			if (menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
		menu.Open();
	}
	#endregion

	#region closemenu(object)
	/// <summary>
	/// close menu using game object
	/// </summary>
	/// <param name="menu"></param>
	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}
    #endregion


	

	
}

