using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagement : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject preparationRoom;
    public GameObject arena;
    public GameObject player;
    public SpawnManagement spawnManagement;

    public GameObject arenaTest;

    private int state = 1, playerState; //Default: state = 1

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = GameObject.Find("MainMenuRoom");
        preparationRoom = GameObject.Find("PreparationRoom");
        arena = GameObject.Find("Arena");
        player = GameObject.Find("VR Rig");
        spawnManagement = FindObjectOfType<SpawnManagement>();

        arenaTest = GameObject.Find("ArenaScene (TEST)");
    }

    // Update is called once per frame
    void Update()
    {
        //State: Idle
        if(state == 0)
        {

        }
        //State: MM
        else if(state == 1)
        {
            spawnManagement.spawn(player, mainMenu.transform.Find("PlayerWaypoint").gameObject);
            playerState = 1;
            state = 0;
        }
        //State: PP
        else if(state == 2)
        {
            spawnManagement.spawn(player, preparationRoom.transform.Find("PlayerWaypoint").gameObject);
            playerState = 2;
            state = 0;
        }
        //State: A
        else if(state == 3)
        {
            spawnManagement.spawn(player, arena.transform.Find("PlayerWaypoint").gameObject);
            arena.GetComponent<Arena>().StartInitiate();
            FindObjectOfType<Player>().SetOriginalStat(); //เก็บพวกค่าเลือดของผู้เล่น เผื่อกรณีผู้เล่นกดปุ่มกลับห้องเตรียมตัว
            playerState = 3;
            state = 0;
        }
        //State: A --> PP
        else if(state == 9)
        {
            FindObjectOfType<Player>().ReturnOriginalStat(); //คืนพวกค่าเลือดของผู้เล่น เมื้อผู้เล่นกดปุ่มกลับห้องเตรียมตัว
            state = 2;
        }
        //State: Test with AI Enemy
        else if(state == 99)
        {
            spawnManagement.spawn(player, arenaTest.transform.Find("PlayerWaypoint").gameObject);
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
