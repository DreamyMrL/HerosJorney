using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueState", menuName = "Dialogue/Dialogue State")]
public class DialogueState : ScriptableObject
{
    public string stateName;  // Name or ID for the dialogue state
    public Dialogue dialogue; // Reference to the Dialogue asset for this state
}

