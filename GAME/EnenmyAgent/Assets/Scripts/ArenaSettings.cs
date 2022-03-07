using UnityEngine;

public class ArenaSettings : MonoBehaviour
{
    public float agentRunSpeed = 1.5f;
    // Slows down strafe & backward movement
    public float speedReductionFactor = 0.75f;

    public Material blueGoalMaterial;
    public Material purpleGoalMaterial;
    public Material defaultMaterial;
}
