using UnityEngine;

public class ArenaSettings : MonoBehaviour
{
    public float agentRunSpeed = 1.5f;
    public float agentJumpHeight = 2.75f;
    public float agentJumpVelocity = 777;
    public float agentJumpVelocityMaxChange = 10;

    // Slows down strafe & backward movement
    public float speedReductionFactor = 0.75f;

    // This is a downward force applied when falling to make jumps look less floaty
    public float fallingForce = 150;
}
