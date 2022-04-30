using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public MqttProtocol mqtt;
    public StateManagement stateManagement;

    public StartButton startButton;
    public CalibrateButton calibrateButton;
    public ExitButton exitButton;

    private float delay;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();
        startButton = GetComponentInChildren<StartButton>();
        calibrateButton = GetComponentInChildren<CalibrateButton>();
        exitButton = GetComponentInChildren<ExitButton>();

        delay = 0.2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ถ้า player กดปุ่ม start
        if (startButton.collision)
        {
            mqtt.Publish("/cmd/","start");

            startButton.collision = false;

            Invoke("StartGame", delay);
        }

        //ถ้า player กดปุ่ม exit
        if (exitButton.collision)
        {
            mqtt.Publish("/cmd/","exit");

            exitButton.collision = false;

            Invoke("ExitGame", delay);
        }

        //ถ้า player กดปุ่ม start
        if (calibrateButton.collision)
        {
            mqtt.Publish("/hr/","C");

            calibrateButton.collision = false;
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
