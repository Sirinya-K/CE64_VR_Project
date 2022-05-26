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

    public EnemyAgent enemy;
    public bool isAttack = false;
    public bool readyCheck = false;

    // Start is called before the first frame update
    private void Start()
    {
        envController = GetComponentInParent<ArenaEnvController>();
        enemy = GetComponentInParent<EnemyAgent>();
        weaponCollider = weapon.GetComponent<Collider>();
    }
    void FixedUpdate()
    {
        if (enemy.tmpAttack)
        {
            readyCheck = true;
        }
        if (!enemy.tmpAttack && readyCheck && teamId == Team.Blue)
        {
            readyCheck = false;
            if (isAttack)
            {
                envController.ResolveEvent(Event.HitPurpleBody);
                isAttack = false;
            }
            else envController.ResolveEvent(Event.BlueDontHitEnemy);
        }
        else if (!enemy.tmpAttack && readyCheck && teamId == Team.Purple)
        {
            readyCheck = false;
            if (isAttack)
            {
                envController.ResolveEvent(Event.HitBlueBody);
                isAttack = false;
            }
            else envController.ResolveEvent(Event.PurpleDontHitEnemy);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (teamId == Team.Blue && enemy.tmpAttack)
        {
            if (collision.gameObject.CompareTag("purpleAgent"))
            {
                isAttack = true;
            }
        }
        else if (teamId == Team.Purple && enemy.tmpAttack)
        {
            if (collision.gameObject.CompareTag("blueAgent"))
            {
                isAttack = true;
            }
        }
    }
}
