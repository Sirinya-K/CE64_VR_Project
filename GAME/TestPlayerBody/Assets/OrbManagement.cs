using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbManagement : MonoBehaviour
{
    public Material green;
    public Material blue;
    public Material red;

    private string[,] orb = new string[,] { { "ฟื้นฟูพลังชีวิต 30%", "Green" }, { "เพิ่มความเสียหายของอาวุธทั้งหมด 20%", "Blue" }, { "ลดความเร็วการเคลื่อนที่ของศัตรู 10%", "Red" } };

    public void Show(int num,string name)
    {
        GameObject theOrb = GameObject.Find(name);
        Text theOrbDesc = theOrb.GetComponentInChildren<Text>();

        if (orb[num, 1] == "Green")
        {
            theOrb.GetComponent<MeshRenderer>().material = green;
        }
        else if (orb[num, 1] == "Blue")
        {
            theOrb.GetComponent<MeshRenderer>().material = blue;
        }
        else if (orb[num, 1] == "Red")
        {
            theOrb.GetComponent<MeshRenderer>().material = red;
        }

        theOrbDesc.text = orb[num,0];
    }
}
