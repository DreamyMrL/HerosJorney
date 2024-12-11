using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string npcName; // NPC's name
    [SerializeField] private Dialogue[] dialogueStates; // Array of dialogues for this NPC
    private int currentStateIndex = 0; // Tracks the current dialogue state

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Interaction key
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager instance not found.");
            return;
        }

        if (currentStateIndex < dialogueStates.Length)
        {
            Dialogue currentDialogue = dialogueStates[currentStateIndex];
            DialogueManager.Instance.StartDialogue(currentDialogue);

            // Move to the next state after the current dialogue
            currentStateIndex++;
        }
        else
        {
            Debug.Log($"{npcName} has no more dialogue states.");
        }
    }

    // Optional: Reset to a specific state
    public void SetCondition(int stateIndex)
    {
        if (stateIndex >= 0 && stateIndex < dialogueStates.Length)
        {
            currentStateIndex = stateIndex;
        }
        else
        {
            Debug.LogWarning("Invalid state index.");
        }
    }
}
