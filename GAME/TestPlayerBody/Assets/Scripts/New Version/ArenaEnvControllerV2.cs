using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team1
{
    Blue = 0,
    Purple = 1,
    Default = 2
}
public enum Event1
{
    HitBlueEnemy = 0,
    HitPurpleEnemy = 1,
    BlueDontHitEnemy = 2,
    PurpleDontHitEnemy = 3,
    CanBlock = 4,
    OutOfRange = 5,
    BlueHitWall = 6,
    PurpleHitWall = 7
}
public class ArenaEnvControllerV2 : MonoBehaviour
{
    // ! If refactor complete dont forget to change from Team1 => Team !!!
    // ! PLEASE RECHECK ALL CONDITION !!!
    ArenaSettings arenaSettings;

    public EnemyAgentV2 blueAgent;
    public EnemyAgentV2 purpleAgent;

    public List<EnemyAgentV2> AgentsList = new List<EnemyAgentV2>();
    public List<SwordControllerV2> SwordsList = new List<SwordControllerV2>();

    Rigidbody blueAgentRb;
    Rigidbody purpleAgentRb;

    [HideInInspector]
    public Team1 currentTeam;
    [HideInInspector]
    public Team1 currentOppositeTeam;

    [HideInInspector]
    public int resetTimer;

    public int MaxEnvironmentSteps;

    [HideInInspector]
    public float blueScore;
    [HideInInspector]
    public float purpleScore;

    // int countBlueHit = 0;
    // int countPurpleHit = 0;

    float agentRot;

    void Start()
    {
        blueAgentRb = blueAgent.GetComponent<Rigidbody>();
        purpleAgentRb = purpleAgent.GetComponent<Rigidbody>();

        arenaSettings = FindObjectOfType<ArenaSettings>();

        currentTeam = Team1.Default;

        // ResetScene();
    }
    public void UpdateEnemySide(Team1 team)
    {
        currentTeam = team;
    }
    public void UpdateEnemyOpposite(Team1 team)
    {
        currentOppositeTeam = team;
    }
    public void ResolveEvent(Event1 triggerEvent)
    {
        switch (triggerEvent)
        {
            case Event1.HitBlueEnemy:
                // * Agent can hit enemy will +2 reward and enemy get -1 (cause we have priority on attack enemy)        
                Debug.Log("PURPLE HIT BUT DISTANCE NOT MATCH");
                // ! DONT FORGET TO CHANGE POLICY
                purpleAgent.AddReward(2f);
                blueAgent.AddReward(-1f);

                // countPurpleHit++;
                // if (countPurpleHit == 5)
                // {
                //     Debug.Log("PURPLE WIN!");
                //     purpleAgent.AddReward(1f);
                //     countPurpleHit = 0;
                //     blueAgent.EndEpisode();
                //     purpleAgent.EndEpisode();

                //     // ResetScene();
                // }
                break;
            case Event1.HitPurpleEnemy:
                // * Agent can hit enemy will +2 reward and enemy get -1 (cause we have priority on attack enemy)        
                Debug.Log("BLUE HIT BUT DISTANCE NOT MATCH");
                // ! DONT FORGET TO CHANGE POLICY
                blueAgent.AddReward(2f);
                purpleAgent.AddReward(-1f);

                // countBlueHit++;
                // if (countBlueHit == 5)
                // {
                //     Debug.Log("BLUE WIN!");
                //     blueAgent.AddReward(1f);
                //     countBlueHit = 0;
                //     blueAgent.EndEpisode();
                //     purpleAgent.EndEpisode();

                //     // ResetScene();
                // }
                break;
            case Event1.BlueDontHitEnemy:
                // * Agent attack but dont hit enemy will get -0.5 reward
                Debug.Log("Blue Dont hit");
                blueAgent.AddReward(-.5f);
                break;
            case Event1.PurpleDontHitEnemy:
                // * Agent attack but dont hit enemy will get -0.5 reward
                Debug.Log("Purple Dont hit");
                purpleAgent.AddReward(-.5f);
                break;
            case Event1.CanBlock:
                // * Agent can block attack using shield will get +1 reward
                if (Team1.Blue == currentOppositeTeam)
                {
                    Debug.Log("BLUE CAN BLOCK");
                    blueAgent.AddReward(.5f);
                }
                else if (Team1.Purple == currentOppositeTeam)
                {
                    Debug.Log("PURPLE CAN BLOCK");
                    purpleAgent.AddReward(.5f);
                }
                break;
            case Event1.OutOfRange:
                // * Agent have distance to each other more than ... will get -1 reward for both
                if (Vector3.Distance(blueAgentRb.transform.position, purpleAgentRb.transform.position) > 20f)
                {
                    blueAgent.AddReward(-1);
                    purpleAgent.AddReward(-1);
                    // ResetScene();
                }
                break;
            case Event1.BlueHitWall:
                // * Agent ran and hit a wall will get -0.5 reward
                Debug.Log("BLUE HIT WALL");
                blueAgent.AddReward(-.5f);
                break;
            case Event1.PurpleHitWall:
                // * Agent ran and hit a wall will get -0.5 reward
                Debug.Log("PURPLE HIT WALL");
                purpleAgent.AddReward(-.5f);
                break;
        }
    }
    void FixedUpdate()
    {
        // resetTimer += 1;
        // blueAgent.AddReward(-.001f);
        // purpleAgent.AddReward(-.001f);
        // if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        // {
        //     blueScore = blueAgent.GetCumulativeReward();
        //     purpleScore = purpleAgent.GetCumulativeReward();
        //     blueAgent.EpisodeInterrupted();
        //     purpleAgent.EpisodeInterrupted();
        //     // ResetScene();
        // }
    }
    public void ResetScene()
    {
        resetTimer = 0;
        currentTeam = Team1.Default; // reset last hitter
        foreach (var agent in AgentsList)
        {
            if (agent.CompareTag("purpleAgent"))
            {
                agentRot = 1;
            }
            else if (agent.CompareTag("blueAgent"))
            {
                agentRot = -1;
            }
            // randomise starting positions and rotations
            var randomPosX = Random.Range(-5f, 5f);
            var randomPosZ = Random.Range(1f * agentRot, 3f * agentRot);
            var randomRot = Random.Range(-90f, 90f);
            agent.transform.localPosition = new Vector3(randomPosX, 0, randomPosZ);
            agent.transform.eulerAngles = new Vector3(0, randomRot, 0);
            agent.GetComponent<Rigidbody>().velocity = default(Vector3);
        }
        foreach (var sword in SwordsList)
        {
            sword.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
