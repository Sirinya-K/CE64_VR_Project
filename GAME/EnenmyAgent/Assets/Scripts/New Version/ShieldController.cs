using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public Team1 teamId;
    private ArenaEnvControllerV2 envController;
    public SwordandShieldControllerV2 swordController;
    private EnemyAgentV2 enemy;
    bool isActionBlock = false;
    // Start is called before the first frame update
    void Start()
    {
        envController = GetComponentInParent<ArenaEnvControllerV2>();
        enemy = GetComponentInParent<EnemyAgentV2>();
        swordController = swordController.GetComponent<SwordandShieldControllerV2>();
    }

    void FixedUpdate()
    {
        if (enemy.tmpBlock)
        {
            isActionBlock = true;
        }
        if (!enemy.tmpBlock && isActionBlock)
        {
            swordController.oppositeEnemyAgent.enemyBlock = true;
            isActionBlock = false;
        }
    }
}
