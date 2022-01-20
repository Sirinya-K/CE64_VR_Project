using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    public float destroyTime = 2f;
    public Vector3 offset = new Vector3(0, 2, 0);
    public Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);

    void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
        Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
        Random.Range(-randomizeIntensity.z, randomizeIntensity.z));
    }
}
