using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Block))]
public class BlockEditor : Editor
{
    public GameManager gm;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        #region  Block Connection Management
        //keep the block types the same as game manager
        //initlizing list
        if(((Block)target).listOfBlockConnections.Count == 0){
            for (int i = 0; i < gm.ListOfBlocks.Count - 1; i++)
            {
                ((Block)target).listOfBlockConnections.Add(new Block.blockConnections( gm.ListOfBlocks[i].GetComponent<Block>().blockTypeName, false) );
            }
        }

        //check to see if connection has blocktype, if it doesn't add
        //check if there are duplicates
        //sort
        for (int i = 0; i < gm.ListOfBlocks.Count; i++)
        {
            bool hasName = false;
            int duplicates = 0;

            for (int j = 0; j < ((Block)target).listOfBlockConnections.Count; j++)
            {
                if ( gm.ListOfBlocks[i].GetComponent<Block>().blockTypeName == ((Block)target).listOfBlockConnections[j].blockTypeName){
                    hasName = true;
                    duplicates++;
                }
            }

            //add new block
            if(hasName == false){
                ((Block)target).listOfBlockConnections.Add(new Block.blockConnections( gm.ListOfBlocks[i].GetComponent<Block>().blockTypeName, false) );
            }

            //remove duplicates
            if(hasName == true && duplicates > 1){
                for (int j = 0; j < ((Block)target).listOfBlockConnections.Count; j++)
                {
                    if ( gm.ListOfBlocks[i].GetComponent<Block>().blockTypeName == ((Block)target).listOfBlockConnections[j].blockTypeName){
                        ((Block)target).listOfBlockConnections.RemoveAt(j);
                        duplicates--;
                    }
                }
            }

            //sort
            if(hasName == true){
                //if the connection isn't the same as the gm's list
                if(((Block)target).listOfBlockConnections[i].blockTypeName != gm.ListOfBlocks[i].GetComponent<Block>().blockTypeName){
                    //find the right name
                    for (int j = 0; j < ((Block)target).listOfBlockConnections.Count; j++)
                    {
                        //switch it around
                        if ( gm.ListOfBlocks[i].GetComponent<Block>().blockTypeName == ((Block)target).listOfBlockConnections[j].blockTypeName ){
                            Block.blockConnections temp = ((Block)target).listOfBlockConnections[i];
                            ((Block)target).listOfBlockConnections[i] = ((Block)target).listOfBlockConnections[j];
                            ((Block)target).listOfBlockConnections[j] = temp;
                        }
                    }
                }
            }
        }

        //check to see if connection has extra blocktypes, if it does remove
        for (int i = 0; i < ((Block)target).listOfBlockConnections.Count; i++)
        {
            //variable containing if gm has the same name
            bool hasName = false;
            for (int j = 0; j < gm.ListOfBlocks.Count; j++)
            {
                if ( ((Block)target).listOfBlockConnections[i].blockTypeName == gm.ListOfBlocks[j].GetComponent<Block>().blockTypeName ){
                    hasName = true;
                }
            }

            if(hasName == false){
                ((Block)target).listOfBlockConnections.RemoveAt(i);
            }
        }
        #endregion
          
        ((Block)target).invalidConnectionUp = GUILayout.Toggle(((Block)target).invalidConnectionUp, "Up", "Button");

        GUILayout.BeginHorizontal();
        ((Block)target).invalidConnectionLeft = GUILayout.Toggle(((Block)target).invalidConnectionLeft, "Left", "Button");
        GUILayout.Space(20);
        ((Block)target).invalidConnectionRight = GUILayout.Toggle(((Block)target).invalidConnectionRight, "Right", "Button");
        GUILayout.EndHorizontal();

        ((Block)target).invalidConnectionDown = GUILayout.Toggle(((Block)target).invalidConnectionDown, "Down", "Button");
    }

}

