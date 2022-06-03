using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGun : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletDamage;

    float timer;
    float fireRate = .3f;
    public GameObject firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < fireRate){
            timer += Time.deltaTime;
        }
    }

    public void FireWeapon(){
        if(timer > fireRate){
            GameObject temp = Instantiate(bullet);
            temp.GetComponent<TestBullet>().SetBullet(bulletSpeed, bulletDamage);
            temp.transform.position = firePoint.transform.position;
            temp.transform.rotation = firePoint.transform.rotation;
            timer -= fireRate;
        }
    }
}
