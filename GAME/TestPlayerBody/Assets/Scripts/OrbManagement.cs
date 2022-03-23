using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbManagement : MonoBehaviour
{
    public Material greenOut;
    public Material greenIn;
    public Material blueOut;
    public Material blueIn;
    public Material redOut;
    public Material redIn;

    public Player player;
    public PlayerWeapon playerWeapon;
    public Arena arena;
    public ArenaSettings arenaSettings;

    public bool canRemove;

    private string[,] orbs = new string[,] { { "Increase 20% HP regeneration", "Green", "20", "0" }, { "Increases player Max HP 15%", "Green", "15", "2" }, { "Increases player Max Stamina 10%", "Green", "10", "2" },
                                            { "Increases player ATK 10% in first 15 sec", "Blue", "10", "1" }, { "Increases player Cri Rate 9%", "Blue", "9", "2" }, { "Increases player Cri Damage 12%", "Blue", "12", "2" },
                                            { "Decreases enemy Speed 10%", "Red", "10", "1" }, { "Reduce Damage from traps 20%", "Red", "20", "1" }, { "When player Stamina is more than 0, There's a 15% chance of dodging Attack from enemy", "Red", "15", "1" },
                                            { " ", " ", " ", " " } };

    private int defaultImpactAtk = 0, defaultSlashAtk = 0, defaultTrapDmg = 0; // 3 3 7
    private float defaultEnemySpeed = 0; // 6

    void Start()
    {
        player = FindObjectOfType<Player>();
        playerWeapon = FindObjectOfType<PlayerWeapon>();
        arena = FindObjectOfType<Arena>();
        arenaSettings = FindObjectOfType<ArenaSettings>();
    }

    public void Show(int num, string name)
    {
        GameObject theOrb = GameObject.Find(name);
        Text theOrbDesc = theOrb.GetComponentInChildren<Text>();

        if (orbs[num, 1] == "Green")
        {
            theOrb.GetComponent<MeshRenderer>().material = greenOut;
            var inner = theOrb.transform.Find("InnerSphere");
            inner.GetComponent<MeshRenderer>().material = greenIn;
        }
        else if (orbs[num, 1] == "Blue")
        {
            theOrb.GetComponent<MeshRenderer>().material = blueOut;
            var inner = theOrb.transform.Find("InnerSphere");
            inner.GetComponent<MeshRenderer>().material = blueIn;
        }
        else if (orbs[num, 1] == "Red")
        {
            theOrb.GetComponent<MeshRenderer>().material = redOut;
            var inner = theOrb.transform.Find("InnerSphere");
            inner.GetComponent<MeshRenderer>().material = redIn;
        }

        theOrbDesc.text = orbs[num, 0];
    }

    public void ImplementEffect(int num)
    {
        if (num == 9) return;

        var effectPercemt = (float)GetEffectPercent(num); //ex. 20
        var effectValue = effectPercemt / 100f; //ex. 0.2

        if (num == 0) //ฟื้นฟูพลังชีวิต 20%
        {
            var current = player.GetCurrentHp();
            var max = player.GetMaxHp();
            player.SetCurrentHp(((int)(current + (max * effectValue))));
            Debug.Log("[OrbManagement] " + "Implement: " + num + ", CurrentHp: " + player.GetCurrentHp());
        }
        if (num == 1) //เพิ่มพลังชีวิตสูงสุดขึ้น 15%
        {
            var max = player.GetMaxHp();
            player.SetMaxHp(((int)(max + (max * effectValue))));
            Debug.Log("[OrbManagement] " + "Implement: " + num + ", MaxHp: " + player.GetMaxHp());
        }
        if (num == 2) //เพิ่มพลังกายสูงสุดขึ้น 10%
        {
            var max = player.GetMaxStamina();
            player.SetMaxStamina(((int)(max + (max * effectValue))));
            Debug.Log("[OrbManagement] " + "Implement: " + num + ", MaxStamina: " + player.GetMaxStamina());
        }
        if (num == 3) //เพิ่มพลังโจมตี 10% ใน 15 วิแรก
        {
            // var currentWeapon = player.GetCurrentWeapon().GetComponent<PlayerWeapon>();
            var currentWeapon = arena.GetPlayerCurrentWeapon().GetComponent<PlayerWeapon>();

            var impactAtk = currentWeapon.GetImpactAtk();
            var slashAtk = currentWeapon.GetSlashAtk();
            defaultImpactAtk = impactAtk;
            defaultSlashAtk = slashAtk;
            currentWeapon.SetImpactAtk(((int)(impactAtk + (impactAtk * effectValue))));
            currentWeapon.SetSlashAtk(((int)(slashAtk + (slashAtk * effectValue))));
            Debug.Log("[OrbManagement] " + currentWeapon.name + " Implement: " + num + ", Impact: " + currentWeapon.GetImpactAtk() + ", Slash: " + currentWeapon.GetSlashAtk());

            var currentLevel = player.getLevel();
            StartCoroutine(WaitThenCanRemove(num, 15, currentLevel));
        }
        if (num == 4) //เพิ่มอัตราคริ 5%
        {
            var criR = playerWeapon.GetCriR();
            var percent = GetEffectPercent(num);
            playerWeapon.SetCriR(criR + percent);
            Debug.Log("[OrbManagement] " + "Implement: " + num + ", CriR: " + playerWeapon.GetCriR());
        }
        if (num == 5) //เพิ่มความเสียหายเมื่อติดคริ 10%
        {
            var criD = playerWeapon.GetCriD();
            var percent = GetEffectPercent(num);
            playerWeapon.SetCriD(criD + percent);
            Debug.Log("[OrbManagement] " + "Implement: " + num + ", CriD: " + playerWeapon.GetCriD());
        }
        if (num == 6) //ลดความเร็วการเคลื่อนที่ของศัตรูลง 10%
        {
            var speed = arenaSettings.agentRunSpeed;
            defaultEnemySpeed = speed;
            arenaSettings.agentRunSpeed = arenaSettings.agentRunSpeed - (speed * effectValue);
            Debug.Log("[OrbManagement] " + "Implement: " + num + ", EnemySpeed: " + arenaSettings.agentRunSpeed);
        }
        if (num == 7) //ลดความเสียหายของกับดักลง 20%
        {
            for (int i = 0; i < arena.GetTotalTrap(); i++)
            {
                var theTrap = GameObject.Find("Trap" + i).GetComponent<Trap>();
                var dmg = theTrap.GetDmg();
                defaultTrapDmg = dmg;
                theTrap.SetDmg(((int)(dmg - (dmg * effectValue))));
                Debug.Log("[OrbManagement] " + "Implement: " + num + ", TrapDmg: " + theTrap.GetDmg());
            }
        }
        if (num == 8) //เมื่อพลังกายของผู้เล่นมากกว่า 0 จะมีโอกาส 15% ที่ศัตรูจะโจมตีพลาด
        {
            //Implement ใน EnemyWeapon
        }
    }

    public void RemoveEffect(int num) // 3 6 7 8
    {
        if (num == 3 && canRemove) //เพิ่มพลังโจมตี 10%
        {
            // var currentWeapon = player.GetCurrentWeapon().GetComponent<PlayerWeapon>();
            var currentWeapon = arena.GetPlayerCurrentWeapon().GetComponent<PlayerWeapon>();

            currentWeapon.SetImpactAtk(defaultImpactAtk);
            currentWeapon.SetSlashAtk(defaultSlashAtk);
            Debug.Log("[OrbManagement] " + currentWeapon.name + " Remove: " + num + ", Impact: " + currentWeapon.GetImpactAtk() + ", Slash: " + currentWeapon.GetSlashAtk());
        }
        if (num == 6) //ลดความเร็วการเคลื่อนที่ของศัตรูลง 10%
        {
            arenaSettings.agentRunSpeed = defaultEnemySpeed;
            Debug.Log("[OrbManagement] " + "Remove: " + num + ", EnemySpeed: " + arenaSettings.agentRunSpeed);
        }
        if (num == 7) //ลดความเสียหายของกับดักลง 20%
        {
            for (int i = 0; i < arena.GetTotalTrap(); i++)
            {
                var theTrap = GameObject.Find("Trap" + i).GetComponent<Trap>();
                theTrap.SetDmg(defaultTrapDmg);
                Debug.Log("[OrbManagement] " + "Remove: " + num + ", TrapDmg: " + theTrap.GetDmg());
            }
        }
    }

    IEnumerator WaitThenCanRemove(int num, float delay, int level)
    {
        yield return new WaitForSeconds(delay);
        if(level == player.getLevel()) //เช็คว่าตอนนี้ player ยังอยู่ใน stage เดิมที่ใช้ orb อยู่หรือเปล่า
        {
            canRemove = true;
            RemoveEffect(num);
        }
        
    }

    public string GetName(int num)
    {
        return orbs[num, 0];
    }

    public Color GetColor(int num)
    {
        if (orbs[num, 1] == "Green")
        {
            return new Color32(121, 224, 133, 255);
        }
        else if (orbs[num, 1] == "Blue")
        {
            return new Color32(160, 208, 225, 255);
        }
        else if (orbs[num, 1] == "Red")
        {
            return new Color32(253, 149, 137, 255);
        }
        else
        {
            return new Color32(255, 255, 255, 255);
        }
    }

    public int GetEffectPercent(int num)
    {
        return int.Parse(orbs[num, 2]);
    }
}
