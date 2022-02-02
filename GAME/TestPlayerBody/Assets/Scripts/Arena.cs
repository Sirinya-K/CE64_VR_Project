using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private int finalLevel = 4;
    private float stateDelay = 2f;
    private GameObject showResult;
    private Text fightResult;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();
        enemy = enemyObj.GetComponent<Enemy>();

        showResult = GameObject.Find("ResultCanvas");
        showResult.SetActive(false);
        fightResult = showResult.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemySpawn)
        {
            enemyObj.SetActive(true);
            spawnManagement.spawn(enemyObj, enemyWaypoint);
            enemySpawn = true;
        }

        //ถ้าศัตรูตาย
        if (enemy.getHealth() <= 0)
        {
            player.levelUp();
            enemyObj.SetActive(false);
            enemy.ResetEnemyStat();
            
            Debug.Log("WIN " + player.getLevel() + "/4");

            if(player.getLevel() == finalLevel) //ถ้าถึงด่านสุดท้าย
            {
                Debug.Log("VICTORY");

                FinishGame();
            }
            else //ถ้ายัง ไม่ ถึงด่านสุดท้าย
            {
                Debug.Log("BACK TO PREPARATIONROOM");

                showResult.SetActive(true);
                fightResult.text = "LOADING . . .";
                Invoke("GoNextLevel",stateDelay);
            }
        }

        //ถ้าผู้เล่นตาย
        if (player.getHealth() <= 0)
        {
            
        }
    }

    private void GoNextLevel()
    {
        enemySpawn = false;
        showResult.SetActive(false);
        stateManagement.GoState(2);
    }

    private void FinishGame()
    {
        showResult.SetActive(true);
        fightResult.text = "VICTORY! :D";
        Invoke("RestartGame",stateDelay);
    }

    private void GameOVer()
    {
        showResult.SetActive(true);
        fightResult.text = "GAME OVER :(";
        Invoke("RestartGame",stateDelay);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
