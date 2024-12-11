using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStateTracker : MonoBehaviour
{
    public static DialogueStateTracker Instance { get; private set; }

    private Dictionary<string, DialogueState> npcDialogueStates;
    private Dictionary<string, Dictionary<string, DialogueState>> conditionalStates;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            npcDialogueStates = new Dictionary<string, DialogueState>();
            conditionalStates = new Dictionary<string, Dictionary<string, DialogueState>>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public DialogueState GetDialogueState(string npcName)
    {
        npcDialogueStates.TryGetValue(npcName, out DialogueState state);
        return state;
    }

    public void SetDialogueState(string npcName, DialogueState newState)
    {
        if (npcDialogueStates.ContainsKey(npcName))
        {
            npcDialogueStates[npcName] = newState;
        }
        else
        {
            npcDialogueStates.Add(npcName, newState);
        }
    }

    public void SetConditionalState(string npcName, string condition, DialogueState newState)
    {
        if (!conditionalStates.ContainsKey(npcName))
        {
            conditionalStates[npcName] = new Dictionary<string, DialogueState>();
        }

        conditionalStates[npcName][condition] = newState;
    }

    public DialogueState GetConditionalState(string npcName, string condition)
    {
        if (conditionalStates.TryGetValue(npcName, out var conditions) &&
            conditions.TryGetValue(condition, out var state))
        {
            return state;
        }

        return GetDialogueState(npcName); // Fallback to default state
    }
    public string ResolveDialogueKey(string npcName, string condition = null)
    {
        if (string.IsNullOrEmpty(condition))
            return npcName;

        return $"{npcName}_{condition}"; // Combine NPC name and condition to form a unique key
    }


    /// <summary>
    /// Updates the dialogue state after a dialogue interaction is completed.
    /// </summary>
    public void UpdateStateAfterDialogue(string npcName, DialogueState newState)
    {
        SetDialogueState(npcName, newState);
    }
}

