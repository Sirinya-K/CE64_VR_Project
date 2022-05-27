using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagement : MonoBehaviour
{
    public void spawn(GameObject obj, GameObject target)
    {
        obj.transform.position = target.transform.position;
    }
}
