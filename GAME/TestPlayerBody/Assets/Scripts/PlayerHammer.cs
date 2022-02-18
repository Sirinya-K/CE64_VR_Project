using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHammer : Weapon
{
    void Start()
    {
        mqtt = FindObjectOfType<MqttProtocol>();

        impactAtk = 70;
        slashAtk = 30;

        lastPosition = transform.position;
    }
}
