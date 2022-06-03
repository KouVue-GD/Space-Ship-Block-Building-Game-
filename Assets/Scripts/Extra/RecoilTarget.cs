using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilTarget : MonoBehaviour
{
    //It recoils the target

    //recoils backwards
    void Recoil(Rigidbody target, float force){

    }

    //recoils backwards
    void Recoil(Rigidbody2D target, float force){
        target.AddForce(Vector2.left * force);
    }

    //recoils into target direction
    void Recoil(Rigidbody2D target, float force, Vector3 direction){
        target.AddForce(direction * force);
    }
}
