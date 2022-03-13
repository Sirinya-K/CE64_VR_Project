using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public MqttProtocol mqtt;
    public Player player;
    public OrbManagement orb;

    protected int impactAtk;
    protected int slashAtk;
    protected float minAtkBias = 1.1f, maxAtkBias = 1.2f;

    private bool onShield = false, onPlayer = false;

    void Awake()
    {
        mqtt = FindObjectOfType<MqttProtocol>();
        player = FindObjectOfType<Player>();
        orb = FindObjectOfType<OrbManagement>();

        // Debug.Log(gameObject.name + "'s Awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        impactAtk = 0;
        slashAtk = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!onPlayer && other.gameObject.CompareTag("PlayerShield"))
        {
            player.PublishArmStateToMqtt("Left", "Stop");

            StopAllCoroutines();
            player.regenable = false;
            StartCoroutine(player.DelayRegenerateStamina());

            if (player.currentStamina == 0)
            {
                var finalAtk = ((int)((impactAtk + slashAtk) * Random.Range(minAtkBias, maxAtkBias))); // (impack + slash) * bias
                player.TakeDamage(finalAtk);
                Debug.Log("Collision: " + other.gameObject.name + ", Atk: " + finalAtk);
            }
            else
            {
                if (player.GetCurrentOrb() == 8) //กรณี orb ปัจจุบันของผู้เล่นเป็นอันที่ 8
                {
                    var r = Random.Range(0, 100);
                    Debug.Log("[OrbManagement] " + " Random: " + r);
                    if (r < orb.GetEffectPercent(8))
                    {
                        var finalAtk = 0;
                        player.ReduceStamina(finalAtk);
                        Debug.Log("Collision: " + other.gameObject.name + ", Atk: " + finalAtk);
                    }
                    else
                    {
                        var finalAtk = ((int)(impactAtk * Random.Range(minAtkBias, maxAtkBias))); // impack * bias
                        player.ReduceStamina(finalAtk);
                        Debug.Log("Collision: " + other.gameObject.name + ", Atk: " + finalAtk);
                    }
                }
                else
                {
                    var finalAtk = ((int)(impactAtk * Random.Range(minAtkBias, maxAtkBias))); // impack * bias
                    player.ReduceStamina(finalAtk);
                    Debug.Log("Collision: " + other.gameObject.name + ", Atk: " + finalAtk);
                }
            }
        }
        else if (!onShield && other.gameObject.CompareTag("Player"))
        {
            if (player.GetCurrentOrb() == 8) //กรณี orb ปัจจุบันของผู้เล่นเป็นอันที่ 8
            {
                var r = Random.Range(0, 100);
                Debug.Log("[OrbManagement] " + " Random: " + r);
                if (r < orb.GetEffectPercent(8))
                {
                    var finalAtk = 0;
                    player.TakeDamage(finalAtk);
                    Debug.Log("Collision: " + other.gameObject.name + ", Atk: " + finalAtk);
                }
                else
                {
                    var finalAtk = ((int)((impactAtk + slashAtk) * Random.Range(minAtkBias, maxAtkBias))); // (impack + slash) * bias
                    player.TakeDamage(finalAtk);
                    Debug.Log("Collision: " + other.gameObject.name + ", Atk: " + finalAtk);
                }
            }
            else
            {
                var finalAtk = ((int)((impactAtk + slashAtk) * Random.Range(minAtkBias, maxAtkBias))); // (impack + slash) * bias
                player.TakeDamage(finalAtk);
                Debug.Log("Collision: " + other.gameObject.name + ", Atk: " + finalAtk);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerShield"))
        {
            onShield = true;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            onPlayer = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        player.PublishArmStateToMqtt("Left", "Free");

        if (other.gameObject.CompareTag("PlayerShield"))
        {
            onShield = false;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            onPlayer = false;
        }
    }
}
