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

    public TMP_Text dialogueText;
    public TMP_Text speakerNameText;
    public Button continueButton;
    public Button endButton;

    private Queue<DialogueLine> dialogueLines;
    private bool isDisplayingText = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dialogueLines = new Queue<DialogueLine>();
        continueButton?.onClick.AddListener(ContinueDialogue);
        endButton?.onClick.AddListener(EndDialogue);
        HideButtons();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueLines.Clear();

        foreach (var line in dialogue.lines)
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

        var line = dialogueLines.Dequeue();
        speakerNameText.text = line.speaker;
        dialogueText.text = "";

        if (isDisplayingText)
        {
            StopAllCoroutines();
        }

        StartCoroutine(TypeLine(line.text));

        if (line.requiresPlayerInput || dialogueLines.Count == 0)
        {
            ShowButtons();
        }
        else
        {
            HideButtons();
            StartCoroutine(WaitAndDisplayNextLine(line.displayTime));
        }
    }

    private IEnumerator TypeLine(string text)
    {
        isDisplayingText = true;

        foreach (var letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isDisplayingText = false;
    }

    private IEnumerator WaitAndDisplayNextLine(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisplayNextLine();
    }

    private void ContinueDialogue()
    {
        HideButtons();
        DisplayNextLine();
    }

    private void EndDialogue()
    {
        HideButtons();
        dialogueText.text = "";
        speakerNameText.text = "";
    }

    private void ShowButtons()
    {
        continueButton?.gameObject.SetActive(true);
        endButton?.gameObject.SetActive(true);
    }

    private void HideButtons()
    {
        continueButton?.gameObject.SetActive(false);
        endButton?.gameObject.SetActive(false);
    }
}
