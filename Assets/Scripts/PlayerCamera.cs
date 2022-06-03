using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    
    //TODO: plus smooth follow

    Vector3 camPos;
    float timer = 0;
    float timeLimit = 0;
    float force = 0;
    //random shaking has to be in late update
    public void ScreenShake(float force, float timeLimit){
        timer = 0f;
        this.force = force;
        this.timeLimit = timeLimit;
    }
    void ScreenShaker(){
        timer += Time.deltaTime;
        if(timer < timeLimit){
            camPos = gameObject.transform.position;
            camPos.x += Random.Range(-force, force);
            camPos.y += Random.Range(-force, force);
            gameObject.transform.position = camPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offset;
    }

    void LateUpdate()
    {
        ScreenShaker();
    }
}
