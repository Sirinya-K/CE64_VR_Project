using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : Weapon
{
    void Start()
    {
        mqtt = FindObjectOfType<MqttProtocol>();

        impactAtk = 30;
        slashAtk = 80;

        lastPosition = transform.position;
    }
}
