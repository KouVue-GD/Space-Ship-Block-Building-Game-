using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//contains a engine, a reactor, and manages gameobjects connected to it
public class CoreBlock : Block
{
    public float speed;
    [HideInInspector]
    public Rigidbody2D rb2d;

    [HideInInspector]
    public EngineBlock[] listOfEngines;
    
    [HideInInspector]
    public GunBlock[] listOfGunBlocks; //later this will be firing piece //TODO: firing piece

    //usercontrolled gunblocks
    // GunBlock[] forward;

    public void CollectConnectedBlocks(){
        listOfEngines = gameObject.transform.GetComponentsInChildren<EngineBlock>();
        listOfGunBlocks = gameObject.transform.GetComponentsInChildren<GunBlock>();

        CalculateSpeed();
    }

    #region Guns
    public void FireAllGuns(){
        if(listOfGunBlocks != null){
            foreach (var GunBlock in listOfGunBlocks)
            {
                if(GunBlock.IsSafeToFire() == true){
                    GunBlock.FireGuns();
                }
            }
        }
    }

    public void ResetAllGuns(){
        if(listOfGunBlocks != null){
            foreach (var GunBlock in listOfGunBlocks)
            {
                GunBlock.ResetLook();
            }
        }
    }

    #endregion
    
    #region  Movement
    float upSpeed;
    float leftSpeed;
    float rightSpeed;
    float downSpeed;
    float rotateLeftSpeed;
    float rotateRightSpeed;

    //TODO: Calculate Center of Mass based on mass of other objects
    public GameObject centerMass;

    //get all the engines that can move the direction towards its desired position 
    void CalculateSpeed(){   

        //TODO: The closer the engine is to the center of mass the more powerful?

        upSpeed = 0;
        leftSpeed = 0;
        rightSpeed = 0;
        downSpeed = 0;
        rotateLeftSpeed = 0;
        rotateRightSpeed = 0;

        foreach (EngineBlock engineBlock in listOfEngines)
        {
            engineBlock.CalculateSpeed(centerMass);

            upSpeed += engineBlock.upSpeed;
            leftSpeed += engineBlock.leftSpeed;
            rightSpeed += engineBlock.rightSpeed;
            downSpeed += engineBlock.downSpeed;

            rotateLeftSpeed += engineBlock.rotateLeftSpeed;
            rotateRightSpeed += engineBlock.rotateRightSpeed;
        }

        upSpeed += speed;
        leftSpeed += speed;
        rightSpeed += speed;
        downSpeed += speed;
        rotateLeftSpeed += speed;
        rotateRightSpeed += speed;

    }
    
    public void Move(Vector2 direction){
        CalculateSpeed();
        //and apply all engine directional values
        if(direction.x > 0){
            // transform.Translate( new Vector2(direction.x * rightSpeed, 0 ), Space.World);
            rb2d.AddForce( new Vector2(direction.x * rightSpeed, 0 ));
        }

        if(direction.x < 0){
            // transform.Translate( new Vector2(direction.x * leftSpeed, 0 ), Space.World);
            rb2d.AddForce( new Vector2(direction.x * leftSpeed, 0 ));
        }

        if(direction.y > 0){
            // transform.Translate( new Vector2( 0, direction.y * upSpeed ), Space.World);
            rb2d.AddForce( new Vector2( 0, direction.y * upSpeed ));
        }

        if(direction.y < 0){
            // transform.Translate( new Vector2( 0, direction.y * downSpeed), Space.World);
            rb2d.AddForce( new Vector2( 0, direction.y * downSpeed));
        }
    }

    public void MoveRelative(Vector2 direction){
        CalculateSpeed();
        //and apply all engine directional values
        if(direction.x > 0){
            // transform.Translate( new Vector2(direction.x * rightSpeed, 0 ), Space.Self);
            rb2d.AddRelativeForce( new Vector2(direction.x * rightSpeed, 0 ));
        }

        if(direction.x < 0){
            // transform.Translate( new Vector2(direction.x * leftSpeed, 0 ), Space.Self);
            rb2d.AddRelativeForce( new Vector2(direction.x * leftSpeed, 0 ));
        }

        if(direction.y > 0){
            // transform.Translate( new Vector2( 0, direction.y * upSpeed ), Space.Self);
            rb2d.AddRelativeForce( new Vector2( 0, direction.y * upSpeed ));
        }

        if(direction.y < 0){
            // transform.Translate( new Vector2( 0, direction.y * downSpeed), Space.Self);
            rb2d.AddRelativeForce( new Vector2( 0, direction.y * downSpeed));
        }
    }

    public void Rotate(float input){
        if(input > 0){
            transform.Rotate(0,0,rotateLeftSpeed * -input * Time.deltaTime * 10);
            // rb2d.AddTorque(rotateLeftSpeed * -input * Time.deltaTime * 10);
        }

        if(input < 0){
            transform.Rotate(0,0,rotateRightSpeed * -input * Time.deltaTime * 10);
            // rb2d.AddTorque(rotateRightSpeed * -input * Time.deltaTime * 10);
        }
    }

    public float GetAverageSpeed(){
        return (rightSpeed + leftSpeed + upSpeed + downSpeed)/4;
    }
    #endregion

    void Start(){
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        foreach (var item in gameObject.GetComponentsInChildren<Rigidbody2D>())
        {
            if(item.GetComponent<CoreBlock>() == null){
                GameObject.Destroy(item);
            } 
        }
    }

    //TODO: should be done upon tick number update to check for upgrades
    void Update(){
        CollectConnectedBlocks();
    }

}
