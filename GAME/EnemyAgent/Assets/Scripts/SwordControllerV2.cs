using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControllerV2 : MonoBehaviour
{
    public Team1 teamId;
    [HideInInspector]
    public ArenaEnvControllerV2 envController;
    // * Get from parent => from blue/purple agent
    private EnemyAgentV2 enemy;
    public GameObject weapon;
    Collider weaponCollider;
    public GameObject oppositeEnemy;
    [HideInInspector]
    public EnemyAgentV2 oppositeEnemyAgent;

    bool isAttack = false;
    bool isActionAttack = false;
    bool isHit = false;
    public bool isBlock = false;

    void Start()
    {
        envController = GetComponentInParent<ArenaEnvControllerV2>();
        enemy = GetComponentInParent<EnemyAgentV2>();
        oppositeEnemyAgent = oppositeEnemy.GetComponent<EnemyAgentV2>();
        weaponCollider = weapon.GetComponent<Collider>();
    }
    void FixedUpdate()
    {
        if (enemy.tmpAttack && !isHit)
        {
            isActionAttack = true;
            if (isAttack)
            {
                isHit = true;
                if (teamId == Team1.Blue)
                {
                    envController.ResolveEvent(Event1.HitPurpleEnemy);
                }
                else if (teamId == Team1.Purple)
                {
                    envController.ResolveEvent(Event1.HitBlueEnemy);
                }
                isAttack = false;
            }
        }
        // ! animation has end and it do animation and dont hit
        if (!enemy.tmpAttack && isActionAttack)
        {
            isActionAttack = false;
            if (!isAttack && !isHit)
            {
                if (teamId == Team1.Blue)
                {
                    envController.ResolveEvent(Event1.BlueDontHitEnemy);
                }
                else if (teamId == Team1.Purple)
                {
                    envController.ResolveEvent(Event1.PurpleDontHitEnemy);
                }
            }
        }
        if (!enemy.tmpAttack && isHit) isHit = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // ! BLOCK CONDITION DONT FORGET TO CHECK THAT ENEMY HAS PLAY BLOCK ANIMATION!!
        // if (teamId == Team1.Blue)
        // {
        //     if (collision.gameObject.CompareTag("purpleAgent"))
        //     {
        //         envController.ResolveEvent(Event1.HitPurpleEnemy);
        //         isAttack = true;
        //     }
        //     envController.UpdateEnemySide(teamId);
        // }
        // else if (teamId == Team1.Purple)
        // {
        //     if (collision.gameObject.CompareTag("blueAgent"))
        //     {
        //         envController.ResolveEvent(Event1.HitBlueEnemy);
        //         isAttack = true;
        //     }
        //     envController.UpdateEnemySide(teamId);
        // }
        if (teamId == Team1.Blue && enemy.tmpAttack)
        {
            if (collision.gameObject.CompareTag("purpleAgent"))
            {
                isAttack = true;
            }
            envController.UpdateEnemySide(teamId);
        }
        else if (teamId == Team1.Purple && enemy.tmpAttack)
        {
            if (collision.gameObject.CompareTag("blueAgent"))
            {
                isAttack = true;
            }
            envController.UpdateEnemySide(teamId);
        }
    }
}
