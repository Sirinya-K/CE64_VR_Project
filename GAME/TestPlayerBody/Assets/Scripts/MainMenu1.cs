// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MainMenu : MonoBehaviour
// {
//     public StateManagement stateManagement;
//     public SpawnManagement spawnManagement;

//     public GameObject playerObj;
//     public GameObject startWaypoint;

//     private bool playerSpawn;
//     private StartButton startButton;

//     // Start is called before the first frame update
//     void Start()
//     {
//         stateManagement = FindObjectOfType<StateManagement>();
//         spawnManagement = FindObjectOfType<SpawnManagement>();
//         startButton = GetComponentInChildren<StartButton>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(!playerSpawn)
//         {
//             spawnManagement.spawn(playerObj, startWaypoint);
//             playerSpawn = true;
//         }

//         //ถ้า player กดปุ่ม start
//         if(startButton.collision)
//         {
//             Debug.Log("START");

//             playerSpawn = false;
//             startButton.collision = false;

//             stateManagement.playerStart = true;
//             stateManagement.onMainMenu = false;
//         }

//         //ถ้า player กดปุ่ม score
        
//         //ถ้า player กดปุ่ม exit
//     }
// }
