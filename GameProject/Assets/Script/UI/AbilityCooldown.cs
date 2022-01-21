using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    // === DOUBLE DAMAGE ===
    [SerializeField]
    private GameObject doubleDamage;
    private bool isDoubleDamageCooldown = false;
    private float doubleDamageDuration;
    private Image doubleDamageImage;

    // === DASH ===
    [SerializeField]
    private GameObject dash;
    private bool isDashCooldown = false;
    private float dashCooldownDuration;
    private Image dashImage;

    // === SLIDE ===
    [SerializeField]
    private GameObject slide;
    private bool isSlideCooldown = false;
    private float slideCooldownDuration;
    private Image slideImage;


    public void DoubleDamageCooldown(float duration) {
        doubleDamageDuration = duration;
        isDoubleDamageCooldown = true;
        doubleDamageImage.fillAmount = 1f;
    }

    public void DashCooldown(float duration) {
        dashCooldownDuration = duration;
        isDashCooldown = true;
        dashImage.fillAmount = 1f;
    }

    public void SlideCooldown(float duration) {
        slideCooldownDuration = duration;
        isSlideCooldown = true;
        slideImage.fillAmount = 1f;
    }

    void Start()
    {
        doubleDamageImage = doubleDamage.transform.Find("Mask").gameObject.GetComponent<Image>();
        dashImage = dash.transform.Find("Mask").gameObject.GetComponent<Image>();
        slideImage = slide.transform.Find("Mask").gameObject.GetComponent<Image>();
        doubleDamageImage.fillAmount = 0f;
        dashImage.fillAmount = 0f;
        slideImage.fillAmount = 0f;
    }

    void Update() {
        if (isDoubleDamageCooldown) {
            doubleDamageImage.fillAmount -= 1 / doubleDamageDuration * Time.deltaTime;

            if (doubleDamageImage.fillAmount <= 0) {
                doubleDamageImage.fillAmount = 0;
                isDoubleDamageCooldown = false;
            }
        }
        if (isSlideCooldown) {
            slideImage.fillAmount -= 1 / slideCooldownDuration * Time.deltaTime;

            if (slideImage.fillAmount <= 0) {
                slideImage.fillAmount = 0;
                isSlideCooldown = false;
            }
        }
        if (isDashCooldown) {
            dashImage.fillAmount -= 1 / dashCooldownDuration * Time.deltaTime;

            if (dashImage.fillAmount <= 0) {
                dashImage.fillAmount = 0;
                isDashCooldown = false;
            }
        }
    }
}
