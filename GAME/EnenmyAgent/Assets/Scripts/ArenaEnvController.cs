using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Blue = 0,
    Purple = 1,
    Default = 2
}

public enum Event
{
    HitBlueBody = 0,
    HitPurpleBody = 1,
    OutOfRange = 2,
    HitIntoBlueArea = 3,
    HitIntoPurpleArea = 4
}

public class ArenaEnvController : MonoBehaviour
{
    int ballSpawnSide;

    ArenaSettings arenaSettings;

    public EnemyAgent blueAgent;
    public EnemyAgent purpleAgent;

    public List<EnemyAgent> AgentsList = new List<EnemyAgent>();
    List<Renderer> RenderersList = new List<Renderer>();

    Rigidbody blueAgentRb;
    Rigidbody purpleAgentRb;

    public GameObject blueGoal;
    public GameObject purpleGoal;

    Renderer blueGoalRenderer;

    Renderer purpleGoalRenderer;

    Team lastHitter;

    private int resetTimer;
    public int MaxEnvironmentSteps;

    float agentRot;

    void Start()
    {

        // Used to control agent & ball starting positions
        blueAgentRb = blueAgent.GetComponent<Rigidbody>();
        purpleAgentRb = purpleAgent.GetComponent<Rigidbody>();

        // Render ground to visualise which agent scored
        blueGoalRenderer = blueGoal.GetComponent<Renderer>();
        purpleGoalRenderer = purpleGoal.GetComponent<Renderer>();
        RenderersList.Add(blueGoalRenderer);
        RenderersList.Add(purpleGoalRenderer);

        arenaSettings = FindObjectOfType<ArenaSettings>();

        ResetScene();
    }

    /// <summary>
    /// Tracks which agent last had control of the ball
    /// </summary>
    public void UpdateLastHitter(Team team)
    {
        lastHitter = team;
    }

    /// <summary>
    /// Resolves scenarios when ball enters a trigger and assigns rewards
    /// </summary>
    public void ResolveEvent(Event triggerEvent)
    {
        switch (triggerEvent)
        {
            //can use for hit shield/sword it cant take damage 
            case Event.OutOfRange:
                if (Vector3.Distance(blueAgentRb.transform.position,purpleAgentRb.transform.position)>12f)
                {
                    blueAgent.AddReward(-1);
                    purpleAgent.AddReward(-1);
                    blueAgent.EndEpisode();
                    purpleAgent.EndEpisode();
                    ResetScene();
                }

                // end episode
                break;

            case Event.HitPurpleBody:
                // blue wins

                // turn +floor blue
                StartCoroutine(GoalScoredSwapGroundMaterial(arenaSettings.blueGoalMaterial, RenderersList, .5f));
                // Add reward
                blueAgent.AddReward(2);
                purpleAgent.AddReward(-1);

                // end episode
                // blueAgent.EndEpisode();
                // purpleAgent.EndEpisode();
                ResetScene();
                break;

            case Event.HitBlueBody:
                // purple wins

                // turn floor purple
                StartCoroutine(GoalScoredSwapGroundMaterial(arenaSettings.purpleGoalMaterial, RenderersList, .5f));
                // Add reward
                blueAgent.AddReward(2);
                purpleAgent.AddReward(-1);

                // end episode
                // blueAgent.EndEpisode();
                // purpleAgent.EndEpisode();
                ResetScene();
                break;

            case Event.HitIntoBlueArea:
                if (lastHitter == Team.Purple)
                {
                    purpleAgent.AddReward(2);
                }
                break;

            case Event.HitIntoPurpleArea:
                if (lastHitter == Team.Blue)
                {
                    blueAgent.AddReward(2);
                }
                break;
        }
    }

    /// <summary>
    /// Changes the color of the ground for a moment.
    /// </summary>
    /// <returns>The Enumerator to be used in a Coroutine.</returns>
    /// <param name="mat">The material to be swapped.</param>
    /// <param name="time">The time the material will remain.</param>
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

    /// <summary>
    /// Called every step. Control max env steps.
    /// </summary>
    void FixedUpdate()
    {
        resetTimer += 1;
        if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            blueAgent.AddReward(-1);
            purpleAgent.AddReward(-1);
            blueAgent.EpisodeInterrupted();
            purpleAgent.EpisodeInterrupted();
            ResetScene();
        }
    }

    /// <summary>
    /// Reset agent and ball spawn conditions.
    /// </summary>
    public void ResetScene()
    {
        // Debug.Log("RESET SCENE");
        resetTimer = 0;
        lastHitter = Team.Default; // reset last hitter
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
            var randomPosZ = Random.Range(2f*agentRot, 5f*agentRot);
            var randomRot = Random.Range(-45f, 45f);
            agent.transform.localPosition = new Vector3(randomPosX, 0, randomPosZ);
            agent.transform.eulerAngles = new Vector3(0, randomRot, 0);
            agent.GetComponent<Rigidbody>().velocity = default(Vector3);
        }
    }
}
