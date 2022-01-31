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
    [HideInInspector] public bool onMainMenu, onPreparationRoom, onArena;

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
        // Debug.Log(state);

        if(state == 0)
        {
            // if(mainMenu.activeSelf == false)
            // {
            //     mainMenu.SetActive(true);
            // }

            if(onMainMenu == false)
            {
                //reset ข้อมูล player ทุกครั้งที่เริ่มใหม่
                player.ResetPlayerStat();
                
                mainMenu.SetActive(true);
                onMainMenu = true;
            }
        }
        else if(state == 1)
        {
            // if(preparationRoom.activeSelf == false)
            // {
            //     preparationRoom.SetActive(true);
            // }

            if(onArena == false)
            {
                if(onPreparationRoom == false)
                {
                    preparationRoom.SetActive(true);
                    onPreparationRoom = true;
                }
            }
        }
        else if(state == 2)
        {
            if(onArena == false)
            {
                if(onPreparationRoom == false)
                {
                    preparationRoom.SetActive(true);
                    onPreparationRoom = true;
                }
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
