using System.Collections.Generic;
using UnityEngine;
using System;

public class Block : MonoBehaviour
{
    public string blockTypeName = "Basic Block";

    public float weight;

    private float currLevel;
    private float maxLevel;

    [HideInInspector]
    public GameManager gm;

    #region  Block Connections
    //Blocktype is create a list of valid connections
    [Serializable]
    public struct blockConnections {
        public string blockTypeName;
        public bool isValid;

        public blockConnections(string blockTypeName, bool isValid){
            this.blockTypeName = blockTypeName;
            this.isValid = isValid;
        }
    }
    //list of valid connection
    public List<blockConnections> listOfBlockConnections = new List<blockConnections>();

    public List<blockConnections> GetListOfBlockConnections(){
        return listOfBlockConnections;
    }
    #endregion

    #region  Directional Connections
    [HideInInspector]
    public bool invalidConnectionUp = false;
    [HideInInspector]
    public bool invalidConnectionLeft = false;
    [HideInInspector]
    public bool invalidConnectionRight = false;
    [HideInInspector]
    public bool invalidConnectionDown = false;

    public GameObject connectionUp;
    public GameObject connectionLeft;
    public GameObject connectionRight;
    public GameObject connectionDown;
    #endregion

    public enum Direction{
        up,
        left,
        right,
        down
    }

    public GameObject GetTargetConnection(Direction direction, GameObject target){
        GameObject correctTarget = null;

        CheckConnectionIsInvalid();

        //add to correct connection
        if(direction == Direction.up && invalidConnectionUp == false){
            correctTarget = target.GetComponent<Block>().connectionUp;
        }
        
        if(direction == Direction.left && invalidConnectionLeft == false){
            correctTarget = target.GetComponent<Block>().connectionLeft;
        }

        if(direction == Direction.right && invalidConnectionRight == false){
            correctTarget = target.GetComponent<Block>().connectionRight;
        }

        if(direction == Direction.down && invalidConnectionDown == false){
            correctTarget = target.GetComponent<Block>().connectionDown;
        }

        return correctTarget;
    }

    void CheckConnectionIsInvalid(){
        if(connectionUp.transform.childCount > 0){
            invalidConnectionUp = true;
        }
        if(connectionLeft.transform.childCount > 0){
            invalidConnectionLeft = true;
        }
        if(connectionRight.transform.childCount > 0){
            invalidConnectionRight = true;
        }
        if(connectionDown.transform.childCount > 0){
            invalidConnectionDown = true;
        }
    }

    //connect this block to the target
    public void ConnectToTarget(GameObject target){
        if(target != null){
            //add to correct connection
            gameObject.transform.SetParent(target.transform);

            //reset local positions
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;

            //remove physics simulation
            GameObject.Destroy(gameObject.GetComponent<Rigidbody2D>());
            // gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }

    }

    public Rigidbody2D AddRigidBody(GameObject target){
        Rigidbody2D grb2d = target.GetComponent<Rigidbody2D>();
        //add rigidbody
        if(target.GetComponent<Rigidbody2D>() == null){

            grb2d = gameObject.AddComponent<Rigidbody2D>();
            grb2d.drag = 1;
            grb2d.gravityScale = 0;
            grb2d.mass = 2;
            grb2d.angularDrag = 1;
            return grb2d;
        }

        return grb2d;
    }

    

    public void Disconnect(){
        Vector3 temp;
        Vector3 tempParentPos;
        temp = gameObject.transform.position;
        tempParentPos = temp;
        if(transform.GetComponentInParent<Rigidbody2D>() != null){
            tempParentPos = transform.GetComponentInParent<Rigidbody2D>().position;
        }
        gameObject.transform.SetParent(null);
        gameObject.transform.position = temp;
        Rigidbody2D rb2d = AddRigidBody(gameObject);
        rb2d.velocity = (tempParentPos - temp) * 30;
        rb2d.velocity = new Vector3(rb2d.velocity.x * UnityEngine.Random.Range(-1f, 1f) , rb2d.velocity.y * UnityEngine.Random.Range(-1f, 1f), 0f);
        print(rb2d.velocity);
        // gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

        
    }

    public void DisconnectAllChildren(){
        Block[] allConnectedBlocks = gameObject.GetComponentsInChildren<Block>();
        // foreach (Block block in allConnectedBlocks)
        // {
        //     block.Disconnect();
        // }

        for (int i = allConnectedBlocks.Length - 1; i >= 0; i--)
        {
            allConnectedBlocks[i].Disconnect();
        }
    }

    [HideInInspector]
    public string description = "";
    public virtual String GetText(){
        description = "";
        description += "Name: "+ name + "\n";
        Health health = GetComponent<Health>();
        if(health != null){
            description += "Health: " + health.GetHealth() + "/" + health.GetMaxHealth() + "\n"; 
        }
        description += "Weight: "+ weight;
        return description;
    }

    // void OnDestroy()
    // {
    //     if(GetComponentInChildren<ParticleSystem>() != null){
    //         GetComponentInChildren<ParticleSystem>().Play();
    //     }
    // }
}