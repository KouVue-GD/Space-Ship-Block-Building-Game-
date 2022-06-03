using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPlayer : MonoBehaviour
{
    //moves and controls gun to shoot
    public float moveSpeed;
    Rigidbody2D rb2d;
    TestingGun tg;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        tg = GetComponentInChildren<TestingGun>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.AddRelativeForce(new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 
                                          Input.GetAxis("Vertical")  * moveSpeed, 0));
        if(Input.GetAxis("Fire1") == 1){
            tg.FireWeapon();
        }
    }
}
