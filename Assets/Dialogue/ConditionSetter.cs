using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionSetter : MonoBehaviour
{
    [SerializeField] private StateData stateData;
    [SerializeField] private int conditionKey;
    [SerializeField] private bool conditionValue;

    public void SetCondition()
    {
        if (stateData != null)
        {
            stateData.SetCondition(conditionKey, conditionValue);
            Debug.Log($"Condition {conditionKey} set to {conditionValue}.");
        }
        else
        {
            Debug.LogError("StateData is not assigned.");
        }
    }
}
