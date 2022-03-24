using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmMenu : MonoBehaviour
{
    public ExitToMainMenuButton exit;
    public BackToPreparationRoomButton back;
    public StateManagement stateManagement;

    private float delay;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();

        delay = 0.2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // เมื่อ player กดปุ่ม Back Button
        if (back.collision && stateManagement.GetState() == 3)
        {
            if (back.targetObj.CompareTag("LeftFinger"))
            {
                back.collision = false;

                Invoke("Back", delay);
            }
        }

        // เมื่อ player กดปุ่ม Exit Button
        if (exit.collision && stateManagement.GetState() != 1)
        {
            if (exit.targetObj.CompareTag("LeftFinger"))
            {
                exit.collision = false;

                Invoke("Exit", delay);
            }
        }
    }

    private void Back()
    {
        Debug.Log("[Back To Preparation Room]");
        stateManagement.GoState(9);
    }

    private void Exit()
    {
        Debug.Log("[Exit To Main Menu]");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
