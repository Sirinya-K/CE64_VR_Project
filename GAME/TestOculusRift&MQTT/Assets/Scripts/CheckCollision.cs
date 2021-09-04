using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Finger: " + gameObject.name + ", Collision with: " + other.collider.name);
    }
}
