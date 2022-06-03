using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to get a value between -1 and 1 determined by a percentage
//for example: to find how close the vector are to 50% or finding if a object is diagonal to another object
//untested

public class NormalizeVectorsByPercent: MonoBehaviour
{
    [Range(0,1), Tooltip("0 to 100%")]
    public float percent;

    Vector3 Normalize(Vector3 pVector){
        float total = pVector.x + pVector.y + pVector.z;
        total *= percent;
        Vector3 normalizedVector = new Vector3( pVector.x/total,
                                                pVector.y/total,
                                                pVector.z/total);
        if(normalizedVector.x > 1){
            float leftOverValue = normalizedVector.x - 1;
            normalizedVector = new Vector3( normalizedVector.x - leftOverValue,
                                            normalizedVector.y,
                                            normalizedVector.z);
        }

        if(normalizedVector.y > 1){
            float leftOverValue = normalizedVector.y - 1;
            normalizedVector = new Vector3( normalizedVector.x,
                                            normalizedVector.y - leftOverValue,
                                            normalizedVector.z);
        }

        if(normalizedVector.z > 1){
            float leftOverValue = normalizedVector.z - 1;
            normalizedVector = new Vector3( normalizedVector.x,
                                            normalizedVector.y,
                                            normalizedVector.z - leftOverValue);
        }

        return normalizedVector;
    }

    Vector2 Normalize(Vector2 pVector){
        float total = pVector.x + pVector.y;
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

        return normalizedVector;
    }
}
