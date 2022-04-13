using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManagement : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject preparationRoom;
    public GameObject arena;
    public GameObject playerObj;
    public SpawnManagement spawnManagement;
    public TimeCounter timeCounter;
    public Player player;

    public GameObject arenaTest;

    private int state = 3, playerState; //Default: state = 1

    void Start()
    {
        mainMenu = GameObject.Find("MainMenuRoom");
        preparationRoom = GameObject.Find("PreparationRoom");
        arena = GameObject.Find("ArenaV3");
        playerObj = GameObject.Find("VR Rig");
        spawnManagement = FindObjectOfType<SpawnManagement>();
        timeCounter = FindObjectOfType<TimeCounter>();
        player = FindObjectOfType<Player>();

        arenaTest = GameObject.Find("ArenaV3");
    }

    void Update()
    {
        // Manually Restart Game
        if(Input.GetKeyDown(KeyCode.O))
        {
            state = 1;
        }

        //State: Idle
        if(state == 0)
        {

        }
        //State: MM
        else if(state == 1)
        {
            // player.PublishArmStateToMqtt("Left", "Free");
            // player.PublishArmStateToMqtt("Right","Free");
            spawnManagement.spawn(playerObj, mainMenu.transform.Find("PlayerWaypoint").gameObject);
            playerState = 1;
            state = 0;
        }
        //State: PP
        else if(state == 2)
        {
            // player.PublishArmStateToMqtt("Left", "Free");
            // player.PublishArmStateToMqtt("Right","Free");
            // timeCounter.EndTimer();
            spawnManagement.spawn(playerObj, preparationRoom.transform.Find("PlayerWaypoint").gameObject);
            playerState = 2;
            state = 0;
        }
        //State: A
        else if(state == 3)
        {
            FindObjectOfType<Player>().SetOriginalStat(); //เก็บพวกค่าเลือดของผู้เล่น เผื่อกรณีผู้เล่นกดปุ่มกลับห้องเตรียมตัว
            timeCounter.BeginTimer();
            spawnManagement.spawn(playerObj, arena.transform.Find("PlayerWaypoint").gameObject);
            arena.GetComponent<Arena>().StartInitiate();
            playerState = 3;
            state = 0;
        }
        //State: A --> PP
        else if(state == 9)
        {
            timeCounter.EndTimer();
            FindObjectOfType<Player>().ReturnOriginalStat(); //คืนพวกค่าเลือดของผู้เล่น เมื้อผู้เล่นกดปุ่มกลับห้องเตรียมตัว
            state = 2;
        }
        //State: Test with AI Enemy
        else if(state == 99)
        {
            spawnManagement.spawn(playerObj, arenaTest.transform.Find("PlayerWaypoint").gameObject);
            state = 0;
        }
    }

    public void GoState(int num)
    {
        state = num;
    }
    public void GoNextState()
    {
        state++;
    }
    public int GetState()
    {
        return playerState;
    }
}
