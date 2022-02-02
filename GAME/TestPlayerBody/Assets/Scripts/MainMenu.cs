using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public StateManagement stateManagement;

    private StartButton startButton;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();
        startButton = GetComponentInChildren<StartButton>();
    }

    // Update is called once per frame
    void Update()
    {
        //ถ้า player กดปุ่ม start
        if(startButton.collision)
        {
            startButton.collision = false;

            Debug.Log("START");
            stateManagement.GoState(2);
        }

        //ถ้า player กดปุ่ม score
        
        //ถ้า player กดปุ่ม exit
    }
}
