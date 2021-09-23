using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections;

public class SimpleCollectorAgent : Agent
{
    [Tooltip("The enemy to be moved around")]
    public GameObject enemy;
    private Vector3 startPosition;
    private SimpleCharacterController characterController;
    new private Rigidbody rigidbody;
    private bool isAttack;
    private bool isCollision;
    private bool tmpAttack;
    private bool canAttack;
    public override void Initialize()
    {
        startPosition = transform.position;
        characterController = GetComponent<SimpleCharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        UnityEngine.Debug.ClearDeveloperConsole();
    }
    // ตอนเริ่ม Episode จะต้องมีการ Reset ค่า position ของ agent  และ player
    public override void OnEpisodeBegin()
    {
        // Reset agent position, rotation
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        rigidbody.velocity = Vector3.zero;

        // Reset enemy position (5 meters away from the agent in a random direction)
        enemy.transform.position = startPosition + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * 5f;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Read input values and round them. GetAxisRaw works better in this case
        // because of the DecisionRequester, which only gets new decisions periodically.
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        bool jump = Input.GetKey(KeyCode.Space);
        canAttack = Input.GetKey(KeyCode.Return);

        // Convert the actions to Discrete choices (0, 1, 2)
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
        actions[2] = canAttack ? 1 : 0;
    }
    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        isAttack = false;
    }
    void DoDelayAction(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log("isAttack in OnActionReceived => "+isAttack);
        // Convert actions from Discrete (0, 1, 2) to expected input values (-1, 0, +1)
        // of the character controller
        float vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
        float horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
        bool attack = actions.DiscreteActions[2] > 0;

        // Punish and end episode if the agent strays too far
        if (Vector3.Distance(startPosition, transform.position) > 10f)
        {
            AddReward(-1f);
            EndEpisode();
        }
        if (attack)
        {
            tmpAttack = true;
        }
        if (attack && !isAttack)
        {
            AddReward(-0.1f);
        }
        isAttack = false;
        //Debug.Log("isAttack in CONDITION => " + isAttack);
        //Debug.Log(isAttack);
        characterController.ForwardInput = vertical;
        characterController.TurnInput = horizontal;
        characterController.AttackInput = attack;
        //Debug.Log(characterController.AttackInput);
    }
    private void OnTriggerEnter(Collider other)
    {
        // If the other object is a collectible, reward and end episode
        if (other.tag == "enemy")
        {
            if (tmpAttack)
            {
                AddReward(3f);
                tmpAttack = false;
            }
            isAttack = true;
            EndEpisode();
        }
    }
}