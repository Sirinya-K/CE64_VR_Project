using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpear : Weapon
{
    void Start()
    {
        mqtt = FindObjectOfType<MqttProtocol>();

        impactAtk = 5;
        slashAtk = 125;

        lastPosition = transform.position;
    }
}
