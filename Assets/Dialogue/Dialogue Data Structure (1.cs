using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<DialogueLine> lines; // The list of dialogue lines.
    public int nextState = -1; // Optional: What state to transition to after this dialogue.
}
