using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    List<GameObject> listOfEnemies;
    public List<GameObject> enemiesPrefabs;
    float amountOfEnemies = 0;
    public float maxEnemies;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        listOfEnemies = new List<GameObject>();
        foreach (var item in enemies)
        {
            listOfEnemies.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        amountOfEnemies = listOfEnemies.Count;
        foreach (var item in listOfEnemies)
        {
            if(item == null){
                listOfEnemies.Remove(item);
                break;
            }
        }

        if(amountOfEnemies < maxEnemies){
            listOfEnemies.Add(GameObject.Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Count)], gameObject.transform));
        }
    }
}
