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

    private bool initiateState, fightingState;

    private bool choseTheOrb;
    private int finalLevel = 4;
    private float stateDelay = 1.5f;
    private GameObject showResult;
    private Text fightResult;

    private EnemyManagement enemyManagement;
    private OrbManagement orbManagement;
    private GameObject firstOrbObj, secondOrbObj, thirdOrbObj;
    private Orb firstOrb, secondOrb, thirdOrb;
    private int firstRandomOrb, secondRandomOrb, thirdRandomOrb, currentOrbNum = 9, newOrbNum;
    private GameObject chest;

    private int totalTrap = 3;
    private GameObject trapOriginal;
    private GameObject[] traps;

    private GameObject playerCurrentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        initiateState = false;

        stateManagement = FindObjectOfType<StateManagement>();

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

        chest = GameObject.Find("Chest");
        chest.SetActive(false);

        //สร้าง Trap เตรียมไว้
        trapOriginal = GameObject.Find("Trap");
        traps = new GameObject[totalTrap];
        for (int i = 0; i < totalTrap; i++)
        {
            traps[i] = Instantiate(trapOriginal, gameObject.transform, false);
            traps[i].name = "Trap" + i.ToString();
            // traps[i].SetActive(false);
        }
        trapOriginal.SetActive(false);

        // newObject = new GameObject[3];
        // newObject[0] = new GameObject("New Gameobject1");
        // newObject[0].AddComponent<BoxCollider>();
        // newObject[0].AddComponent<Trap>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //เมื่อ active arena
        if (initiateState)
        {
            initiateState = false;

            //crate enemy via EnemyManagement class
            var currentLevel = player.getLevel();
            if (currentLevel == 0)
            {
                Debug.Log("[      Enter      ] 4");
                var randomEnemy = Random.Range(0, 1); // (0, 2) สุ่ม enemy ลำดับที่ 0 1
                theEnemyObj = enemyManagement.CreateEnemy(currentLevel, randomEnemy);
            }
            else
            {
                var randomEnemy = Random.Range(0, 1); // (2, 5) สุ่ม enemy ลำดับที่ 2 3 4
                theEnemyObj = enemyManagement.CreateEnemy(currentLevel, randomEnemy);
            }

            //inactive orbs
            firstOrbObj.SetActive(false);
            secondOrbObj.SetActive(false);
            thirdOrbObj.SetActive(false);
            //inactive chest
            chest.SetActive(false);

            //spawn the enemy
            theEnemyObj.SetActive(true);
            spawnManagement.spawn(theEnemyObj, enemyWaypoint);
            theEnemyProperty = theEnemyObj.GetComponent<Enemy>();

            //เก็บว่า player ถืออาวุธอะไรในด่านนี้
            playerCurrentWeapon = player.GetCurrentWeapon();

            //implement the orb's effect
            currentOrbNum = player.GetCurrentOrb();
            orbManagement.ImplementEffect(currentOrbNum);

            //active traps
            for (int i = 0; i < totalTrap; i++)
            {
                traps[i].GetComponent<Trap>().active();
            }

            fightingState = true;
        }

        if (fightingState)
        {
            //ถ้าศัตรูตาย
            if (theEnemyProperty.GetCurrentHp() <= 0)
            {
                //inactive traps
                for (int i = 0; i < totalTrap; i++)
                {
                    traps[i].GetComponent<Trap>().inactive();
                }

                //remove the orb's effect
                if (orbManagement.canRemove == false) orbManagement.canRemove = true;
                orbManagement.RemoveEffect(currentOrbNum);
                orbManagement.canRemove = false;

                player.levelUp();
                theEnemyObj.SetActive(false);
                theEnemyProperty.ResetEnemyStat();

                Debug.Log("WIN " + player.getLevel() + "/4");

                if (player.getLevel() == finalLevel) //ถ้าถึงด่านสุดท้าย
                {
                    Debug.Log("VICTORY");

                    player.RecordTime();
                    FinishGame();
                }
                else //ถ้ายัง ไม่ ถึงด่านสุดท้าย
                {
                    //active chest
                    chest.SetActive(true);

                    //active orbs
                    firstOrbObj.SetActive(true); // Active & Random 1st Orb
                    firstRandomOrb = Random.Range(0, 3); // มีโอกาสสุ่มเจอ 0 1 2
                    orbManagement.Show(firstRandomOrb, "FirstOrb");

                    secondOrbObj.SetActive(true); // Active & Random 2nd Orb
                    secondRandomOrb = Random.Range(3, 4); // มีโอกาสสุ่มเจอ 3 4 5
                    orbManagement.Show(secondRandomOrb, "SecondOrb");

                    thirdOrbObj.SetActive(true); // Active & Random 3rd Orb
                    thirdRandomOrb = Random.Range(6, 9); // มีโอกาสสุ่มเจอ 6 7 8
                    orbManagement.Show(thirdRandomOrb, "ThirdOrb");
                }
            }

            //ถ้าผู้เล่นตาย
            if (player.GetCurrentHp() <= 0)
            {

            }

            if (firstOrb.collision) //กรณีเลือก 1st Orb
            {
                firstOrb.collision = false;
                choseTheOrb = true;

                newOrbNum = firstRandomOrb;

                Debug.Log("Chose 1st Orb: " + orbManagement.GetName(firstRandomOrb));
            }
            else if (secondOrb.collision) //กรณีเลือก 2nd Orb
            {
                secondOrb.collision = false;
                choseTheOrb = true;

                newOrbNum = secondRandomOrb;

                Debug.Log("Chose 2nd Orb: " + orbManagement.GetName(secondRandomOrb));
            }
            else if (thirdOrb.collision) //กรณีเลือก 3rd Orb
            {
                thirdOrb.collision = false;
                choseTheOrb = true;

                newOrbNum = thirdRandomOrb;

                Debug.Log("Chose 3rd Orb: " + orbManagement.GetName(thirdRandomOrb));
            }

            //ถ้าผู้เล่นเลือก Orb แล้ว
            if (choseTheOrb)
            {
                // endtime
                FindObjectOfType<TimeCounter>().EndTimerWin();

                choseTheOrb = false;

                player.SetCurrentOrb(newOrbNum); //player เก็บ Orb ที่เลือก
                player.ShowCurrentOrb(); //แสดงผล orb ที่เลือกผ่าน UI

                //inactive orbs
                firstOrbObj.SetActive(false);
                secondOrbObj.SetActive(false);
                thirdOrbObj.SetActive(false);
                //inactive chest
                chest.SetActive(false);

                showResult.SetActive(true);
                fightResult.text = " ";

                playerCurrentWeapon = null;

                Invoke("GoNextLevel", stateDelay);
            }
        }
    }

    private void GoNextLevel()
    {
        // initiate = false;
        fightingState = false;
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

    public void StartInitiate()
    {
        initiateState = true;
    }

    public GameObject GetPlayerCurrentWeapon()
    {
        return playerCurrentWeapon;
    }

    public void SetPlayerCurrentWeapon(GameObject obj)
    {
        playerCurrentWeapon = obj;
    }

    public int GetTotalTrap()
    {
        return totalTrap;
    }
}
