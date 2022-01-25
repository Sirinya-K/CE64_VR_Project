using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationRoom : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject playerModel;
    private Player player;

    public SpawnManagement spawnManagement;
    public GameObject startWaypoint;
    public GameObject arenaWaypoint;

    public ArenaEntrance arenaEntrance;

    public GameObject arena;
    
    public GameObject theRoof;

    // Start is called before the first frame update
    void Start()
    {
        player = playerModel.GetComponent<Player>();

        spawnManagement.spawn(playerObj, startWaypoint);

        theRoof.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //เมื่อผู้เล่นยืนหน้าประตู
        if(arenaEntrance.collision == true)
        {
            //เช็คว่าหยิบอาวุธหรือยัง
            if(player.theItem == "Weapon")
            {
                spawnManagement.spawn(playerObj, arenaWaypoint);
                arena.SetActive(true);
            }
        }
    }
}
