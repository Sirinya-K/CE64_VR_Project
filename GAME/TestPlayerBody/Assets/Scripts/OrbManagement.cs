using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbManagement : MonoBehaviour
{
    public Material green;
    public Material blue;
    public Material red;

    private string[,] orbs = new string[,] { { "ฟื้นฟูพลังชีวิต 20%", "Green", "20", "0" }, { "เพิ่มพลังชีวิตสูงสุดขึ้น 15%", "Green", "15", "2" }, { "เพิ่มพลังกายสูงสุดขึ้น 10%", "Green", "10", "2" },
                                            { "เพิ่มพลังโจมตี 10% ใน 15 วินาทีแรก", "Blue", "10", "1" }, { "เพิ่มอัตราคริ 5%", "Blue", "5", "2" }, { "เพิ่มความเสียหายเมื่อติดคริ 10%", "Blue", "10", "2" },
                                            { "ลดความเร็วการเคลื่อนที่ของศัตรูลง 10%", "Red", "10", "1" }, { "ลดความเสียหายของกับดักลง 20%", "Red", "20", "1" }, { "เมื่อพลังกายของผู้เล่นมากกว่า 0 จะมีโอกาส 15% ที่ศัตรูจะโจมตีพลาด", "Red", "15%", "1" } };

    public void Show(int num, string name)
    {
        GameObject theOrb = GameObject.Find(name);
        Text theOrbDesc = theOrb.GetComponentInChildren<Text>();

        if (orbs[num, 1] == "Green")
        {
            theOrb.GetComponent<MeshRenderer>().material = green;
        }
        else if (orbs[num, 1] == "Blue")
        {
            theOrb.GetComponent<MeshRenderer>().material = blue;
        }
        else if (orbs[num, 1] == "Red")
        {
            theOrb.GetComponent<MeshRenderer>().material = red;
        }

        theOrbDesc.text = orbs[num, 0];
    }

    public void ImplementEffect(int num)
    {

    }

    public void RemoveEffect(int num)
    {

    }
}
