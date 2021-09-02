using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovementScript : MonoBehaviourPunCallbacks
{
    #region fields
    /// <summary>
    /// score variables
    /// </summary>
    public float score;
    /// <summary>
    /// crates stolen
    /// </summary>
    public float cratesStolen = 0;
    /// <summary>
    /// creates game object
    /// </summary>
    public GameObject cratesobject;
    /// <summary>
    /// current crate
    /// </summary>
    public GameObject currentCrate;
    /// <summary>
    /// new  crate gameobject
    /// </summary>
    public GameObject newCrate;
    /// <summary>
    /// other gamw object
    /// </summary>2
    public GameObject secondarycrate;
    /// <summary>
    /// bool holding set to false
    /// </summary>
    public bool holding = false;
    /// <summary>
    /// collider reference
    /// </summary>
    Collider myCollider;
    /// <summary>
    /// photon view reference
    /// </summary>
    PhotonView pv;
    /// <summary>
    /// controller
    /// </summary>
    public CharacterController controller;
    /// <summary>
    /// speeds
    /// </summary>
    public float speed = 11f;
    /// <summary>
    /// jump speed
    /// </summary>
    public float gravity = 7.0F;
    /// <summary>
    /// rotationspeed
    /// </summary>
    public float rotateSpeed;
    /// <summary>
    /// movedirection
    /// </summary>
    private Vector3 moveDirection = Vector3.zero;
    /// <summary>
    /// target destination
    /// </summary>
    private Vector3 targetDestination;


    #endregion


    void Start()
    {
        // gets collider component
        myCollider = GetComponent<Collider>();
        // gets photon vire component
        pv = GetComponent<PhotonView>();
     


    }

    public  void Update()
    {
        // if photon view is mine
        if (pv.IsMine)
        {
            // sysncs name function
            pv.RPC("SyncNamesRPC", RpcTarget.All, PhotonNetwork.NickName);

            // press esape to pause game
            bool pause = Input.GetKeyDown(KeyCode.Escape);

                if (pause)
                {
                    // toggle pause
                    GameObject.Find("GameManager").GetComponent<GameManager>().TogglePause();
                }

                if (GameManager.paused)
                {
                    // character movement disable
                    speed = 0;


                }
                else
                {
                    // character can move
                    speed = 6;
                    // cursor is visible
                    Cursor.visible = true;

                }
                
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), -10.0f, Input.GetAxis("Vertical")); // creates  horizontal and vertical movemnent
            float x = Input.GetAxis("Horizontal"); // set a float for x axis
            float y = Input.GetAxis("Vertical"); // set a float for y axis
            Vector3 move = transform.right * x + transform.forward * y; // sets movement for right and forward
            // allows player moving using controller
            controller.Move(moveDirection * speed * Time.deltaTime);
            float translationV = Input.GetAxis("Vertical"); // sets a float for  vertical
            float translationH = Input.GetAxis("Horizontal"); // sets a float for horizontal
            Vector3 targetInput = new Vector3(x, 0, y); // creates vector for target input
            targetDestination = transform.position + targetInput; // sets target destination
            transform.LookAt(targetDestination); // sets look at for target destrination

        }

    }

    /// <summary>
    /// on trigger enter fall all tags related to this object
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CrateInstantiate")
        {
            if (holding == false)
            {
                Debug.Log("Nothing happens");
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            PlayerMovementScript playerMovement = other.GetComponent<PlayerMovementScript>(); // gets component
            bool crateheld = playerMovement.holding; // gets bool reference
            if (crateheld == false)
            {
                if (holding == true) // if holding is true
                {
                    playerMovement.OtherCrateAccessed(currentCrate); // gets current crate
                }

            }
            else if (crateheld == true) // if crate held is true
            {
                if (holding == false) // if holding is false
                {
                    currentCrate = (GameObject)Instantiate(cratesobject, this.transform); // instantiate crate
                    crateheld = false; // crate held is false
                    playerMovement.holding = crateheld; // holding bool equals crate held bool
                    playerMovement.currentCrate.SetActive(false); //  set current crate to inactive
                    Destroy(playerMovement.currentCrate); // destroy crate
                    cratesStolen += 1f; // cratesstolen adds score
                    holding = true; // hold equqls true

                }
            }


        }
        //Debug.Log("Hit player");


    }

    /// <summary>
    /// create access fucntion
    /// </summary>
    /// <param name="crateAccessed"></param>
    public void CrateAccessesed(GameObject crateAccessed)
    {
        currentCrate = crateAccessed; // current crate accessed  is equal to current crate
        

    }
    /// <summary>
    /// other crate access function
    /// </summary>
    /// <param name="crateAccessed"></param>
    public void OtherCrateAccessed(GameObject crateAccessed)
    {
        secondarycrate = crateAccessed; // other crate  = crate accessed
    }

    [PunRPC]
    void SyncNamesRPC(string nameIn)
    {

        ScoreManager.SM.playerNames[pv.OwnerActorNr - 1] = nameIn; // identified planers name
        ScoreManager.SM.playerNameTexts[pv.OwnerActorNr - 1].text = nameIn; // displays players name
        ScoreManager.SM.playerScores[pv.OwnerActorNr - 1] = score; // assigns players name to score


    }

}

    
    


