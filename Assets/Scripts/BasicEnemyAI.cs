using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//contains the basic enemy ai
public class BasicEnemyAI : MonoBehaviour
{
    CoreBlock coreBlock;
    public GameObject target;
    public float maxShootingDistance;

    public bool isAICircle;

    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {

        ps = GetComponentInChildren<ParticleSystem>();
        coreBlock = gameObject.GetComponent<CoreBlock>();
        if(target == null){
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        AICircleTarget(target);
        AllGunsLookAtTarget(target);
        //attempt to fire weapon as much as possible
        if(Vector3.Distance(gameObject.transform.position, target.transform.position) < maxShootingDistance){
            coreBlock.FireAllGuns();
        }else{
            coreBlock.ResetAllGuns();
        }

    }

    void AllGunsLookAtTarget(GameObject target){
        foreach (GunBlock gunBlock in coreBlock.listOfGunBlocks)
        {
            gunBlock.LookAtTarget(target);
        }
    }

    public float circleRange;

    float angle = 0;
    Vector3 dir;
    void AICircleTarget(GameObject target){
        //check ai type
        if(isAICircle == true){
        //look for player
            if(target != null){

                //try to get close first
                if(Vector3.Distance(gameObject.transform.position, target.transform.position) > circleRange){
                    //get direction towards player
                    dir = (transform.InverseTransformPoint(target.transform.position) - gameObject.transform.position).normalized;
                    coreBlock.MoveRelative(dir);

                }else{//if close enough...
                //TODO: rotate to target rotaion based on core Rotation speeds
                    angle = Time.fixedTime;// * (coreBlock.GetAverageSpeed()/5);
                    
                    //spot on target circle
                    Vector3 pos = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * circleRange;
                    pos += target.transform.position;

                    //direction towards spot
                    pos -= transform.position;
                    
                    coreBlock.Move( pos.normalized );

                    //TODO: AI switch direction occasionally

                }
            }
        }
    }
    void OnDestroy()
    {
        if(ps != null){

            ps.transform.parent = null;
            ps.Play();
            Destroy(ps, 5f);
        }
    }

    void OnApplicationQuit(){
        if(ps != null){
            Destroy(ps);
        }
    }
}
