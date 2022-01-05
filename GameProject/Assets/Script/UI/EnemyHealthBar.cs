using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Color low, high;
    [SerializeField]
    private Vector3 offset;

    public void SetMaxHealth(float maxHealth) {
        slider.gameObject.SetActive(false);
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(float health) {
        slider.gameObject.SetActive(health < slider.maxValue && health > 0);
        slider.value = health;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
