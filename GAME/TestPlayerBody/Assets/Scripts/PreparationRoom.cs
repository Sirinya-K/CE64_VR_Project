using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationRoom : MonoBehaviour
{
    public SpawnManagement spawnManagement;
    public GameObject player;
    public GameObject startWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnManagement.spawn(player, startWaypoint);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
