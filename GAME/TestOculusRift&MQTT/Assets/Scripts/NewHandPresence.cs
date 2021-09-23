using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class NewHandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristic;
    private InputDevice targetDevice;

    public GameObject handModelPrefab;
    private GameObject spawnedHandModel;

    private Animator handAnimator;

    private MqttProtocol mqtt;

    private void Awake() {
        mqtt = GameObject.FindObjectOfType<MqttProtocol> ();    
    }

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if(devices.Count > 0)
        {
            targetDevice = devices[0];
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {

        if (mqtt.data!="")
        {
            string[] buffer = mqtt.data.Split(':');
            if(buffer[0] == "hl" || buffer[0] == "hr")
            {
                handAnimator.SetFloat("Thumb", float.Parse(buffer[1].Split(' ')[0])/20);
                handAnimator.SetFloat("Index", float.Parse(buffer[1].Split(' ')[1])/20);
                handAnimator.SetFloat("ThreeFingers", float.Parse(buffer[1].Split(' ')[2])/20);
            }

            // handAnimator.SetFloat("Index", float.Parse(mqtt.data)/99);
        }

        // Debug.Log("MQTT Data: "+mqtt.data);

        // handAnimator.SetFloat("Grip", float.Parse(mqtt.data)/99);

        // if(targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        // {
        //     // Debug.Log("gripValue: " + gripValue);
        //     handAnimator.SetFloat("Index", gripValue);
        // }
        // else
        // {
        //     handAnimator.SetFloat("Index", 0);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHandAnimation();
    }
}
