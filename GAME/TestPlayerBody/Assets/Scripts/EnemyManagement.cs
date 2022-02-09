using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    private int totalEnemies = 5;
    private GameObject[] enemies;
    private Enemy theEnemyProperty;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new GameObject[totalEnemies];
        for (int i = 0; i < totalEnemies; i++)
        {
            enemies[i] = GameObject.Find("Enemy"+i);
        }
    }

    public GameObject CreateEnemy(int level, int num)
    {
        theEnemyProperty = enemies[num].GetComponent<Enemy>();

        if (level == 0)
        {
            theEnemyProperty.setMaxHealth(2000);
            
        }
        else if (level == 1)
        {
            theEnemyProperty.setMaxHealth(4000);
        }
        else if (level == 2)
        {
            theEnemyProperty.setMaxHealth(6000);
        }
        else if (level == 3)
        {
            theEnemyProperty.setMaxHealth(8000);
        }

        Debug.Log("Level: " + level + ", Create: " + enemies[num] + ", MaxHP: " + theEnemyProperty.getMaxHealth());

        return enemies[num];
    }
}
