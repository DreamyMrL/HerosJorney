using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public int bagdmg;
    public int amount;
    public int poison = 0;
    public GameObject battleprefab;
    public bool ice = true;
    public bool fire = false;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        transform.DOPunchScale(Vector3.one * 1.5f, 0.25f, vibrato: 15);
        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public bool PoisonDamage(int poison)
    {
        currentHP -= poison;
        transform.DOPunchScale(Vector3.one * 1.5f, 0.25f, vibrato: 15);
        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public bool UseMP(int amount)
    {
        if (currentMP >= amount)
        {
            currentMP -= amount;
            return true; // MP used successfully
        }
        return false; // Not enough MP
    }

    public bool BagDamage(int bagdmg)
    {
        currentHP -= bagdmg;
        transform.DOPunchScale(Vector3.one * 1.5f, 0.25f, vibrato: 15);
        if (currentHP <= 0)
            return true;
        else
            return false;
    }

}


