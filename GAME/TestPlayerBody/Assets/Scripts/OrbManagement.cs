using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbManagement : MonoBehaviour
{
    public Material green;
    public Material blue;
    public Material red;
    public Material defaultColor;

    public Player player;
    public PlayerWeapon playerWeapon;
    public Enemy enemy;
    public Trap trap;
    public ArenaSettings arenaSettings;

    private string[,] orbs = new string[,] { { "ฟื้นฟูพลังชีวิต 20%", "Green", "20", "0" }, { "เพิ่มพลังชีวิตสูงสุดขึ้น 15%", "Green", "15", "2" }, { "เพิ่มพลังกายสูงสุดขึ้น 10%", "Green", "10", "2" },
                                            { "เพิ่มพลังโจมตี 10% ใน 15 วินาทีแรก", "Blue", "10", "1" }, { "เพิ่มอัตราคริ 5%", "Blue", "5", "2" }, { "เพิ่มความเสียหายเมื่อติดคริ 10%", "Blue", "10", "2" },
                                            { "ลดความเร็วการเคลื่อนที่ของศัตรูลง 10%", "Red", "10", "1" }, { "ลดความเสียหายของกับดักลง 20%", "Red", "20", "1" }, { "เมื่อพลังกายของผู้เล่นมากกว่า 0 จะมีโอกาส 15% ที่ศัตรูจะโจมตีพลาด", "Red", "15%", "1" }, 
                                            { " ", " ", " ", " " } };

    private int defaultImpactAtk = 0, defaultSlashAtk = 0, defaultTrapDmg = 0; // 3 3 7
    private float defaultEnemySpeed = 0; // 6
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerWeapon = FindObjectOfType<PlayerWeapon>();
        enemy = FindObjectOfType<Enemy>();
        trap = FindObjectOfType<Trap>();
        arenaSettings = FindObjectOfType<ArenaSettings>();
    }

    public void Show(int num, string name)
    {
        GameObject theOrb = GameObject.Find(name);
        Text theOrbDesc = theOrb.GetComponentInChildren<Text>();

        if (orbs[num, 1] == "Green")
        {
            theOrb.GetComponent<MeshRenderer>().material = green;
        }
        else if (orbs[num, 1] == "Blue")
        {
            theOrb.GetComponent<MeshRenderer>().material = blue;
        }
        else if (orbs[num, 1] == "Red")
        {
            theOrb.GetComponent<MeshRenderer>().material = red;
        }

        theOrbDesc.text = orbs[num, 0];
    }

    public void ImplementEffect(int num)
    {
        if(num == 9) return;

        Debug.Log((float)GetEffectPercent(num));

        var effectPercemt = (float)GetEffectPercent(num); //ex. 20
        var effectValue = effectPercemt / 100f; //ex. 0.2

        if (num == 0) //ฟื้นฟูพลังชีวิต 20%
        {
            var current = player.GetCurrentHp();
            var max = player.GetMaxHp();
            player.SetCurrentHp(((int)(current + (max * effectValue))));
        }
        if (num == 1) //เพิ่มพลังชีวิตสูงสุดขึ้น 15%
        {
            var max = player.GetMaxHp();
            player.SetMaxHp(((int)(max + (max * effectValue))));
        }
        if (num == 2) //เพิ่มพลังกายสูงสุดขึ้น 10%
        {
            var max = player.GetMaxStamina();
            player.SetMaxStamina(((int)(max + (max * effectValue))));
        }
        if (num == 3) //เพิ่มพลังโจมตี 10%
        {
            var impactAtk = playerWeapon.GetImpactAtk();
            var slashAtk = playerWeapon.GetSlashAtk();
            defaultImpactAtk = impactAtk;
            defaultSlashAtk = slashAtk;
            playerWeapon.SetImpactAtk(((int)(impactAtk + (impactAtk * effectValue))));
            playerWeapon.SetSlashAtk(((int)(slashAtk + (slashAtk * effectValue))));

            Invoke("RemoveEffect(num)", 15);
        }
        if(num == 4) //เพิ่มอัตราคริ 5%
        {
            var criR = playerWeapon.GetCriR();
            playerWeapon.SetCriR(((int)(criR + (criR * effectValue))));
        }
        if(num == 5) //เพิ่มความเสียหายเมื่อติดคริ 10%
        {
            var criD = playerWeapon.GetCriD();
            playerWeapon.SetCriD(((int)(criD + (criD * effectValue))));
        }
        if(num == 6) //ลดความเร็วการเคลื่อนที่ของศัตรูลง 10%
        {
            var speed = arenaSettings.agentRunSpeed;
            defaultEnemySpeed = speed;
            arenaSettings.agentRunSpeed -= speed * effectValue;
        }
        if(num == 7) //ลดความเสียหายของกับดักลง 20%
        {
            var dmg = trap.GetDmg();
            defaultTrapDmg = dmg;
            trap.SetDmg(((int)(dmg + (dmg * effectValue))));
        }
        if(num == 8) //เมื่อพลังกายของผู้เล่นมากกว่า 0 จะมีโอกาส 15% ที่ศัตรูจะโจมตีพลาด
        {
            //Implement ใน EnemyWeapon
        }
    }

    public void RemoveEffect(int num) // 3 6 7 8
    {
        if (num == 3) //เพิ่มพลังโจมตี 10%
        {
            playerWeapon.SetImpactAtk(defaultImpactAtk);
            playerWeapon.SetSlashAtk(defaultSlashAtk);
        }
        if(num == 6) //ลดความเร็วการเคลื่อนที่ของศัตรูลง 10%
        {
            arenaSettings.agentRunSpeed = defaultEnemySpeed;
        }
        if(num == 7) //ลดความเสียหายของกับดักลง 20%
        {
            trap.SetDmg(defaultTrapDmg);
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
            return new Color32(121,224,133,255);
        }
        else if (orbs[num, 1] == "Blue")
        {
            return new Color32(160,208,225,255);
        }
        else if (orbs[num, 1] == "Red")
        {
            return new Color32(253,149,137,255);
        }
        else
        {
            return new Color32(255,255,255,255); 
        }
    }

    public int GetEffectPercent(int num)
    {
        return int.Parse(orbs[num, 2]);
    }
}
