using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private StateData stateData;
    [SerializeField] private int requiredConditionKey = -1;
    [SerializeField] private bool requiredConditionValue = true;

    public void StartDialogue()
    {
        if (stateData != null && requiredConditionKey >= 0)
        {
            if (stateData.GetCondition(requiredConditionKey) != requiredConditionValue)
            {
                Debug.Log("Condition not met. Dialogue cannot start.");
                return;
            }
        }

        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
