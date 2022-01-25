using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public SpawnManagement spawnManagement;
    public GameObject enemyWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        enemy.SetActive(true);
        spawnManagement.spawn(enemy, enemyWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
