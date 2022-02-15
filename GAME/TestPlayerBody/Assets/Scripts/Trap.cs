using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private GameObject warmLight;
    private GameObject trap;
    private BoxCollider box;

    private float currentTime, timeStamp, countTime;
    private float x1 = 41.76f, x2 = 59.41f, y = 14.1f, z1 = -38.26f, z2 = -53.44f;

    private int trapDamage = 50;
    private int state = 0;
    private bool onHit = true;

    private bool activeState;

    // Start is called before the first frame update
    public void Start()
    {
        warmLight = gameObject.transform.Find("WarningLight").gameObject;
        trap = gameObject.transform.Find("TrapModel").gameObject;
        box = GetComponent<BoxCollider>();

        warmLight.SetActive(false);
        trap.SetActive(false);
        box.enabled = false;
    }

    // Update is called once per frame
    public void FixedUpdate()
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
        }
        else if (((int)currentTime) - ((int)timeStamp) == 5 && state == 1) //active: Trap (default = 22)
        {
            state = 2;
            warmLight.SetActive(false);
            trap.SetActive(true);
            box.enabled = true;
        }
        else if (((int)currentTime) - ((int)timeStamp) == 10) //inactive: Trap (default = 30)
        {
            state = 0;
            trap.SetActive(false);
            box.enabled = false;
            timeStamp = currentTime;
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("PlayerBody")) return;
        
        Debug.Log(other.gameObject.name + " Coliision with " + gameObject.name);

        countTime = 0f;
        onHit = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("PlayerBody")) return;

        countTime += Time.deltaTime;
        
        if(((int)countTime) == 0 && onHit)
        {
            FindObjectOfType<Player>().TakeDamage(trapDamage);
            onHit = false;
        }
        else if(((int)countTime) == 2 && !onHit)
        {
            countTime = 0f;
            onHit = true;
        }
    }
}
