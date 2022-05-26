using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshProController : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [HideInInspector]
    public ArenaEnvControllerV2 arenaEnvController;
    private string winText = "-";
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        arenaEnvController = GetComponentInParent<ArenaEnvControllerV2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (arenaEnvController.currentTeam == Team1.Blue)
        {
            winText = "BLUE";
        }
        if (arenaEnvController.currentTeam == Team1.Purple)
        {
            winText = "PURPLE";
        }
        textMesh.text = "Team : " + winText + "\nBlueScore : " + arenaEnvController.blueScore.ToString() + "\nPurpleScore :  " 
        + arenaEnvController.purpleScore.ToString() + "\nSteps : " +arenaEnvController.resetTimer.ToString();
    }
}
