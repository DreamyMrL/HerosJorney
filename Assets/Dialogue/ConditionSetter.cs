using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionSetter : MonoBehaviour
{
    [SerializeField]
    private DialogueTrigger dialogueTrigger; // Reference to the DialogueTrigger

    public void SetCondition(int condition)
    {
        if (dialogueTrigger != null)
        {
            dialogueTrigger.SetCondition(condition);
        }
        else
        {
            Debug.LogError("DialogueTrigger reference is missing in ConditionSetter.");
        }
    }
}

