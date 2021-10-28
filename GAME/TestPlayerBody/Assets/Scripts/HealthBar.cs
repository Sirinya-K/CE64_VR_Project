using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image health;

    public void UpdateHealth(float fraction)
    {
        health.fillAmount = fraction;
    }
}
