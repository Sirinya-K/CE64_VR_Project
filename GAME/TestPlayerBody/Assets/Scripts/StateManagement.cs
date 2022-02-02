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

    private int state = 1;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = GameObject.Find("MainMenuRoom");
        preparationRoom = GameObject.Find("PreparationRoom");
        arena = GameObject.Find("Arena");
        player = GameObject.Find("VR Rig");
        spawnManagement = FindObjectOfType<SpawnManagement>();
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
            state = 0;
        }
        //State: PP
        else if(state == 2)
        {
            spawnManagement.spawn(player, preparationRoom.transform.Find("PlayerWaypoint").gameObject);
            state = 0;
        }
        //State: A
        else if(state == 3)
        {
            spawnManagement.spawn(player, arena.transform.Find("PlayerWaypoint").gameObject);
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
        return state;
    }
}
