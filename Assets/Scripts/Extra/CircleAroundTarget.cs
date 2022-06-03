using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAroundTarget : MonoBehaviour
{
    public float speed;
    public float radius;
    public GameObject target;


    // Update is called once per frame
    void Update()
    {
        //creates a number that moves sequentially
        float angle = Time.fixedTime * speed;

        //Sets a point on the circle based on a number
        transform.position = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;

        //target is the offset position. It offsets from 0,0,0
        transform.position += target.transform.position;
        /*
        //offset code is...

        public Vector3 offset;
        transfrom.position += offset;

        //instead of 
        //transform.position += target.transform.position;
        */
    }
}
