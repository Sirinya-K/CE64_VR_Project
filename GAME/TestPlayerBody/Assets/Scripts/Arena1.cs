// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class Arena : MonoBehaviour
// {
//     public StateManagement stateManagement;

//     public GameObject playerObj;
//     public Player player;
//     public GameObject enemyObj;
//     public Enemy enemy;

//     public SpawnManagement spawnManagement;
//     public GameObject enemyWaypoint;

//     private bool enemySpawn;

//     // Start is called before the first frame update
//     void Start()
//     {
//         enemy = enemyObj.GetComponent<Enemy>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (!enemySpawn)
//         {
//             enemyObj.SetActive(true);
//             spawnManagement.spawn(enemyObj, enemyWaypoint);
//             enemySpawn = true;
//         }

//         //ถ้าศัตรูตาย
//         if (enemy.getHealth() <= 0)
//         {
//             // EnemyDead();

//             Debug.Log("BACK TO PREPARATION ROOM");

//             enemyObj.SetActive(false);
//             enemySpawn = false;
//             enemy.ResetEnemyStat();

//             stateManagement.onArena = false;
//             stateManagement.playerWin = true;
//         }

//         //ถ้าผู้เล่นตาย
//         if (player.getHealth() <= 0)
//         {
//             stateManagement.playerFail = true;
//         }
//     }

//     // private void EnemyDead()
//     // {
//     //     enemyObj.SetActive(false);
//     //     enemy.ResetEnemyStat();

//     //     stateManagement.onArena = false;
//     //     stateManagement.GoNextState();
//     //     enemySpawn = false;
//     //     this.gameObject.SetActive(false);
//     // }
// }
