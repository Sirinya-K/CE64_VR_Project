using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmMenu : MonoBehaviour
{
    public ExitToMainMenuButton exit;
    public BackToPreparationRoomButton back;
    public StateManagement stateManagement;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // เมื่อ player กดปุ่ม Back Button
        if (back.collision && stateManagement.GetState() == 3)
        {
            if (back.targetObj.CompareTag("LeftFinger"))
            {
                Debug.Log("Collision Back Button");

                stateManagement.GoState(9);

                back.collision = false;
            }
        }

        // เมื่อ player กดปุ่ม Exit Button
        if (exit.collision && stateManagement.GetState() != 1)
        {
            if (exit.targetObj.CompareTag("LeftFinger"))
            {
                Debug.Log("Collision Exit Button");

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                exit.collision = false;
            }
        }
    }
}
