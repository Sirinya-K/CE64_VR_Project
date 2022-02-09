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

    public SpawnManagement spawnManagement;
    public GameObject enemyWaypoint;

    private GameObject theEnemyObj;
    private Enemy theEnemyProperty;

    private bool initiate, choseTheOrb;
    private int finalLevel = 4;
    private float stateDelay = 1.5f;
    private GameObject showResult;
    private Text fightResult;

    private EnemyManagement enemyManagement;
    private OrbManagement orbManagement;
    private GameObject firstOrbObj, secondOrbObj, thirdOrbObj;
    private Orb firstOrb, secondOrb, thirdOrb;
    private int firstRandomOrb, secondRandomOrb, thirdRandomOrb, currentOrbNum, newOrbNum;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();
        // enemy = enemyObj.GetComponent<Enemy>();

        showResult = GameObject.Find("ResultCanvas");
        showResult.SetActive(false);
        fightResult = showResult.GetComponentInChildren<Text>();

        enemyManagement = FindObjectOfType<EnemyManagement>();

        orbManagement = FindObjectOfType<OrbManagement>();

        firstOrbObj = GameObject.Find("FirstOrb");
        firstOrbObj.SetActive(false);
        firstOrb = firstOrbObj.GetComponent<Orb>();

        secondOrbObj = GameObject.Find("SecondOrb");
        secondOrbObj.SetActive(false);
        secondOrb = secondOrbObj.GetComponent<Orb>();

        thirdOrbObj = GameObject.Find("ThirdOrb");
        thirdOrbObj.SetActive(false);
        thirdOrb = thirdOrbObj.GetComponent<Orb>();
    }

    // Update is called once per frame
    void Update()
    {
        //เมื่อ active arena
        if (!initiate)
        {
            initiate = true;

            //crate enemy via EnemyManagement class
            var currentLevel = player.getLevel();
            if(currentLevel == 0)
            {
                var randomEnemy = Random.Range(0,2); // enemy ลำดับที่ 0 1
                theEnemyObj = enemyManagement.CreateEnemy(currentLevel, randomEnemy);
            }
            else
            {
                var randomEnemy = Random.Range(2,5); // enemy ลำดับที่ 2 3 4
                theEnemyObj = enemyManagement.CreateEnemy(currentLevel, randomEnemy);
            }

            //spawn the enemy
            theEnemyObj.SetActive(true);
            spawnManagement.spawn(theEnemyObj, enemyWaypoint);

            //implement the orb's effect
            currentOrbNum = player.GetTheOrb();
            orbManagement.ImplementEffect(currentOrbNum);
        }

        //ถ้าศัตรูตาย
        if (theEnemyProperty.getHealth() <= 0 && theEnemyObj.activeSelf)
        {
            orbManagement.RemoveEffect(currentOrbNum);
            player.levelUp();
            theEnemyObj.SetActive(false);
            theEnemyProperty.ResetEnemyStat();

            Debug.Log("WIN " + player.getLevel() + "/4");

            if (player.getLevel() == finalLevel) //ถ้าถึงด่านสุดท้าย
            {
                Debug.Log("VICTORY");

                FinishGame();
            }
            else //ถ้ายัง ไม่ ถึงด่านสุดท้าย
            {
                //active orbs
                firstOrbObj.SetActive(true); // Active & Random 1st Orb
                firstRandomOrb = Random.Range(0,3); // มีโอกาสสุ่มเจอ 0 1 2
                orbManagement.Show(firstRandomOrb,"FirstOrb");

                secondOrbObj.SetActive(true); // Active & Random 2nd Orb
                secondRandomOrb = Random.Range(3,6);
                orbManagement.Show(secondRandomOrb,"SecondOrb");

                thirdOrbObj.SetActive(true); // Active & Random 3rd Orb
                thirdRandomOrb = Random.Range(6,9);
                orbManagement.Show(thirdRandomOrb,"ThirdOrb");
            }
        }

        //ถ้าผู้เล่นตาย
        if (player.getHealth() <= 0)
        {

        }

        if (firstOrb.collision) //กรณีเลือก 1st Orb
        {
            firstOrb.collision = false;
            choseTheOrb = true;

            newOrbNum = firstRandomOrb;

            Debug.Log("Chose 1st Orb: " + firstRandomOrb);
        }
        else if (secondOrb.collision) //กรณีเลือก 2nd Orb
        {
            secondOrb.collision = false;
            choseTheOrb = true;

            newOrbNum = secondRandomOrb;

            Debug.Log("Chose 2nd Orb: " + secondRandomOrb);
        }
        else if (thirdOrb.collision) //กรณีเลือก 3rd Orb
        {
            thirdOrb.collision = false;
            choseTheOrb = true;

            newOrbNum = thirdRandomOrb;

            Debug.Log("Chose 3rd Orb: " + thirdRandomOrb);
        }
        
        //ถ้าผู้เล่นเลือก Orb แล้ว
        if(choseTheOrb)
        {
            choseTheOrb = false;
            
            player.Inventory(newOrbNum); //เก็บ Orb ที่เหลือเข้ากระเป๋าของผู้เล่น

            firstOrbObj.SetActive(false);
            secondOrbObj.SetActive(false);
            thirdOrbObj.SetActive(false);

            showResult.SetActive(true);
            fightResult.text = " ";
            Invoke("GoNextLevel", stateDelay);
        }
    }

    private void GoNextLevel()
    {
        initiate = false;
        showResult.SetActive(false);
        stateManagement.GoState(2);
    }

    private void FinishGame()
    {
        showResult.SetActive(true);
        fightResult.text = "VICTORY! :D";
        Invoke("RestartGame", stateDelay);
    }

    private void GameOVer()
    {
        showResult.SetActive(true);
        fightResult.text = "GAME OVER :(";
        Invoke("RestartGame", stateDelay);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
