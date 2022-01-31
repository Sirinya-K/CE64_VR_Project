using System;
using System.ComponentModel;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class EnemyAgentV2 : Agent
{
    public GameObject area;
    Rigidbody agentRB;
    public Team teamId;

    // * Sword/opposite enemy Location for observations
    public GameObject sword;
    public GameObject enemy;
    Rigidbody swordRB;
    Rigidbody enemyRB;

    // * Arena environment setting for controll response of agent each side
    ArenaSettings arenaSettings;
    ArenaEnvController envController;

    // * Animation var.
    private Animator anim;
    public bool isAttack { get; set; }

    // * Direction of agent
    float agentRot;
    EnvironmentParameters resetParams;
    void Start()
    {
        envController = area.GetComponent<ArenaEnvController>();
    }
    public override void Initialize()
    {
        arenaSettings = FindObjectOfType<ArenaSettings>();

        agentRB = GetComponent<Rigidbody>();
        swordRB = sword.GetComponent<Rigidbody>();
        enemyRB = enemy.GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();

        // * For symmetry between agent 
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
        // * Get velocity ref from rotation of agent 
        float velocityZ = Vector3.Dot(agentRB.velocity, transform.forward);
        float velocityX = Vector3.Dot(agentRB.velocity, transform.right);

        anim.SetFloat("Velocity Z",velocityZ);
        anim.SetFloat("Velocity X",velocityX);

        // * Event check distance between 2 agent (out of range?)
        envController.ResolveEvent(Event.OutOfRange);
    }
    void attack(int attackType)
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
        {
            if(attackType == 1){
                anim.SetTrigger("Attack_1");
            }
            else if(attackType == 2){
                anim.SetTrigger("Attack_2");
            }
        }
    }
    void block(bool isBlock){
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

        // tmpAttack = anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Sword_atk01");

        if (dirToGoForwardAction == 1)
        {
            dirToGo = transform.forward * 1f;
        }
        else if (dirToGoForwardAction == 2)
        {
            dirToGo = transform.forward * -1f;
        }
        if (rotateDirAction == 1)
            rotateDir = transform.up * -1f;
        else if (rotateDirAction == 2)
            rotateDir = transform.up * 1f;
        if (dirToGoSideAction == 1)
        {
            dirToGo = transform.right * arenaSettings.speedReductionFactor * -1f;
        }
        else if (dirToGoSideAction == 2)
        {
            dirToGo = transform.right * arenaSettings.speedReductionFactor;
        }
        if (attackAction == 1 || attackAction == 2)
        {
            this.attack(attackAction);
        }
        if (blockAction == 1)
        {
            this.block(true);
        }
        if( blockAction == 0)
        {
            this.block(false);
        }
        //swordController.isAttack = false;
        var force = agentRot * dirToGo * arenaSettings.agentRunSpeed;

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        agentRB.AddForce(force, ForceMode.VelocityChange);

        // condition of walking animator

    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // // * Agent/Enemy rotation
        // sensor.AddObservation(this.transform.rotation.y);
        // sensor.AddObservation(enemy.transform.rotation.y);
        // // * Agent/Enemy velocity
        // sensor.AddObservation(agentRB.velocity);
        // sensor.AddObservation(enemyRB.velocity);
        // // * Sword Information
        // Vector3 toSword = new Vector3((swordRB.transform.position.x - this.transform.position.x) * agentRot,
        // (swordRB.transform.position.y - this.transform.position.y),
        // (swordRB.transform.position.z - this.transform.position.z) * agentRot);
        // sensor.AddObservation(toSword.normalized);
        // sensor.AddObservation(swordRB.velocity.y);
        // sensor.AddObservation(swordRB.velocity.x * agentRot);
        // sensor.AddObservation(swordRB.velocity.z * agentRot);
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
        discreteActionsOut[4] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
