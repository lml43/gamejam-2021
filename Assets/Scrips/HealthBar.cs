using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public IntValue health;
    
    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth(health.initialValue);
    }

    void Update() {
        SetHealth(health.runtimeValue);
    }

    private void SetMaxHealth(int health) {
        slider.maxValue = health; 
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    private void SetHealth(int health) {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
