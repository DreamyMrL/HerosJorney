using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUIManager : MonoBehaviour
{
    public GameObject dialoguePrefab;
    public float displayDuration = 2f;

    public void ShowDialogue(GameObject character, string text)
    {
        var dialogueInstance = Instantiate(dialoguePrefab, character.transform.position + Vector3.up * 1.5f, Quaternion.identity);
        dialogueInstance.transform.SetParent(character.transform);

        TMP_Text dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();
        if (dialogueText != null)
        {
            dialogueText.text = text;
        }

        StartCoroutine(DestroyAfterTime(dialogueInstance, displayDuration));
    }

    private IEnumerator DestroyAfterTime(GameObject instance, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(instance);
    }
}
