using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationRoom : MonoBehaviour
{
    public GameObject player;

    public SpawnManagement spawnManagement;
    public GameObject startWaypoint;
    public GameObject arenaWaypoint;

    public ArenaEntrance arenaEntrance;

    public GameObject arena;

    // Start is called before the first frame update
    void Start()
    {
        spawnManagement.spawn(player, startWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        //เมื่อผู้เล่นยืนหน้าประตู
        if(arenaEntrance.collision == true)
        {
            //เช็คว่าหยิบอาวุธหรือยัง (ถ้าหยิบแล้ว object อาวุธ จะเป็นลูกของ object ผู้เล่น)

            spawnManagement.spawn(player, arenaWaypoint);
            arena.SetActive(true);
        }
    }
}
