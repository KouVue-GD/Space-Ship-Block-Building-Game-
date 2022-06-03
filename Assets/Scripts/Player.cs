using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Is a core block but with controls
public class Player : CoreBlock
{

    public bool isBuildModeOn = false;
    PlayerCamera playerCameraScript;
    void Start(){
        textInfo = GUITextObject.GetComponentInChildren<Text>();
        textPos = GUITextObject.GetComponent<RectTransform>();
        canvas = GUITextObject.GetComponentInParent<Canvas>();
        mainCam = Camera.main;
        playerCameraScript = mainCam.GetComponent<PlayerCamera>();
        CollectConnectedBlocks();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        //check for connected parts
        foreach (var item in gameObject.GetComponentsInChildren<Rigidbody2D>())
        {
            if(item.GetComponent<CoreBlock>() == null){
                GameObject.Destroy(item);
                // item.isKinematic = false;
            } 
        }
        line = GetComponent<LineRenderer>();
    }

    public bool isRotateModeOn = false;
    public bool isMouseModeOn = true;
    Camera mainCam;

    public Text ammoText;
     void Update(){
        CollectConnectedBlocks();
        mainCam.orthographicSize -= Input.mouseScrollDelta.y;
        mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize, 10, 38);

        ammoText.text = "";
        foreach (var item in listOfGunBlocks)
        {
            if(item.isCurrentlyManualReloading == true){
                ammoText.text += "Gun Ammo: Reloading \n"; 
            }else{
                ammoText.text += "Gun Ammo: " + item.currentAmmo + "/" + item.ammoMax + "\n"; 
            }
            
        }

        if(Input.GetKeyUp("b")){
            if(isBuildModeOn == false){
                isBuildModeOn = true;
                GUITextObject.SetActive(false);
                
                ResetAllGuns();
            }else{
                isBuildModeOn = false;
            }
        }

