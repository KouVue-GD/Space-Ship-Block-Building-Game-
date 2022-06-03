using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*****************************************************************************************************
    Adapted from:
    https://answers.unity.com/questions/1227892/how-do-i-have-my-character-aim-where-my-mouse-curs.html
    Answer by Sethhalocat · Aug 10, 2016 at 10:42 PM
*******************************************************************************************************/
public class LookAtTarget2D : MonoBehaviour
{   
    public Vector3 offset;
    public Transform target;
    void LookAtTarget(Transform target){
        Vector3 dir = target.position - gameObject.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        gameObject.transform.eulerAngles += offset;
    }

    void Update()
    {
        LookAtTarget(target);
    }
}
