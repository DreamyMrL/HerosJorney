using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text nameText;
    public Slider hpSlider;
    public Slider mpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        mpSlider.maxValue = unit.maxMP;
        mpSlider.value = unit.currentMP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
    public void SetMP(int mp)
    {
        mpSlider.value = mp;
    }
}
