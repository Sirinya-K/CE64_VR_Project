using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Team teamId;

    [HideInInspector]
    public ArenaEnvController envController;
    public GameObject weapon;
    Collider weaponCollider;

    // Start is called before the first frame update
    private void Start()
    {
        envController = GetComponentInParent<ArenaEnvController>();
        weaponCollider = weapon.GetComponent<Collider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(teamId == Team.Blue)
        {
            if (collision.gameObject.CompareTag("purpleAgent"))
            {
                envController.ResolveEvent(Event.HitPurpleBody);
            }
        }
        if (teamId == Team.Purple)
        {
            if (collision.gameObject.CompareTag("blueAgent"))
            {
                envController.ResolveEvent(Event.HitBlueBody);
            }
        }
    }
}
