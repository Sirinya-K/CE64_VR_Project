using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagement : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject preparationRoom;
    public GameObject arena;

    public Player player;

    private int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(false);
        preparationRoom.SetActive(false);
        arena.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == 0)
        {
            player.ResetPlayerStat();

            if(mainMenu.activeSelf == false)
            {
                mainMenu.SetActive(true);
            }
        }
        else if(state == 1)
        {
            if(preparationRoom.activeSelf == false)
            {
                preparationRoom.SetActive(true);
            }
        }
        else if(state == 2)
        {
            if(preparationRoom.activeSelf == false)
            {
                preparationRoom.SetActive(true);
            }
        }
        else if(state == 3)
        {
            ChooseState(0);
        }
    }

    public void ChooseState(int num){
        state = num;
    }
    public void GoNextState(){
        state++;
    }
    public int GetState()
    {
        return state;
    }
}
