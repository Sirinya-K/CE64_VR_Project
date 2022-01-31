using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public StateManagement stateManagement;
    
    public GameObject playerObj;

    public SpawnManagement spawnManagement;
    public GameObject startWaypoint;

    private bool playerSpawn;
    private StartButton startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton = GetComponentInChildren<StartButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerSpawn)
        {
            spawnManagement.spawn(playerObj, startWaypoint);
            playerSpawn = true;
        }

        //ถ้า player กดปุ่ม start
        if(startButton.collision)
        {
            Debug.Log("START");

            stateManagement.onMainMenu = false;
            stateManagement.GoNextState();

            playerSpawn = false;
            startButton.collision = false;

            this.gameObject.SetActive(false);
        }

        //ถ้า player กดปุ่ม score
        
        //ถ้า player กดปุ่ม exit
    }
}
