using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    public float speed;
    public float damage;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
    
        rb2d.velocity = (speed * gameObject.transform.up);

    }

    public void SetBullet(float speed, float damage){
        this.speed = speed;
        this.damage = damage;
    }
}
