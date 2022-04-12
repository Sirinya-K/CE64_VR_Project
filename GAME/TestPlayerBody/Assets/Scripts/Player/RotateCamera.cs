using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public MqttProtocol mqtt;
    public CharacterController character;
    public GameObject playerCamera;

    private float sec;
    private bool left = false, right = false;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        int rotateData = 5;

        // hl:1+6 6
        if ("hl" == mqtt.data.Split(':')[0])
        {
            string data = mqtt.data.Split(':')[1];
            string mode = data.Split('+')[0];
            if (mode == "1") // Rotate Character
            {
                rotateData = int.Parse(data.Split('+')[1].Split(' ')[1]);
            }
        }

        if (Input.GetKey(KeyCode.R) || rotateData > 6)
        {
            RotateLeft();
            left = true;
        }
        else if (Input.GetKey(KeyCode.T) || rotateData < 4)
        {
            RotateRight();
            right = true;
        }
        else
        {
            left = false;
            right = false;
        }
    }

    void RotateLeft()
    {
        if(left == true)
        {
            sec += Time.deltaTime;

            if(sec >= 0.6f)
            {
                transform.RotateAround(new Vector3(playerCamera.transform.position.x, 0, playerCamera.transform.position.z), Vector3.up, -45.0f);
                sec = 0f;
            }
        }
        else
        {
            transform.RotateAround(new Vector3(playerCamera.transform.position.x, 0, playerCamera.transform.position.z), Vector3.up, -45.0f);
            sec = 0f;
        }
    }

    void RotateRight()
    {
        if(right == true)
        {
            sec += Time.deltaTime;

            if(sec >= 0.6f)
            {
                transform.RotateAround(new Vector3(playerCamera.transform.position.x, 0, playerCamera.transform.position.z), Vector3.up, 45.0f);
                sec = 0f;
            }
        }
        else
        {
            transform.RotateAround(new Vector3(playerCamera.transform.position.x, 0, playerCamera.transform.position.z), Vector3.up, 45.0f);
            sec = 0f;
        }
    }

    public void ResetRotation()
    {
        float currentR = transform.eulerAngles.y;
        Debug.Log("CurrentR: " + currentR);
        transform.RotateAround(new Vector3(playerCamera.transform.position.x, 0, playerCamera.transform.position.z), Vector3.up, -currentR);
    }
}
