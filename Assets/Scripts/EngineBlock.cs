using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EngineBlock : Block
{
    //value of speed from engine block
    [SerializeField]
    float baseSpeed;

    //current speed
    float speed;

   // [ReadOnlyAttribute]
    public float upSpeed;
   // [ReadOnlyAttribute]
    public float leftSpeed;
    //[ReadOnlyAttribute]
    public float rightSpeed;
    //[ReadOnlyAttribute]
    public float downSpeed;
    //[ReadOnlyAttribute]
    public float rotateLeftSpeed;
    //[ReadOnlyAttribute]
    public float rotateRightSpeed;

    //target has to be the center of mass relative to the core
    public void CalculateSpeed(GameObject target){
        //weight comes from block
        speed = baseSpeed - weight;

        #region Directional Speed
        //get the placement of the engine block
        Vector3 normalizedDistance = (target.transform.localPosition - target.transform.InverseTransformPoint(gameObject.transform.position)).normalized;

        //left pushes to right
        if( normalizedDistance.x < 0 ){
            rightSpeed = Mathf.Abs(normalizedDistance.x * speed);
        }

        //right pushes to left
        if( normalizedDistance.x > 0 ){
            leftSpeed = Mathf.Abs(normalizedDistance.x * speed);
        }

        //down pushes up
        if( normalizedDistance.y > 0 ){
            upSpeed = Mathf.Abs(normalizedDistance.y * speed);
        }

        //up pushes down
        if( normalizedDistance.y < 0 ){
            downSpeed = Mathf.Abs(normalizedDistance.y * speed);
        }

        #endregion

        #region Rotation Speed
        rotateLeftSpeed = 0;
        rotateRightSpeed = 0;

        Vector2 normalizedDistanceDiagonal = Normalize((Vector2)(target.transform.localPosition - target.transform.InverseTransformPoint(gameObject.transform.position)), .5f);      

        //check to see which position the engine is in and give force accordingly

        //bottom left
        if( normalizedDistance.y < 0 && normalizedDistance.x < 0){
            //rotate right
            rotateRightSpeed += ((Mathf.Abs(normalizedDistanceDiagonal.x) + Mathf.Abs(normalizedDistanceDiagonal.y))/2f)*speed;
        }

        //bottom right
        if( normalizedDistance.y < 0 && normalizedDistance.x > 0){
            //rotate left
            rotateLeftSpeed += ((Mathf.Abs(normalizedDistanceDiagonal.x) + Mathf.Abs(normalizedDistanceDiagonal.y))/2f)*speed;
            
        }

        //top left
        if( normalizedDistance.y > 0 && normalizedDistance.x < 0){
            //rotate left
            rotateLeftSpeed += ((Mathf.Abs(normalizedDistanceDiagonal.x) + Mathf.Abs(normalizedDistanceDiagonal.y))/2f)*speed;
        }

        //top right
        if( normalizedDistance.y > 0 && normalizedDistance.x > 0){
            //rotate right
            rotateRightSpeed += ((Mathf.Abs(normalizedDistanceDiagonal.x) + Mathf.Abs(normalizedDistanceDiagonal.y))/2f)*speed;
        }
        #endregion
    
    }

    Vector2 Normalize(Vector2 pVector, float percent){ //percent is 0 to 1
        float total = Mathf.Abs(pVector.x) + Mathf.Abs(pVector.y);
        total *= percent;
        Vector2 normalizedVector = new Vector2( pVector.x/total,
                                                pVector.y/total);
        if(normalizedVector.x > 1){
            float leftOverValue = normalizedVector.x - 1;
            normalizedVector = new Vector2( normalizedVector.x - leftOverValue,
                                            normalizedVector.y);
        }

        if(normalizedVector.y > 1){
            float leftOverValue = normalizedVector.y - 1;
            normalizedVector = new Vector2( normalizedVector.x,
                                            normalizedVector.y - leftOverValue);
        }

        if(normalizedVector.x < -1){
            float leftOverValue = normalizedVector.x + 1;
            normalizedVector = new Vector2( normalizedVector.x + leftOverValue,
                                            normalizedVector.y);
        }

        if(normalizedVector.y < -1){
            float leftOverValue = normalizedVector.y + 1;
            normalizedVector = new Vector2( normalizedVector.x,
                                            normalizedVector.y + leftOverValue);
        }

        return normalizedVector;
    }

    public override String GetText(){
        description = "";
        description += "Name: "+ name + "\n";
        Health health = GetComponent<Health>();
        if(health != null){
            description += "Health: " + health.GetHealth() + "/" + health.GetMaxHealth() + "\n"; 
        }
        description += "Speed: "+ speed + "\n";
        description += "Weight: "+ weight;
        return description;
    }

}
