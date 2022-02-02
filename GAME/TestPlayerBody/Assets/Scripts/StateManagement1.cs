// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class StateManagement : MonoBehaviour
// {
//     public GameObject mainMenu;
//     public GameObject preparationRoom;
//     public GameObject arena;

//     [HideInInspector] public bool onMainMenu, onPreparationRoom, onArena;
//     [HideInInspector] public bool playerStart, playerWin, playerFail;

//     private int state = 0;
//     private float stateDelay = 1f;
//     private GameObject showResult;
//     private Text fightResult;

//     // Start is called before the first frame update
//     void Start()
//     {
//         mainMenu.SetActive(false);
//         preparationRoom.SetActive(false);
//         arena.SetActive(false);

//         showResult = GameObject.Find("ResultCanvas");
//         showResult.SetActive(false);
//         fightResult = showResult.GetComponentInChildren<Text>();

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         Debug.Log(state);

//         if (state == 0)
//         {
//             //Active: Main Menu
//             if (!onMainMenu)
//             {
//                 if (!playerStart)
//                 {
//                     mainMenu.SetActive(true);
//                     onMainMenu = true;
//                 }
//             }
//             if (playerStart)
//             {
//                 mainMenu.SetActive(false);
//                 GoNextState();
//                 playerStart = false;
//             }
//         }
//         else if (state == 1)
//         {
//             // if (onArena == false)
//             // {
//             //     if (onPreparationRoom == false)
//             //     {
//             //         preparationRoom.SetActive(true);
//             //         onPreparationRoom = true;
//             //     }
//             // }

//             //Active: Preparation Room
//             if (!onArena)
//             {
//                 if (!onPreparationRoom)
//                 {
//                     arena.SetActive(false);
//                     preparationRoom.SetActive(true);
//                     onPreparationRoom = true;
//                 }
//             }
//             //Active: Arena
//             if (!onPreparationRoom)
//             {
//                 if (!onArena)
//                 {
//                     preparationRoom.SetActive(false);
//                     arena.SetActive(true);
//                     onArena = true;
//                 }
//             }
//             //ถ้า player ชนะ enemy
//             if (playerWin)
//             {
//                 Invoke("FinishGame", stateDelay);
//                 playerWin = false;
//             }
//             //ถ้า player แพ้ enemy
//             if (playerFail)
//             {
//                 Invoke("GameOver", stateDelay);
//                 playerFail = false;
//             }
//         }
//         else if (state == 2) //State สุดท้าย
//         {

//         }
//         else if (state == 3)
//         {

//         }
//     }

//     public void ChooseState(int num)
//     {
//         state = num;
//     }
//     public void GoNextState()
//     {
//         state++;
//     }
//     public int GetState()
//     {
//         return state;
//     }

//     private void FinishGame()
//     {
//         showResult.SetActive(true);
//         fightResult.text = "VICTORY! :D";
//         RestartGame();
//     }

//     private void GameOVer()
//     {
//         showResult.SetActive(true);
//         fightResult.text = "GAME OVER :(";
//         RestartGame();
//     }

//     private void RestartGame()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }
// }
