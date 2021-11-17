using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter(Collider other)
    {
        // If it isn't a weapon
        if (!other.gameObject.CompareTag("EnemyWeapon")) return;

        // Debug.Log(gameObject.name);

        if (gameObject.tag == "Shield")
        {
            StopAllCoroutines();
            player.regenable = false;
            StartCoroutine(player.DelayRegenerateStamina());

            if (player.currentStamina == 0) player.TakeDamage((int)(Random.Range(34, 50)));
            else
            {
                player.TakeDamage(0);
                player.CheckStamina();
            }
        }
        else player.TakeDamage((int)(Random.Range(34, 50)));

        if (gameObject.tag == "Stoppable")
        {
            Debug.Log("in");
            player.CheckArmState("in");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == "Stoppable")
        {
            Debug.Log("free");
            player.CheckArmState("free");
        }
    }
}
