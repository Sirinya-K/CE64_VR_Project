using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationRoom : MonoBehaviour
{
    public StateManagement stateManagement;
    public Text stateNumber;
    public Player player;
    public GameObject theRoof;

    private ArenaEntrance arenaEntrance;

    // Start is called before the first frame update
    void Start()
    {
        stateManagement = FindObjectOfType<StateManagement>();
        arenaEntrance = GetComponentInChildren<ArenaEntrance>();

        theRoof.SetActive(true);
    }

    // Update is called once per fram e
    void Update()
    {
        //โชว์เลข state ปัจจุบัน
        stateNumber.text = player.getLevel().ToString();

        //เมื่อผู้เล่นยืนหน้าประตู
        if(arenaEntrance.collision)
        {
            //เช็คว่าหยิบอาวุธหรือยัง
            if(player.GetCurrentWeapon().name == "PlayerSword" || player.GetCurrentWeapon().name == "PlayerHammer" || player.GetCurrentWeapon().name == "PlayerSpear")
            {
                arenaEntrance.collision = false;

                Debug.Log("GO TO ARENA");
                stateManagement.GoState(3);
            }
        }
    }
}
