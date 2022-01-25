using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagement : MonoBehaviour
{
    public GameObject preparationRoom;
    public GameObject arena;

    private int state = 1;

    // Start is called before the first frame update
    void Start()
    {
        preparationRoom.SetActive(false);
        arena.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == 0)
        {
            
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

        }
    }
}
