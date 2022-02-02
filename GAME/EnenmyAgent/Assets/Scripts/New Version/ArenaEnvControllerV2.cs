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
    HitEnemy = 0,
    DontHitEnemy = 1,
    CanBlock = 2,
    OutOfRange = 3,
    HitWall = 4
}
public class ArenaEnvControllerV2 : MonoBehaviour
{
    // ! If refactor complete dont forget to change from Team1 => Team !!!
    // ! PLEASE RECHECK ALL CONDITION !!!
    ArenaSettings arenaSettings;

    public EnemyAgentV2 blueAgent;
    public EnemyAgentV2 purpleAgent;

    public List<EnemyAgentV2> AgentsList = new List<EnemyAgentV2>();
    List<Renderer> RenderersList = new List<Renderer>();

    Rigidbody blueAgentRb;
    Rigidbody purpleAgentRb;

    public Team1 currentTeam;

    private int resetTimer;
    public int MaxEnvironmentSteps;

    public float blueScore;
    public float purpleScore;

    float agentRot;

    void Start()
    {
        blueAgentRb = blueAgent.GetComponent<Rigidbody>();
        purpleAgentRb = purpleAgent.GetComponent<Rigidbody>();

        arenaSettings = FindObjectOfType<ArenaSettings>();

        currentTeam = Team1.Default;

        ResetScene();
    }
    public void UpdateEnemySide(Team1 team)
    {
        currentTeam = team;
    }
    public void ResolveEvent(Event1 triggerEvent)
    {
        switch (triggerEvent)
        {
            case Event1.HitEnemy:
                // * Agent can hit enemy will +2 reward and enemy get -1 (cause we have priority on attack enemy)        
                if (Team1.Blue == currentTeam)
                {
                    StartCoroutine(GoalScoredSwapGroundMaterial(arenaSettings.blueGoalMaterial, RenderersList, .5f));
                    blueAgent.AddReward(2);
                    purpleAgent.AddReward(-1);
                }
                else if (Team1.Purple == currentTeam)
                {
                    StartCoroutine(GoalScoredSwapGroundMaterial(arenaSettings.purpleGoalMaterial, RenderersList, .5f));
                    purpleAgent.AddReward(2);
                    blueAgent.AddReward(-1);
                }
                ResetScene();
                break;
            case Event1.DontHitEnemy:
                // * Agent attack but dont hit enemy will get -0.5 reward
                if (Team1.Blue == currentTeam)
                {
                    blueAgent.AddReward(-.5f);
                }
                else if (Team1.Purple == currentTeam)
                {
                    purpleAgent.AddReward(-.5f);
                }
                break;
            case Event1.CanBlock:
                // * Agent can block attack using shield will get +1 reward
                if (Team1.Blue == currentTeam)
                {
                    blueAgent.AddReward(1);
                }
                else if (Team1.Purple == currentTeam)
                {
                    purpleAgent.AddReward(1);
                }
                break;
            case Event1.OutOfRange:
                // * Agent have distance to each other more than ... will get -1 reward for both
                if (Vector3.Distance(blueAgentRb.transform.position, purpleAgentRb.transform.position) > 20f)
                {
                    blueAgent.AddReward(-1);
                    purpleAgent.AddReward(-1);
                    ResetScene();
                }
                break;
            case Event1.HitWall:
                // * Agent ran and hit a wall will get -0.5 reward
                if (Team1.Blue == currentTeam)
                {
                    blueAgent.AddReward(-.5f);
                }
                else if (Team1.Purple == currentTeam)
                {
                    purpleAgent.AddReward(-.5f);
                }
                break;
        }
    }

    // * Swap color to color of win team 
    // ! CHANGE COLOR OF FLOOR STILL BUG!!
    IEnumerator GoalScoredSwapGroundMaterial(Material mat, List<Renderer> rendererList, float time)
    {
        foreach (var renderer in rendererList)
        {
            renderer.material = mat;
        }

        yield return new WaitForSeconds(time); // wait for 2 sec

        foreach (var renderer in rendererList)
        {
            renderer.material = arenaSettings.defaultMaterial;
        }

    }
    void FixedUpdate()
    {
        resetTimer += 1;
        if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            if (blueAgent.GetCumulativeReward() > purpleAgent.GetCumulativeReward())
            {
                blueAgent.AddReward(2f);
            }
            if (blueAgent.GetCumulativeReward() < purpleAgent.GetCumulativeReward())
            {
                purpleAgent.AddReward(2f);
            }
            blueScore = blueAgent.GetCumulativeReward();
            purpleScore = purpleAgent.GetCumulativeReward();
            blueAgent.EndEpisode();
            purpleAgent.EndEpisode();
            ResetScene();
        }
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
            var randomPosX = Random.Range(-1f, 1f);
            var randomPosZ = Random.Range(2f * agentRot, 5f * agentRot);
            var randomRot = Random.Range(-45f, 45f);
            agent.transform.localPosition = new Vector3(randomPosX, 0, randomPosZ);
            agent.transform.eulerAngles = new Vector3(0, randomRot, 0);
            agent.GetComponent<Rigidbody>().velocity = default(Vector3);
        }
    }
}
