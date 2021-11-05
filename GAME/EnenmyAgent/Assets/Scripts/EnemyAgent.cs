using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class EnemyAgent : Agent
{
    public GameObject area;
    Rigidbody agentRb;
    BehaviorParameters behaviorParameters;
    public Team teamId;

    // To get Sword's location for observations 
    // Bring sword object to this param in each other sword
    public GameObject sword;
    public GameObject enemy;
    Rigidbody swordRb;
    Rigidbody enemyRb;

    ArenaSettings arenaSettings;
    ArenaEnvController envController;

    private Animator anim;

    // Controls jump behavior
    float jumpingTime;
    Vector3 jumpTargetPos;
    Vector3 jumpStartingPos;
    // direction of agent
    float agentRot;
    float maxSpeed;

    public Collider[] hitGroundColliders = new Collider[3];
    EnvironmentParameters resetParams;

    void Start()
    {
        envController = area.GetComponent<ArenaEnvController>();
        maxSpeed = 10f;
    }

    public override void Initialize()
    {
        arenaSettings = FindObjectOfType<ArenaSettings>();
        behaviorParameters = gameObject.GetComponent<BehaviorParameters>();

        agentRb = GetComponent<Rigidbody>();
        swordRb = sword.GetComponent<Rigidbody>();
        enemyRb = enemy.GetComponent<Rigidbody>();

        // for symmetry between player side
        if (teamId == Team.Blue)
        {
            agentRot = -1;
        }
        else
        {
            agentRot = 1;
        }
    }
    private void FixedUpdate()
    {
        anim = GetComponentInChildren<Animator>();

        // Controll maxSpeed of Agent
        if (agentRb.velocity.magnitude > maxSpeed)
        {
            agentRb.velocity = agentRb.velocity.normalized * maxSpeed;
        }
        if (agentRb.velocity.magnitude >= 0)
        {
            var speedMap = 0f;
            speedMap = agentRb.velocity.magnitude / maxSpeed;
            if (speedMap >= 0)
            {
                anim.SetFloat("Speed", Mathf.Abs(speedMap));
                if (Input.GetKey(KeyCode.W))
                {
                    anim.SetFloat("WalkDir", 2f);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    anim.SetFloat("WalkDir", -3f);
                }
            }
        }
    }

    /// <summary>
    /// Moves  a rigidbody towards a position smoothly.
    /// </summary>
    /// <param name="targetPos">Target position.</param>
    /// <param name="rb">The rigidbody to be moved.</param>
    /// <param name="targetVel">The velocity to target during the
    ///  motion.</param>
    /// <param name="maxVel">The maximum velocity posible.</param>
    void MoveTowards(
        Vector3 targetPos, Rigidbody rb, float targetVel, float maxVel)
    {
        var moveToPos = targetPos - rb.worldCenterOfMass;
        var velocityTarget = Time.fixedDeltaTime * targetVel * moveToPos;
        if (float.IsNaN(velocityTarget.x) == false)
        {
            rb.velocity = Vector3.MoveTowards(
                rb.velocity, velocityTarget, maxVel);
        }
    }

    /// <summary>
    /// Check if agent is on the ground to enable/disable jumping
    /// </summary>
    /// function for check isAttackedandHit **future feature**
    public bool CheckIfGrounded()
    {
        hitGroundColliders = new Collider[3];
        var o = gameObject;
        Physics.OverlapBoxNonAlloc(
            o.transform.localPosition + new Vector3(0, -0.05f, 0),
            new Vector3(0.95f / 2f, 0.5f, 0.95f / 2f),
            hitGroundColliders,
            o.transform.rotation);
        var grounded = false;
        foreach (var col in hitGroundColliders)
        {
            if (col != null && col.transform != transform &&
                (col.CompareTag("walkableSurface") ||
                 col.CompareTag("purpleGoal") ||
                 col.CompareTag("blueGoal")))
            {
                grounded = true; //then we're grounded
                break;
            }
        }
        return grounded;
    }

    /// <summary>
    /// Called when agent collides with the ball
    /// </summary>
    void OnCollisionEnter(Collision c)
    {
        //if (teamId == Team.Blue)
        //{
        //    if (c.gameObject.CompareTag("purpleSword"))
        //    {
        //        envController.UpdateLastHitter(teamId);
        //    }
        //}
        //if(teamId == Team.Purple)
        //{
        //    if (c.gameObject.CompareTag("blueSword"))
        //    {
        //        envController.UpdateLastHitter(teamId);
        //    }
        //}
    }

    /// <summary>
    /// Starts the jump sequence
    /// </summary>
    public void Attack()
    {
        anim.SetTrigger("Attack");
        this.AddReward(-0.1f);
    }

    /// <summary>
    /// Resolves the agent movement
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var grounded = CheckIfGrounded();

        var dirToGoForwardAction = act[0];
        var rotateDirAction = act[1];
        var dirToGoSideAction = act[2];
        var attackAction = act[3];

        if (dirToGoForwardAction == 1)
        {
            dirToGo = transform.forward * 1f;
        }
        else if (dirToGoForwardAction == 2)
            dirToGo = transform.forward * arenaSettings.speedReductionFactor * -1f;
        if (rotateDirAction == 1)
            rotateDir = transform.up * -1f;
        else if (rotateDirAction == 2)
            rotateDir = transform.up * 1f;
        if (dirToGoSideAction == 1)
            dirToGo = transform.right * arenaSettings.speedReductionFactor * -1f;
        else if (dirToGoSideAction == 2)
            dirToGo = transform.right * arenaSettings.speedReductionFactor;
        if (attackAction == 1)
        {
            this.Attack();
        }

        var force = agentRot * dirToGo * arenaSettings.agentRunSpeed;

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        agentRb.AddForce(force,
            ForceMode.VelocityChange);

        // condition of walking animator

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.rotation.y);
        sensor.AddObservation(agentRb.velocity);
        sensor.AddObservation(enemyRb.velocity);
        sensor.AddObservation(enemy.transform.rotation.y);
        Vector3 toSword = new Vector3((swordRb.transform.position.x - this.transform.position.x) * agentRot,
        (swordRb.transform.position.y - this.transform.position.y),
        (swordRb.transform.position.z - this.transform.position.z) * agentRot);
        sensor.AddObservation(toSword.normalized);
        sensor.AddObservation(swordRb.velocity.y);
        sensor.AddObservation(swordRb.velocity.x * agentRot);
        sensor.AddObservation(swordRb.velocity.z * agentRot);
    }

    // For human controller
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            // rotate right
            discreteActionsOut[1] = 2;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // forward
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // rotate left
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // backward
            discreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // move left
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // move right
            discreteActionsOut[2] = 2;
        }
        discreteActionsOut[3] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
