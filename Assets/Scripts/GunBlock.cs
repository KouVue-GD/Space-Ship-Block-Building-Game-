using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: split into multiple parts
public class GunBlock : Block
{
    public GameObject swivel;
    public Transform firePoint;
    public GameObject bullet;
    public float damage;
    public float fireRate;
    float fireRateTimer;
    public float bulletSpeed;
    public float reloadSpeed;
    public bool isAutoReload = false;
    public bool isManualReload = false;

    //gradual increase in recoil 
    public float recoilRate;
    float currentRecoil;
    public float recoilMax;
  
    public float ammoRate;
    public float currentAmmo;
    public float ammoMax;

    #region Keyboard

    public Vector3 offset;
    public void LookAtMouse(){
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        swivel.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        swivel.transform.eulerAngles += offset;

        LimitRotation(swivel, 45);
    }

    public void LookAtMouse(Vector3 pOffset){
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        swivel.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        swivel.transform.eulerAngles += offset;
        //adds the local angles to account for the angle difference //used when the camera also rotates
        swivel.transform.eulerAngles += pOffset;

        LimitRotation(swivel, 45);

    }

    public void LookAtTarget(GameObject target){
        Vector3 dir = target.transform.position - gameObject.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        swivel.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        swivel.transform.eulerAngles += offset;

        LimitRotation(swivel, 45);  
    }

    void LimitRotation(GameObject target, float angle){
        
        if(target.transform.localEulerAngles.z > angle && target.transform.localEulerAngles.z < 360 - angle){
            //check which one it is closer to
            if( Mathf.Abs(target.transform.localEulerAngles.z - angle) > Mathf.Abs(target.transform.localEulerAngles.z - (360 - angle))){
                //closer to angle on the left
                target.transform.localEulerAngles = new Vector3(target.transform.localEulerAngles.x,
                                                                target.transform.localEulerAngles.y,
                                                                360 - angle);
            }else{
                //closer to angle on the right
                target.transform.localEulerAngles = new Vector3(target.transform.localEulerAngles.x,
                                                                target.transform.localEulerAngles.y,
                                                                angle);
            }
        }
    }

    #endregion
    
    public void ResetLook(){
        //Vector3.Lerp(swivel.transform.position, swivelInitPos, Time.fixedTime * 2f);
        swivel.transform.localPosition = swivelInitPos;

        //Quaternion.Lerp(swivel.transform.rotation, swivelInitRot, Time.fixedTime * 2f);
        swivel.transform.localRotation = swivelInitRot;
    }

    public void FireGuns(){

        //if there is ammo you can shoot and firerate is valid
        if(CheckAmmo() == true && fireRateTimer >= fireRate){

            if(isCurrentlyManualReloading == true){
                isCurrentlyManualReloading = false;
            }

            Bullet temp = Instantiate(bullet).GetComponent<Bullet>();
            temp.transform.position = firePoint.transform.position;
            temp.transform.eulerAngles = firePoint.transform.eulerAngles;
            temp.SetBullet(bulletSpeed + gameObject.GetComponentInParent<CoreBlock>().rb2d.velocity.magnitude, damage);

            #region Recoil
            //apply recoil
            Recoil(gameObject.GetComponentInParent<Rigidbody2D>(), currentRecoil, -firePoint.transform.right);
            if(currentRecoil < recoilMax){
                currentRecoil += recoilRate;
            }
            if(GetComponentInParent<Player>() != null){
                mainCamera.ScreenShake(0.005f * currentRecoil, 0.1f);
            }
            #endregion

            #region Use Gun Resources
            fireRateTimer -= fireRate;
            currentAmmo -= 1;
            #endregion

            //TODO:gunanimation
            firePs.Play();
        }
    }

    //recoils into target direction
    void Recoil(Rigidbody2D target, float force, Vector3 direction){
        target.AddForce(direction * force);
    }

    //checks to make sure it doesn't hit itself
    public bool IsSafeToFire(){
        bool isSafe = false;//this is replaced by the else. I'm just following gun safety.
        
        RaycastHit2D hit = Physics2D.Raycast(swivel.transform.position, swivel.transform.up);
        //Debug.DrawRay(swivel.transform.position, swivel.transform.up, Color.red, 10f);
        
        if(hit != false){
            if(hit.collider.transform.GetComponent<Rigidbody2D>() == transform.GetComponent<Rigidbody2D>()){
                if(hit.transform.tag == transform.GetComponentInParent<Rigidbody2D>().tag){
                    isSafe = false;
                }
                //print(hit.collider);
            }else{
                isSafe = true;
            }
        }else{
            isSafe = true;
        }

        return isSafe;
    }

    #region Controller

    #endregion



    bool CheckAmmo(){
        bool temp = true;

        //if there is at least one bullet left return true else false
        if(currentAmmo < 1){
            temp = false;
        }
        
        return temp;
    }

    public override string GetText(){
        description = "";
        description += "Name: "+ name + "\n";
        Health health = GetComponent<Health>();
        if(health != null){
            description += "Health: " + health.GetHealth() + "/" + health.GetMaxHealth() + "\n"; 
        }

        description += "Damage: "+ damage + "\n";
        description += "Fire Rate: "+ fireRate + " RPS" + "\n";

        if(isAutoReload == true){
            description += "ReloadType: Continous" + "\n";
        }else if(isManualReload == true){
            description += "ReloadType: Magazine" + "\n";
        }

        description += "MagazineSize: "+ ammoMax + "\n";

        description += "Reload: "+ reloadSpeed + " Seconds" + "\n";
        
        description += "Weight: "+ weight;

        return description;
    }

    public string GetAmmoText(){
        return currentAmmo + "/" + ammoMax;
    }


    Vector3 swivelInitPos;
    Quaternion swivelInitRot;
    PlayerCamera mainCamera;
    ParticleSystem firePs;
    void Start(){
        swivelInitPos = swivel.transform.localPosition;
        swivelInitRot = swivel.transform.localRotation;
        mainCamera = Camera.main.GetComponent<PlayerCamera>();
        firePs = GetComponentInChildren<ParticleSystem>();
    }

    public bool isCurrentlyManualReloading = false;
    void Update(){
        if(fireRateTimer <= fireRate){
            fireRateTimer += Time.deltaTime;
        }
        //Auto reload
        if(currentAmmo < ammoMax && isAutoReload == true){
            currentAmmo += ammoRate * Time.deltaTime;
        }

        //reload
        if(currentAmmo <= 0 && isManualReload == true && isCurrentlyManualReloading == false){
            currentAmmo = ammoMax;
            isCurrentlyManualReloading = true;
            fireRateTimer = 0;
            fireRateTimer -= reloadSpeed;
            //TODO: reload animation
        }

        //currentRecoil -= Time.deltaTime * recoilRate;



        //LookAtMouse();
        
    }
}
