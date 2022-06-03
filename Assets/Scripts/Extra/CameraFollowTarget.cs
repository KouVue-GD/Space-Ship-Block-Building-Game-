using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    
    //TODO: plus smooth follow

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offset;
    }
}
