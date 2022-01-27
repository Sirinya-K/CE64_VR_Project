using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arena : MonoBehaviour
{
    public StateManagement stateManagement;

    public GameObject playerObj;
    public Player player;
    public GameObject enemyObj;
    public Enemy enemy;

    public SpawnManagement spawnManagement;
    public GameObject enemyWaypoint;

    private bool enemySpawn;

    // Start is called before the first frame update
    void Start()
    {
        enemyObj.SetActive(true);
        enemy = enemyObj.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemySpawn)
        {
            spawnManagement.spawn(enemyObj, enemyWaypoint);
            enemySpawn = true;
        }

        //ถ้าศัตรูตาย
        if(enemy.getHealth() <= 0)
        {
            enemyObj.SetActive(true);
            //นับถอยหลัง 3 วิ แล้วไปยัง state ถัดไป
            StartCoroutine(DelayThreeSec());
        }

        //ถ้าผู้เล่นตาย
        if(player.getHealth() <= 0)
        {
            stateManagement.ChooseState(0);
        }
    }

    private IEnumerator DelayThreeSec()
    {
        yield return new WaitForSeconds(3);
        stateManagement.GoNextState();

        this.gameObject.SetActive(false);
        enemySpawn = false;
    }
}
