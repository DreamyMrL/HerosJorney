using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Conversation")]
public class Dialogue : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
    public string continuePromptText = "Press E to continue";  // Text to show when input is needed to continue
    public string endPromptText = "Press E to end conversation"; // Text to show when input is needed to end
}

[System.Serializable]
public class DialogueLine
{
    public string speaker;  // e.g., "Player" or "NPC"
    public string speakerName;  // Name of the speaker displayed above text
    [TextArea(2, 5)] public string text;
    public float displayDuration = 2f;  // Control how long each line is displayed
    public bool requiresPlayerInput = false;  // True if player input is needed after this line
}

