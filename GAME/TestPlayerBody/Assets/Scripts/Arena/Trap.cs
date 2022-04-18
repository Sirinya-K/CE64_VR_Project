using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public AudioSource trapSound;
    public AudioSource warningSound;

    private GameObject warmLight;
    private GameObject trap;
    private BoxCollider box;

    private float currentTime, timeStamp, countTime;
    private float x1 = -7.64f, x2 = 7.64f, y = 0.25f, z1 = -7.64f, z2 = 8.68f;
    // private float x1 = 41.76f, x2 = 59.41f, y = 14.1f, z1 = -38.26f, z2 = -53.44f;

    private int dmg = 5;
    private int state = 0;
    private bool onHit = true;

    private bool activeState;

    // Start is called before the first frame update
    void Start()
    {
        warmLight = gameObject.transform.Find("WarningLight").gameObject;
        trap = gameObject.transform.Find("TrapModel").gameObject;
        box = GetComponent<BoxCollider>();

        warmLight.SetActive(false);
        trap.SetActive(false);
        box.enabled = false;

        trapSound = transform.Find("TrapSound").GetComponent<AudioSource>();
        warningSound = transform.Find("WarningSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!activeState) return;

        currentTime += Time.deltaTime;
        // Debug.Log("current time: " + currentTime + ", timestamp: " + timeStamp);

        if (((int)currentTime) - ((int)timeStamp) == 3 && state == 0) //active: Warning Light (default = 20)
        {
            state = 1;
            warmLight.SetActive(true);
            var randomX = Random.Range(x1, x2);
            var randomZ = Random.Range(z1, z2);
            gameObject.transform.localPosition = new Vector3(randomX, y, randomZ);
            warningSound.Play();
        }
        else if (((int)currentTime) - ((int)timeStamp) == 5 && state == 1) //active: Trap (default = 22)
        {
            state = 2;
            warmLight.SetActive(false);
            trap.SetActive(true);
            box.enabled = true;
            warningSound.Stop();
            trapSound.Play();
        }
        else if (((int)currentTime) - ((int)timeStamp) == 10) //inactive: Trap (default = 30)
        {
            state = 0;
            trap.SetActive(false);
            box.enabled = false;
            timeStamp = currentTime;
            trapSound.Stop();
        }
    }

    public void active()
    {
        activeState = true;
    }

    public void inactive()
    {
        warmLight.SetActive(false);
        trap.SetActive(false);
        box.enabled = false;
        currentTime = 0f;
        timeStamp = 0f;
        state = 0;

        activeState = false;

        warningSound.Stop();
        trapSound.Stop();
    }

    public int GetDmg()
    {
        return dmg;
    }

    public void SetDmg(int newDmg)
    {
        dmg = newDmg;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        Debug.Log(other.gameObject.name + " Coliision with " + gameObject.name);

        countTime = 0f;
        onHit = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        countTime += Time.deltaTime;
        
        if(((int)countTime) == 0 && onHit)
        {
            FindObjectOfType<Player>().TakeDamage(dmg);
            onHit = false;
        }
        else if(((int)countTime) == 2 && !onHit)
        {
            countTime = 0f;
            onHit = true;
        }
    }
}
