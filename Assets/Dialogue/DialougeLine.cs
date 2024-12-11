using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker; // Name of the speaker (e.g., "Player" or NPC name).
    public string text;    // Text for the line of dialogue.
    public float displayTime = 3f; // Duration for auto-advance.
    public bool requiresPlayerInput = false; // If true, wait for input to advance.
}
