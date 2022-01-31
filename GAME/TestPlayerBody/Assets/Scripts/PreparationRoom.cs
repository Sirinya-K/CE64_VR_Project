using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationRoom : MonoBehaviour
{
    public StateManagement stateManagement;
    public Text stateNumber;

    public GameObject playerObj;
    public Player player;

    public SpawnManagement spawnManagement;
    public GameObject startWaypoint;
    public GameObject arenaWaypoint;

    public ArenaEntrance arenaEntrance;

    public GameObject arena;
    
    public GameObject theRoof;

    private bool playerSpawn;

    // Start is called before the first frame update
    void Start()
    {
        theRoof.SetActive(true);
    }

    // Update is called once per fram e
    void Update()
    {
        //โชว์เลข state ปัจจุบัน
        stateNumber.text = stateManagement.GetState().ToString();

        if(!playerSpawn)
        {
            spawnManagement.spawn(playerObj, startWaypoint);
            playerSpawn = true;
        }

        //เมื่อผู้เล่นยืนหน้าประตู
        if(arenaEntrance.collision)
        {
            //เช็คว่าหยิบอาวุธหรือยัง
            if(player.theItem == "Weapon")
            {
                //activate arena
                arena.SetActive(true);
                spawnManagement.spawn(playerObj, arenaWaypoint);

                stateManagement.onPreparationRoom = false;
                stateManagement.onArena = true;

                playerSpawn = false;
                arenaEntrance.collision = false;

                this.gameObject.SetActive(false);
            }
        }
    }
}
