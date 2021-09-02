
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTest : MonoBehaviour
{


    #region fields
    /// <summary>
    /// target
    /// </summary>
    public Transform target;
    /// <summary>
    /// smooth speed
    /// </summary>
    public float smoothSpeed = 0.125f;
    /// <summary>
    /// offset
    /// </summary>
    public Vector3 offset;

    #endregion



    #region methods
    /// <summary>
    /// updates  movement of camera to the position it's set too
    /// </summary>
    void LateUpdate()
    {
        // desired poistion
        Vector3 desiredPosition = target.position + offset;
        // smooth position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // transform position
        transform.position = smoothedPosition;
        // look at target
        transform.LookAt(target);


    }
    #endregion
}
