using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image attackCD;
    [SerializeField] Image dashCD;
    [SerializeField] Slider hpSlider;
    [SerializeField] TextPopup damagePopup;
    public TextPopup DamagePopup => damagePopup;
    [SerializeField] TextPopup titlePopup;
    public TextPopup TitlePopup => titlePopup;
    [SerializeField] Color enemyTextColor;
    public Color EnemyTextColor => enemyTextColor;

    public void UpdateAttackCD(float amount)
    {
        attackCD.fillAmount = amount;
    }

    public void UpdateDashCD(float amount)
    {
        dashCD.fillAmount = amount;
    }

    public void SetupHealthBar(float amount)
    {
        hpSlider.maxValue = amount;
        hpSlider.value = amount;
    }

    public void UpdateHealthBar(float amount)
    {
        hpSlider.value = amount;
    }
}
