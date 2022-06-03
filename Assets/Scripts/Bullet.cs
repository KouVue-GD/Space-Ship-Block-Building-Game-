using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    [SerializeField]
    float damage;
    
    Vector3 initialPos;
    float maxDistance;

    public Rigidbody2D rb2d;
    
    public void SetBullet(float speed){
        this.speed = speed;
    }
    public void SetBullet(float speed, float damage){
        this.speed = speed;
        this.damage =  damage;
    }

    bool hasSetVelocity = false;
    float timer = 0;
    public float lifeTime = 10;
    void Update(){
        timer += Time.deltaTime;
        if( hasSetVelocity == false ){
            rb2d.velocity = transform.right * speed;
            maxDistance = speed * 10;
            initialPos = transform.position;
            hasSetVelocity = true;
        }
        //print(initialPos);
        //print(Vector3.Distance(initialPos, transform.position) + " > " + maxDistance + " ? " + (Vector3.Distance(initialPos, transform.position) > maxDistance));
        if(Vector3.Distance(initialPos, transform.position) > maxDistance){
            Destroy(gameObject);
        }else if(timer > lifeTime){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        timer = 0;
        lifeTime = 1;
    }
    ParticleSystem impactPs;
    void Awake()
    {
        impactPs = GetComponentInChildren<ParticleSystem>();
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        impactPs.Play();
        Health health = coll.transform.GetComponent<Health>();
        if(health != null){
            health.TakeDamage(damage);
        }
        // timer = 0;
        // lifeTime = 1;
        Destroy(gameObject, 1f);
    }
}
