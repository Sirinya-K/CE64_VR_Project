using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public StateManagement stateManagement;

    public StartButton startButton;
    public ExitButton exitButton;

    private float delay;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();
        startButton = GetComponentInChildren<StartButton>();
        exitButton = GetComponentInChildren<ExitButton>();

        delay = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        //ถ้า player กดปุ่ม start
        if (startButton.collision)
        {
            startButton.collision = false;

            Invoke("StartGame", delay);
        }

        //ถ้า player กดปุ่ม exit
        if (exitButton.collision)
        {
            exitButton.collision = false;

            Invoke("ExitGame", delay);
        }
    }

    private void StartGame()
    {
        Debug.Log("[Start]");
        stateManagement.GoState(2);
    }

    private void ExitGame()
    {
        Debug.Log("[Exit]");
        Application.Quit();
    }
}
