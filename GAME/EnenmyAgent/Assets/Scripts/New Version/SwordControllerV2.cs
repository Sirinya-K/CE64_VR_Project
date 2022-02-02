using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControllerV2 : MonoBehaviour
{
    public Team1 teamId;
    [HideInInspector]
    public ArenaEnvControllerV2 envController;
    public GameObject weapon;
    Collider weaponCollider;

    public EnemyAgentV2 enemy;
    bool isAttack = false;
    bool isAction = false;
    bool isBlock = false;

    // Start is called before the first frame update
    void Start()
    {
        envController = GetComponentInParent<ArenaEnvControllerV2>();
        enemy = GetComponentInParent<EnemyAgentV2>();
        weaponCollider = weapon.GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemy.tmpAttack)
        {
            isAction = true;
        }
        if (!enemy.tmpAttack && isAction)
        {
            isAction = false;
            if(isAttack)
            {
                Debug.Log(envController.currentTeam);
                envController.ResolveEvent(Event1.HitEnemy);
                isAttack = false;
            }
            else envController.ResolveEvent(Event1.DontHitEnemy);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // ! BLOCK CONDITION DONT FORGET TO CHECK THAT ENEMY HAS PLAY BLOCK ANIMATION!!
        if (teamId == Team1.Blue && enemy.tmpAttack)
        {
            if (collision.gameObject.CompareTag("purpleAgent"))
            {
                envController.UpdateEnemySide(teamId);
                isAttack = true;
            }
            if(collision.gameObject.CompareTag("purpleShield"))
            {
                isBlock = true;
            }
        }
        else if (teamId == Team1.Purple && enemy.tmpAttack)
        {
            if (collision.gameObject.CompareTag("blueAgent"))
            {
                envController.UpdateEnemySide(teamId);
                isAttack = true;
            }
            if(collision.gameObject.CompareTag("blueShield"))
            {
                isBlock = true;
            }
        }
    }
}
