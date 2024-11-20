using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public TMP_Text playerDialogueText;
    public TMP_Text npcDialogueText;
    public Overworld playerController;

    public Button continueButton;
    public Button endButton;
    public float displayDuration = 2f;

    private Queue<DialogueLine> dialogueLines;
    private bool isDisplayingText = false;
    private bool isPromptVisible = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        dialogueLines = new Queue<DialogueLine>();

        continueButton.onClick.AddListener(ContinueDialogue);
        endButton.onClick.AddListener(EndDialogue);

        continueButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueLines.Clear();
        isPromptVisible = false;

        playerController.enabled = false;

        foreach (DialogueLine line in dialogue.lines)
        {
            dialogueLines.Enqueue(line);
        }

        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        playerDialogueText.text = "";
        npcDialogueText.text = "";

        DialogueLine line = dialogueLines.Dequeue();
        TMP_Text targetText = line.speaker == "Player" ? playerDialogueText : npcDialogueText;

        if (isDisplayingText)
        {
            StopAllCoroutines();
        }

        StartCoroutine(TypeLine(line.text, targetText));

        if (line.requiresPlayerInput)
        {
            ShowPromptButtons();
        }
        else
        {
            HidePromptButtons();
            StartCoroutine(WaitAndDisplayNextLine(line.displayDuration));
        }
    }

    private IEnumerator TypeLine(string line, TMP_Text targetText)
    {
        targetText.text = "";
        isDisplayingText = true;

        foreach (char letter in line.ToCharArray())
        {
            targetText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isDisplayingText = false;
    }

    private IEnumerator WaitAndDisplayNextLine(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (!isPromptVisible)
        {
            DisplayNextLine();
        }
    }

    private void ShowPromptButtons()
    {
        continueButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(true);
        isPromptVisible = true;
    }

    private void HidePromptButtons()
    {
        continueButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);
        isPromptVisible = false;
    }

    private void ContinueDialogue()
    {
        HidePromptButtons();
        DisplayNextLine();
    }

    private void EndDialogue()
    {
        HidePromptButtons();
        playerController.enabled = true;

        playerDialogueText.text = "";
        npcDialogueText.text = "";
    }
}
