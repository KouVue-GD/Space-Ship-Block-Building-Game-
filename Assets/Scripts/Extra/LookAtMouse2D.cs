using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************************************
    Adapted from:
    https://answers.unity.com/questions/1227892/how-do-i-have-my-character-aim-where-my-mouse-curs.html
    Answer by Sethhalocat · Aug 10, 2016 at 10:42 PM
*******************************************************************************************************/

public class LookAtMouse2D : MonoBehaviour
{
    public Vector3 offset;
    void LookAtMouse(){
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.eulerAngles += offset;
    }

    void Update(){
        LookAtMouse();
    }
}
