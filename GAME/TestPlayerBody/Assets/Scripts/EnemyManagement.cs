using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    private int totalEnemies = 1;
    private GameObject[] enemies;
    private Enemy theEnemyProperty;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = new GameObject[totalEnemies];
        for (int i = 0; i < totalEnemies; i++)
        {
            enemies[i] = GameObject.Find("Enemy" + 1);
            Debug.Log(enemies[i].name);

            enemies[i].SetActive(false);
        }
    }

    public GameObject CreateEnemy(int level, int num)
    {
        theEnemyProperty = enemies[num].GetComponent<Enemy>();

        if (level == 0)
        {
            theEnemyProperty.SetMaxHp(150); // 5000
        }
        else if (level == 1)
        {
            theEnemyProperty.SetMaxHp(150);
        }
        else if (level == 2)
        {
            theEnemyProperty.SetMaxHp(150);
        }
        else if (level == 3)
        {
            theEnemyProperty.SetMaxHp(150);
        }

        Debug.Log("Level: " + level + ", Create: " + enemies[num].name + ", MaxHP: " + theEnemyProperty.GetMaxHp());

        return enemies[num];
    }
}
