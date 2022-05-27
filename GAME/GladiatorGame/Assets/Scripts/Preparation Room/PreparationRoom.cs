using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationRoom : MonoBehaviour
{
    public StateManagement stateManagement;
    public Text stageNumber;
    public GameObject enemyFirstType, enemySecondType;
    public Player player;
    public GameObject theRoof;

    public bool enter = false;

    private ArenaEntrance arenaEntrance;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();
        arenaEntrance = GetComponentInChildren<ArenaEntrance>();

        theRoof.SetActive(true);
    }

    // Update is called once per fram e
    void FixedUpdate()
    {
        //โชว์จำนวน stage ที่ผู้เล่นเคลียร์แล้วในปัจจุบัน
        stageNumber.text = player.getLevel().ToString();

        //แสดงศัตรูที่มีโอกาสเจอ
        if(player.getLevel() == 0)
        {
            enemyFirstType.SetActive(true);
            enemySecondType.SetActive(false);
        }
        else if(player.getLevel() >= 1 && player.getLevel() <= 3)
        {
            enemyFirstType.SetActive(false);
            enemySecondType.SetActive(true);
        }

        //เมื่อผู้เล่นยืนหน้าประตู
        if (arenaEntrance.collision && enter == true)
        {
            //เช็คว่าหยิบอาวุธหรือยัง
            if (player.GetCurrentWeapon() != null)
            {
                if (player.GetCurrentWeapon().name == "PlayerSword" || player.GetCurrentWeapon().name == "PlayerHammer" || player.GetCurrentWeapon().name == "PlayerSpear")
                {
                    arenaEntrance.collision = false;

                    Debug.Log("GO TO ARENA");
                    stateManagement.GoState(3);

                    enter = false;
                }
            }


        }
    }
}
