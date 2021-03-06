using System;
using System.ComponentModel;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class EnemyNoShieldAgent : Agent
{
    public GameObject area;
    Rigidbody agentRB;
    public Team1 teamId;

    // * Sword/opposite enemy Location for observations
    public GameObject sword;
    public GameObject enemy;
    Rigidbody swordRB;
    Rigidbody enemyRB;

    // * Arena environment setting for controll response of agent each side
    ArenaSettings arenaSettings;
    ArenaEnvControllerV2 envController;

    // * Animation var.
    private Animator anim;
    public bool tmpAttack { get; set; }
    public bool enemyBlock { get; set; }

    // * Direction of agent
    float agentRot;
    EnvironmentParameters resetParams;
    void Start()
    {
        envController = area.GetComponent<ArenaEnvControllerV2>();
    }
    public override void Initialize()
    {
        arenaSettings = FindObjectOfType<ArenaSettings>();

        agentRB = GetComponent<Rigidbody>();
        swordRB = sword.GetComponent<Rigidbody>();
        enemyRB = enemy.GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();

        // * For symmetry between agent 
        if (teamId == Team1.Blue)
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
        // * Get velocity ref from rotation of agent 
        float velocityZ = Vector3.Dot(agentRB.velocity, transform.forward);
        float velocityX = Vector3.Dot(agentRB.velocity, transform.right);

        anim.SetFloat("Velocity Z", velocityZ * agentRot);
        anim.SetFloat("Velocity X", velocityX * agentRot);

        // * Event check distance between 2 agent (out of range?)
        envController.ResolveEvent(Event1.OutOfRange);
    }
    void attack(int attackType)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Movement")||anim.GetCurrentAnimatorStateInfo(0).IsName("Movement_Block"))
        {
            if (attackType == 1)
            {
                anim.SetTrigger("Attack_1");
            }
            else if (attackType == 2)
            {
                anim.SetTrigger("Attack_2");
            }
        }
    }
    void block(bool isBlock)
    {
        anim.SetBool("Block", isBlock);
    }
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var dirToGoForwardAction = act[0];
        var rotateDirAction = act[1];
        var dirToGoSideAction = act[2];
        var attackAction = act[3];
        var blockAction = act[4];

        // * For check attack animation is working?
        tmpAttack = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_2");

        if (attackAction == 1 || attackAction == 2)
        {
            this.attack(attackAction);
        }
        if (rotateDirAction == 1)
            rotateDir = transform.up * -1f;
        else if (rotateDirAction == 2)
            rotateDir = transform.up * 1f;
        // * Use tmpAttack when want to break agent while attack
        // if (!tmpAttack)
        // {
            if (dirToGoForwardAction == 1)
            {
                dirToGo = transform.forward * 1f;
            }
            else if (dirToGoForwardAction == 2)
            {
                dirToGo = transform.forward * -1f;
            }
            if (dirToGoSideAction == 1)
            {
                dirToGo = transform.right * arenaSettings.speedReductionFactor * -1f;
            }
            else if (dirToGoSideAction == 2)
            {
                dirToGo = transform.right * arenaSettings.speedReductionFactor;
            }
        // }

        var force = agentRot * dirToGo * arenaSettings.agentRunSpeed;
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        agentRB.AddForce(force, ForceMode.VelocityChange);

    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // * Agent/Enemy rotation
        sensor.AddObservation(this.transform.localRotation.eulerAngles.y/360.0f);
        sensor.AddObservation(enemy.transform.localRotation.eulerAngles.y/360.0f);
        // * Distance between gent and enemy
        var minDis = 0;
        var maxDis = 26.5f;
        var disX = Mathf.Abs(enemy.transform.localPosition.x - this.transform.localPosition.x);
        var disZ = Mathf.Abs(enemy.transform.localPosition.z - this.transform.localPosition.z);
        sensor.AddObservation((disX-minDis)/(maxDis-minDis));
        sensor.AddObservation((disZ-minDis)/(maxDis-minDis));
        // * Agent/Enemy velocity
        sensor.AddObservation(agentRB.velocity.normalized);
        sensor.AddObservation(enemyRB.velocity.normalized);
        // * Sword Information
        Vector3 toSword = new Vector3((swordRB.transform.position.x - this.transform.position.x),
        (swordRB.transform.position.y - this.transform.position.y),
        (swordRB.transform.position.z - this.transform.position.z));
        sensor.AddObservation(toSword.normalized);
        sensor.AddObservation(toSword.magnitude);
    }
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
        if (Input.GetKey(KeyCode.Z))
        {
            // acttackAction_1
            discreteActionsOut[3] = 1;
        }
        if (Input.GetKey(KeyCode.X))
        {
            // acttackAction_2
            discreteActionsOut[3] = 2;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            if (teamId == Team1.Blue)
            {
                envController.ResolveEvent(Event1.BlueHitWall);
            }
            if (teamId == Team1.Purple)
            {
                envController.ResolveEvent(Event1.PurpleHitWall);
            }
        }
    }
}
