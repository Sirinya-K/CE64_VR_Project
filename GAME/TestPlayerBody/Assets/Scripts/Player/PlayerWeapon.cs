using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public AudioSource stabSound;
    public AudioSource swingSoftSound;
    public AudioSource swingHardSound;

    public Player player;
    public ContinuousMovement cMovement;

    protected int impactAtk;
    protected int slashAtk;
    protected int criR = 10;
    protected int criD = 50;
    protected float speed; //ตีแบบออกแรงปกติจะอยู่ที่ 5.0 ~ 8.0
    protected float speedBias = 0.2f;
    protected Vector3 lastPosition;

    private bool onShield = false, onEnemy = false, enableSoft = false, enableHard = false;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        cMovement = FindObjectOfType<ContinuousMovement>();

        stabSound = transform.Find("WeaponSounds/Stab").GetComponent<AudioSource>();
        swingSoftSound = transform.Find("WeaponSounds/Swing (Soft)").GetComponent<AudioSource>();
        swingHardSound = transform.Find("WeaponSounds/Swing (Hard)").GetComponent<AudioSource>();

        lastPosition = transform.position;

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
        CalculateSpeed();

        // [Swing Sound]
        // Debug.Log(gameObject.name + " | Player's Weapon: " + speed);

        if (((speed >= 5.5f && cMovement.moving) || (speed >= 3f && !cMovement.moving)) && (onEnemy == false && onShield == false) && enableSoft == true)
        {
            swingHardSound.Play();
            enableSoft = false;
            enableHard = false;
        }
        else if (((speed >= 4f && cMovement.moving) || (speed >= 1.5f && !cMovement.moving)) && (onEnemy == false && onShield == false) && enableHard == true)
        {
            swingSoftSound.Play();
            enableHard = false;
        }
        // else if (speed > 0f && ((speed < 1.8f && cMovement.moving) || (speed <= 0.4f && !cMovement.moving)) && enableSound == false)
        else if (speed > 0f && ((speed < 1.8f && cMovement.moving) || (speed <= 0.4f && !cMovement.moving)))
        {
            if (enableSoft == false)
                enableSoft = true;
            if (enableHard == false)
                enableHard = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // player.PublishArmStateToMqtt("Right","Stop");

        // Debug.Log("Sword Speed: " + speed);

        var finalImpactAtk = ((int)(impactAtk * speed * speedBias));
        var finalSlashAtk = ((int)(slashAtk * speed * speedBias));
        finalSlashAtk = Critical(finalSlashAtk, criR, criD);

        if (!onEnemy && other.gameObject.CompareTag("EnemyShield"))
        {
            var finalAtk = finalImpactAtk;
            other.gameObject.GetComponentInParent<Enemy>().TakeDamage(finalAtk);
            Debug.Log("Collision: " + other.gameObject.tag + ", Atk: " + finalAtk);
            stabSound.Play();
        }
        else if (!onShield && other.gameObject.CompareTag("Enemy"))
        {
            var finalAtk = finalImpactAtk + finalSlashAtk;
            other.gameObject.GetComponentInParent<Enemy>().TakeDamage(finalAtk);
            Debug.Log("Collision: " + other.gameObject.tag + ", Atk: " + finalAtk);
            stabSound.Play();
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
        // player.PublishArmStateToMqtt("Right","Free");

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

    public int GetImpactAtk()
    {
        return impactAtk;
    }

    public void SetImpactAtk(int newAtk)
    {
        impactAtk = newAtk;
    }

    public int GetSlashAtk()
    {
        return slashAtk;
    }

    public void SetSlashAtk(int newAtk)
    {
        slashAtk = newAtk;
    }

    public int GetCriR()
    {
        return criR;
    }

    public void SetCriR(int newCriR)
    {
        criR = newCriR;
    }

    public int GetCriD()
    {
        return criD;
    }

    public void SetCriD(int newCriD)
    {
        criD = newCriD;
    }
}
