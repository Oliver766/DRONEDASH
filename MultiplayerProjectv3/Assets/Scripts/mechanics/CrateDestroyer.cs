using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDestroyer : MonoBehaviour
{
    #region variables
    /// <summary>
    /// game object that is going to be destroyed
    /// </summary>
    public GameObject CrateObject;
    #endregion

    /// <summary>
    /// game object enters trigger and desgtroys after entering
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dropoff")
        {
            PlayerMovementScript playerScript = CrateObject.transform.parent.GetComponent<PlayerMovementScript>(); //  get player script reference
            bool crateHeld = playerScript.holding; // bool equals player script bool
            if (crateHeld == true) // is crate held bool is set to true
            {
                CrateObject.SetActive(false); // crate set to false
                Destroy(CrateObject); // destroy game object
                crateHeld = false; // crate held set to false
                playerScript.holding = crateHeld; // holding bool equals crateheld bool
                CrateObject = null;  // crate is null


            }


        }

    }

    /// <summary>
    /// function for stealing object from other player
    /// </summary>
    public void OnPlayerSteal()
    {
        PlayerMovementScript playerScript = CrateObject.transform.parent.GetComponent<PlayerMovementScript>(); //  get player script reference
        bool crateHeld = playerScript.holding;// bool equals player script bool
        if (crateHeld == true) // is crate held bool is set to true
        {

            CrateObject.SetActive(false); // crate set to false
            Destroy(CrateObject); // destroy game object
            crateHeld = false;  // crate held set to false
            playerScript.holding = crateHeld; // holding bool equals crateheld bool

        }
    }
}
