using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    //health at 100 is 1. Health lost at 1 is 50% to a 1 to 2 ratio
    //TODO: create a inspector that matches the unity healthbar gui
    GameObject healthBar;
    GameObject healthLost;
    [SerializeField]
    Health healthScript;
    SpriteRenderer[] listOfSprites;

    void Start(){
        healthBar = gameObject;
        healthLost = gameObject.transform.GetChild(0).gameObject;
        healthScript = GetComponentInParent<Health>();
        listOfSprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
    }

    float CalculatePercentHealthLost(){
        return healthScript.GetHealth()/healthScript.maxHealth;
    }

    public float currPercent = 0;
    float timer;
    float showUpTime = 3;
    void Update()
    {
        if(transform.parent == null){
            Destroy(this);
        }
        
        //reset timer if it gets hurt
        if(currPercent != CalculatePercentHealthLost()){
            timer = 0;
        }

        currPercent = CalculatePercentHealthLost();

        //don't display it hasn't been hurt
        if(currPercent == 1f){
            foreach (var item in listOfSprites)
            {
                item.enabled = false;
            }
        }else{
            //display for 3 seconds
            timer += Time.deltaTime;
            if(timer < showUpTime){
                if(listOfSprites[0].enabled == false){
                    foreach (var item in listOfSprites)
                    {
                        item.enabled = true;
                    }
                }

                healthLost.transform.localScale = new Vector3(2 - (currPercent * 2), 1, 1); 
            }else{
                //disable after 3 seconds
                foreach (var item in listOfSprites)
                {
                    item.enabled = false;
                }
            }
        }
        
    }
}
