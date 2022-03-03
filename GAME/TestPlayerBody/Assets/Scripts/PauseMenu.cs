using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject playerEyes;
    public GameObject pauseMenu;
    public ResumeButton resumeButton;

    private bool gamePause = false;

    private RaycastHit hitTraget;
    public GameObject pointer;

    void Start()
    {
        playerEyes = GameObject.Find("VR Rig");
        pauseMenu = GameObject.Find("PauseCanvas");
        resumeButton = FindObjectOfType<ResumeButton>();
    }

    void Update()
    {
        if (gamePause)
        {
            if (resumeButton.collision)
            {
                Debug.Log("กดแล้วจ้า");
                resumeButton.collision = false;
                Resume();
            }

            // if (Physics.Raycast(pointer.transform.position, hitTraget.transform.position))
            // {
            //     if (hitTraget.transform.name == "ResumeButton")
            //     {
            //         Resume();
            //         Destroy(hitTraget.transform);
            //     }
            // }

        }
    }

    public void Pause()
    {
        if (!gamePause)
        {
            Debug.Log("GAME PAUSE");
            var x = playerEyes.transform.position.x;
            var y = playerEyes.transform.position.y;
            var z = playerEyes.transform.position.z;
            pauseMenu.transform.position = new Vector3(x, y + 1.1f, z + 1.4f);

            Time.timeScale = 0f;

            gamePause = true;
        }

    }

    private void Resume()
    {
        Debug.Log("RESUME");
        pauseMenu.transform.position = new Vector3(0, 0, 0);

        Time.timeScale = 1f;

        gamePause = false;
    }
}
