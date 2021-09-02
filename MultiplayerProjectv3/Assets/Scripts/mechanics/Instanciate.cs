using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanciate : MonoBehaviour
{
    #region fields
    /// <summary>
    ///  gameobject spawn objects
    /// </summary>
    public GameObject[] spawnee;
    /// <summary>
    /// stopspawning bool set to false
    /// </summary>
    public bool stopSpawning = false;
    /// <summary>
    /// spawn time
    /// </summary>
    public float spawnTime;
    /// <summary>
    /// spawn delay
    /// </summary>
    public float spawnDelay;
    /// <summary>
    /// speed
    /// </summary>
    public float speed;
    /// <summary>
    /// target
    /// </summary>
    public GameObject target;
    /// <summary>
    /// max spawn delay
    /// </summary>
    public float maxSpawnDelay;
    /// <summary>
    /// min spawn delay
    /// </summary>
    public float minSpawnDelay;
    #endregion



    #region methods
    void Start()
    {
        // invoke repeating  method
        InvokeRepeating("SpawnObject", spawnTime, Random.Range(maxSpawnDelay,maxSpawnDelay));
 
    }

    public void SpawnObject()
    {
        int objectIndex = Random.Range(0, spawnee.Length);
       // instantiate object
        GameObject SpawnObject =  Instantiate(spawnee[objectIndex], transform.position, Quaternion.identity);
        Debug.Log(transform.position);
        // target direction of object
        Vector3 targetDirection = target.transform.position - transform.position;

        // move object towards oject
        SpawnObject.GetComponent<Rigidbody>().velocity = targetDirection.normalized * speed;

       

        //Debug.Log(targetDirection);
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }

    }
    #endregion

}

