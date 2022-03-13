// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CheckTrigger : MonoBehaviour
// {
//     public Player player;

//     private void OnTriggerEnter(Collider other)
//     {
//         // If it isn't a weapon
//         if (!other.gameObject.CompareTag("EnemyWeapon")) return;

//         // Debug.Log(gameObject.name);

//         if (gameObject.tag == "PlayerShield")
//         {
//             StopAllCoroutines();
//             player.regenable = false;
//             StartCoroutine(player.DelayRegenerateStamina());

//             if (player.currentStamina == 0) player.TakeDamage((int)(Random.Range(34, 50)/2));
//             else
//             {
//                 player.TakeDamage(0);
//                 player.ReduceStamina(1);
//             }
//         }
//         else player.TakeDamage((int)(Random.Range(34, 50)));

//         // เช็คว่าเป็นส่วนของแขนใช่มั้ย
//         if (gameObject.tag == "Stoppable")
//         {
//             Debug.Log("in");
//             player.PublishArmStateToMqtt("Right","In");
//             // player.CheckArmState("in");
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (!other.gameObject.CompareTag("EnemyWeapon")) return;

//         if (gameObject.tag == "Stoppable")
//         {
//             Debug.Log("free");
//             player.PublishArmStateToMqtt("Right","free");
//             // player.CheckArmState("free");
//         }
//     }
// }
