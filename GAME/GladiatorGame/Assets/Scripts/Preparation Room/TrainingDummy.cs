using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        
        if(other.gameObject.CompareTag("Weapon")) animator.Play("pushed");
    }
}
