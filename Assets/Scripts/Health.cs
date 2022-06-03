using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for use only for block
public class Health : MonoBehaviour
{
    float maxArmor = 900;
    public float armor;
    public float maxHealth;
    float minHealth = 0;

    //[SerializeField, ReadOnlyAttribute]
    [SerializeField]
    float currHealth;


    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currHealth <= minHealth)
        {
            if(gameObject.GetComponent<Block>() != null){
                gameObject.GetComponent<Block>().DisconnectAllChildren();
            }
            

            //TODO: death animation
            Destroy(gameObject);
        }

        //no over heals
        if(currHealth > maxHealth){
            currHealth = maxHealth;
        }
    }

    public void SetCurrHealth(float health){
        currHealth = health;
    }

    public void TakeDamage(float damage){
        currHealth -= (damage - (damage * ((armor/maxArmor)*0.8f)));
        //80% damage reduction is 900. 900 is max armor so armor is (armor/maxarmor)* .8 = 80% 
        //so 200 armor is 200/900= 22% * .8 = .177 or 17.8% damage reduction
        //theortical armor for 100 damage reduction is 1125 armor 
        //TODO: better armor formula probably linear scaling up to a certain point and then soft cap
    }

    //You could use hurt health but then it's function would be for a different purpose
    public void HealHealth(float heal){
        currHealth += heal;
    }

    public float GetHealth(){
        return currHealth;
    }

    public float GetMaxHealth(){
        return maxHealth;
    }
}