        if(isBuildModeOn == false){
            Time.timeScale = 1;
            line.enabled = false;


            if(isMouseModeOn == false){
                if(isRotateModeOn == false){
                    //don't rotate camera, move globally, aim at mouse
                    AllGunsLookAtMouse();
                    if(Input.GetMouseButton(0) == true){
                        FireAllGuns();
                    }

                    Move( new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
                    Rotate(Input.GetAxis("Rotate"));
                }else{
                    //rotate the camera and move locally
                    mainCam.transform.rotation = transform.rotation;
                    AllGunsLookAtMouse(transform.localEulerAngles);
                    if(Input.GetMouseButton(0) == true){
                        FireAllGuns();
                    }

                    MoveRelative( new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
                    Rotate(Input.GetAxis("Rotate"));
                }
            }

            if(isMouseModeOn == true){
                //rotate to mouse, move locally, aim with mouse

                //rotate to mouse
                Vector3 dir = Input.mousePosition - mainCam.WorldToScreenPoint(transform.position);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.localEulerAngles += new Vector3(0,0,-90);

                //shoot guns
                AllGunsLookAtMouse();
                if(Input.GetMouseButton(0) == true){
                    FireAllGuns();
                }

                //move forward based on localPosition
                MoveRelative( new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
                
            }
            
        }else{
            BuildMode();
        }
    }


    //TODO: rotate to mouse using rotational speed
    void RotateToMouse(){
        //check which rotate would be faster or closer
    }

    void AllGunsLookAtMouse(){
        foreach (GunBlock gunBlock in listOfGunBlocks)
        {
            gunBlock.LookAtMouse();
        }
    }

    void AllGunsLookAtMouse(Vector3 eularAngle){ //for when the camera also rotates
        foreach (GunBlock gunBlock in listOfGunBlocks)
        {
            gunBlock.LookAtMouse(eularAngle);
        }
    }

    GameObject blockToDrag = null;
    public GameObject GUITextObject;
    public Vector3 GUIOffset;
    RectTransform textPos;
    Text textInfo;
    Canvas canvas;
    GameObject blockForTextInfo;
    void BuildMode(){
        Time.timeScale = 0;

        #region Text tooltip
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetTextPos = Vector3.zero;
        blockForTextInfo = GetTheBlockMouseIsOver();
        if(blockForTextInfo != null){
            if(GUITextObject.activeSelf == false){
                GUITextObject.SetActive(true);
            }

            //TODO: check if left or right side of the screen
            targetTextPos = mousePos + GUIOffset;
            if( Screen.width/2 <= targetTextPos.x ){
                targetTextPos.x -= GUIOffset.x * 2;
            }

            textPos.transform.position = targetTextPos;
            textInfo.text = blockForTextInfo.GetComponent<Block>().GetText();

        }else{
            if(GUITextObject.activeSelf == true){
                GUITextObject.SetActive(false);
            }
        }
        #endregion

        //mouse
        if(Input.GetMouseButtonDown(0) == true){
            blockToDrag = GetTheBlockMouseIsOver();
            //remove exception warning/error
            if(blockToDrag == null){}

            //don't pick up coreblocks
            else if(blockToDrag.GetComponent<CoreBlock>() != null){
                blockToDrag = null;
            }

            //don't pick up from enemies
            else if(blockToDrag.GetComponentInParent<BasicEnemyAI>() != null){
                blockToDrag = null;
            }

            //Should never happen
            else if(blockToDrag.GetComponentInChildren<CoreBlock>() != null){
                print("You picked up a coreblock in children? Error in Player.GetMouseOver()");
            }

            //remove collision
            if(blockToDrag == null){} // to remove exception error
            else if(blockToDrag.GetComponent<Rigidbody2D>() != null){
                Destroy(blockToDrag.GetComponent<Rigidbody2D>());
                // blockToDrag.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
        if(Input.GetMouseButton(0) == true){
            if(blockToDrag != null){
                DragBlockWithMouse(blockToDrag);
            }
        }
        if(Input.GetMouseButtonUp(0) == true){
            if(blockToDrag != null){
                ConnectToClosest(blockToDrag);
                blockToDrag = null;
                line.enabled = false;
            }
        }

        //keyboard


        //controller

    }

    GameObject GetTheBlockMouseIsOver(){
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, 1500f);

        
        //TODO: circle cast of .1 position to grab block
        float closetBlockDistance = float.PositiveInfinity;
        GameObject closestBlock = null;
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        foreach (RaycastHit2D hit in hits)
        {
            if(hit.transform.gameObject.GetComponent<Block>() != null){
                Block[] arrayOfBlocks = hit.transform.GetComponentsInChildren<Block>();
                foreach (Block block in arrayOfBlocks){
                    //print( block + ": MousePos: " + mousePos + " - " + "blockPos: " + block.transform.position + " = " + Vector3.Distance( mousePos, block.transform.position));
                    float currDistance = Vector2.Distance( mousePos, block.transform.position);

                    //return the object as a dragable object
                    if(currDistance < closetBlockDistance){
                        closetBlockDistance = currDistance;
                        closestBlock = block.transform.gameObject;
                        //print(closestBlock);
                    }
                }
                
            }
        }

        return closestBlock;
    }
    LineRenderer line;
    GameObject tempClosestBlock;
    void DragBlockWithMouse(GameObject pBlock){
        pBlock.GetComponent<Block>().DisconnectAllChildren();
        pBlock.transform.position = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pBlock.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, 0);


        tempClosestBlock = GetClosestBlock(pBlock);

        if(tempClosestBlock != null){
            pBlock.transform.position = tempClosestBlock.transform.position;
            pBlock.transform.rotation = tempClosestBlock.transform.rotation;
        }else{
            pBlock.transform.position = mainCam.ScreenToWorldPoint(Input.mousePosition);
            pBlock.transform.position = new Vector3(pBlock.transform.position.x, pBlock.transform.position.y, 0);
        }

        line.SetPosition(0, pBlock.transform.position);
        line.SetPosition(1, mainCam.ScreenToWorldPoint(Input.mousePosition));
        line.enabled = true;
        
    }

    GameObject GetClosestBlock(GameObject pBlock){
        if(pBlock != null){
            Block[] aBlocks = gameObject.GetComponentsInChildren<Block>();
            Block closestBlock = aBlocks[0];
            float closestDistance  = Vector2.Distance(pBlock.transform.position, aBlocks[0].transform.position);

            //Find the Closest Block
            foreach (Block block in aBlocks)
            {
                float currDistance = Vector2.Distance(pBlock.transform.position, block.transform.position);
                if(currDistance < closestDistance){
                    closestBlock = block;
                    closestDistance  = currDistance;
                }
            }
            //check all 4 sides
            GameObject closestConnection = closestBlock.connectionUp;
            closestDistance = Vector2.Distance(closestBlock.connectionUp.transform.position, pBlock.transform.position);
            Direction closestDirection = Direction.up;

            
            float leftDistance = Vector2.Distance(closestBlock.connectionLeft.transform.position, pBlock.transform.position);
            float rightDistance = Vector2.Distance(closestBlock.connectionRight.transform.position, pBlock.transform.position);
            float downDistance = Vector2.Distance(closestBlock.connectionDown.transform.position, pBlock.transform.position);

            if(leftDistance < closestDistance){
                closestConnection = closestBlock.connectionLeft;
                closestDistance  = leftDistance;
                closestDirection = Direction.left;
            }

            if(rightDistance < closestDistance){
                closestConnection = closestBlock.connectionRight;
                closestDistance  = rightDistance;
                closestDirection = Direction.right;
            }

            if(downDistance < closestDistance){
                closestConnection = closestBlock.connectionDown;
                closestDistance  = downDistance;
                closestDirection = Direction.down;
            }

            //Check if object is in range
            if(closestDistance < 5f){
                return pBlock.GetComponent<Block>().GetTargetConnection(closestDirection, closestBlock.gameObject);
            }else{
                return null;
            }
        }
        return null;

    }

    //connect pblock to closest block
    void ConnectToClosest(GameObject pBlock){
        
        //TODO: use closest connection as a highlight

        if(pBlock != null){
            
            pBlock.GetComponent<Block>().ConnectToTarget(GetClosestBlock(pBlock));
        }else{
            //add collision and relase to back to space
            AddRigidBody(pBlock);
            // pBlock.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
}
