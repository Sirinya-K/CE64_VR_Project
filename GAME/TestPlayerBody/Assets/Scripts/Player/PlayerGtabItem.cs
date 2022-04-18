using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGtabItem : MonoBehaviour
{
    public GameObject index, middle, ring, pinky;

    private Player player;

    private LayerMask grabbableLayer;

    private Rigidbody connectedBody, itemBody;

    private FixedJoint itemJoint;

    private FingerCollision indexCollision, middleCollision, ringCollision, pinkyCollision;

    private PlayerHandController hand;

    // private string finger = "";
    // private float fingerValue = 0f;
    // private float diff = 0.1f;

    private GameObject theItem; //คือ item ที่ player กำลังถือ
    private Vector3 theItemDefaultPosition; //position ดั้งเดิมของ theItem

    // private Collider[] itemColliders;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        grabbableLayer = LayerMask.NameToLayer("Grabbable");

        connectedBody = GetComponent<Rigidbody>();

        indexCollision = index.GetComponent<FingerCollision>();
        middleCollision = middle.GetComponent<FingerCollision>();
        ringCollision = ring.GetComponent<FingerCollision>();
        pinkyCollision = pinky.GetComponent<FingerCollision>();

        hand = GameObject.FindObjectOfType<PlayerHandController>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == grabbableLayer)
        {
            if (theItem == null) //if (other.gameObject.GetComponent<FixedJoint>() == null)
            {
                // Check that Index or Middle collision with the item
                if ((indexCollision.collisionStatus == true && hand.RIndexValue >= 0.15) || (middleCollision.collisionStatus == true && hand.RMiddleValue >= 0.15) || (ringCollision.collisionStatus == true && hand.RRingValue >= 0.15) || (pinkyCollision.collisionStatus == true && hand.RPinkyValue >= 0.15))
                {
                    // Create Fixed Joint between the Item and Hand
                    itemJoint = other.gameObject.AddComponent<FixedJoint>();
                    itemJoint.connectedBody = connectedBody;
                    itemJoint.connectedMassScale = 0;

                    itemBody = other.gameObject.GetComponent<Rigidbody>();
                    itemBody.useGravity = false;
                    itemBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    itemBody.interpolation = RigidbodyInterpolation.Interpolate;

                    // Add CheckVelocity Script to the Item
                    // other.gameObject.AddComponent<CheckVelocity>();

                    // Save Finger Value (เช็คว่าเป็นการจับแบบไหน (ฝ่ามือ,โป้ง)ชี้กลาง/(ฝ่ามือ,โป้ง)ชี้/(ฝ่ามือ,โป้ง)กลาง)
                    // if (indexCollision.collisionStatus && middleCollision.collisionStatus)
                    // {
                    //     finger = "both";
                    //     fingerValue = hand.RMiddleValue;
                    // }
                    // else if (indexCollision.collisionStatus)
                    // {
                    //     finger = "index";
                    //     fingerValue = hand.RIndexValue;
                    // }
                    // else if (middleCollision.collisionStatus)
                    // {
                    //     finger = "middle";
                    //     fingerValue = hand.RMiddleValue;
                    // }

                    // Set Trigger on the Item
                    // itemColliders = other.gameObject.GetComponentsInChildren<Collider>();
                    // foreach (Collider c in itemColliders) c.isTrigger = true;

                    //เก็บข้อมูล object ที่ถือ และให้ item เป็นลูกของ player เพื่อให้ position ตามติด
                    theItem = other.gameObject;
                    theItemDefaultPosition = theItem.transform.position;
                    theItem.transform.parent = GameObject.FindGameObjectWithTag("PlayerHand").transform;

                    player.SetCurrentWeapon(theItem);
                    if (player.GetCurrentWeapon().tag == "Weapon")
                    {
                        GameObject info = player.GetCurrentWeapon().transform.Find("WeaponInfo").gameObject;
                        info.SetActive(false);
                    }

                    // Debug.Log("Grab " + theItem.name);
                }
            }
        }
    }

    private void Release()
    {
        // Debug.Log(transform.name + " | Release: " + theItem.name);

        Destroy(itemJoint);
        if (theItem.gameObject.tag != "Weapon") itemBody.useGravity = true;
        itemBody = null;

        // finger = "";
        // fingerValue = 0f;

        //แสดง info ของอาวุธเมื่อปล่อย
        if (player.GetCurrentWeapon() != null)
        {
            if (player.GetCurrentWeapon().tag == "Weapon")
            {
                GameObject info = player.GetCurrentWeapon().transform.Find("WeaponInfo").gameObject;
                info.SetActive(true);
            }
        }


        //ให้ parent เป็น null เมื่อผู้เล่นปล่อย item แล้ว
        theItem.transform.parent = null;

        player.SetCurrentWeapon(null);

        theItem = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (finger == "index" && (fingerValue - hand.RIndexValue >= diff))
        // {
        //     Release();
        // }
        // else if (finger == "middle" && (fingerValue - hand.RMiddleValue >= diff))
        // {
        //     Release();
        // }
        // else if (finger == "both" && (fingerValue - hand.RIndexValue >= diff && fingerValue - hand.RMiddleValue >= diff))
        // {
        //     Release();
        // }

        // Debug.Log(transform.name + " | theItem: " + theItem);

        if (theItem != null && (indexCollision.collisionStatus == false || hand.RIndexValue < 0.15) && (middleCollision.collisionStatus == false || hand.RMiddleValue < 0.15) && (ringCollision.collisionStatus == false || hand.RRingValue < 0.15) && (pinkyCollision.collisionStatus == false || hand.RPinkyValue < 0.15))
        {
            Release();
        }

        //ปล่อยอาวุธกรณี player กลับหน้าเมนูหลัก (ไม่ใช้แล้ว)
        // if(stateManagement.onMainMenu && theItem != null)
        // {
        //     Release();
        //     theItem.transform.position = theItemDefaultPosition;
        //     theItem = null;
        // }
    }
}
