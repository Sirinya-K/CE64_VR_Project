using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public MqttProtocol mqtt;

    protected int impactAtk;
    protected int slashAtk;
    protected int criR = 10;
    protected int criD = 50;
    private bool onShield = false, onEnemy = false;
    protected float speed; //ตีแบบออกแรงปกติจะอยู่ที่ 5.0 ~ 8.0
    protected float speedBias = 0.2f;

    protected Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        mqtt = FindObjectOfType<MqttProtocol>();

        impactAtk = 0;
        slashAtk = 0;

        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculateSpeed();
    }

    void OnTriggerEnter(Collider other)
    {
        // CheckArmState("stop");

        // if (!other.gameObject.CompareTag("Enemy")) return; //ตรวจสอบว่าที่ตีโดนใช่ enemy มั้ย

        Debug.Log("Sword Speed: " + speed);

        var finalImpactAtk = ((int)(impactAtk * speed * speedBias));
        var finalSlashAtk = ((int)(slashAtk * speed * speedBias));
        finalSlashAtk = Critical(finalSlashAtk, criR, criD);

        if (!onEnemy && other.gameObject.CompareTag("EnemyShield"))
        {
            var finalAtk = finalImpactAtk;
            other.gameObject.GetComponentInParent<Enemy>().TakeDamage(finalImpactAtk);
            Debug.Log("Collision: " + other.gameObject.tag + ", Atk: " + finalAtk);
        }
        else if (!onShield && other.gameObject.CompareTag("Enemy"))
        {
            var finalAtk = finalImpactAtk + finalSlashAtk;
            other.gameObject.GetComponentInParent<Enemy>().TakeDamage(finalAtk);
            Debug.Log("Collision: " + other.gameObject.tag + ", Atk: " + finalAtk);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyShield"))
        {
            onShield = true;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            onEnemy = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyShield"))
        {
            onShield = false;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            onEnemy = false;
        }
    }

    private void CalculateSpeed()
    {
        speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    private int Critical(int atk, int criR, int criD)
    {
        var r = Random.Range(0, 100);
        // Debug.Log("Random: " + r);

        if (r < criR)
        {
            // Debug.Log("%CriD: " + criD / 100f);
            atk = ((int)((float)atk * (1f + ((float)criD / 100f))));
            // Debug.Log("FinalAtk: " + atk);

            return atk;
        }
        else
        {
            // Debug.Log("FinalAtk: " + atk);

            return atk;
        }
    }

    private void CheckArmState(string armState)
    {
        if (armState is "stop") mqtt.Publish("/ar/", "S");
        else if (armState is "free") mqtt.Publish("/ar/", "F");
    }

}
