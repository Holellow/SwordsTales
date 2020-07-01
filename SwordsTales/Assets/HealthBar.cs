using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   [SerializeField] private Slider healthBarSlider;

    public void HealthBarValue(float health)
    {
        healthBarSlider.value = health;
    }
}
