using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float speed = 1;
    public float additionalHeight = 0.2f;

    private Vector2 inputAxis;
    private CharacterController character;

    private XRRig rig;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
    //     device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    // }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();

        // Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y);

        // *** set ให้ player เคลื่อนไหวตามทิศทางของ headset
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        // Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        if (Input.GetKey(KeyCode.A))  
        {  
            direction = headYaw * new Vector3(-1, 0, 0);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }  
        if (Input.GetKey(KeyCode.D))    
        {  
            direction = headYaw * new Vector3(1, 0, 0);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }  
        if (Input.GetKey(KeyCode.S))  
        {  
            direction = headYaw * new Vector3(0, 0, -1);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }  
        if (Input.GetKey(KeyCode.W))  
        {   
            direction = headYaw * new Vector3(0, 0, 1);
            character.Move(direction * Time.fixedDeltaTime * speed);
        }  

        // character.Move(direction * Time.fixedDeltaTime * speed);
    }

    // *** set ให้ตัว capsule ของ character เคลื่อนไหวตาม headset
    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height/2 + character.skinWidth, capsuleCenter.z);
    }
}