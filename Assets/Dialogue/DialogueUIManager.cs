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
        Debug.Log($"Showing dialogue above {character.name}: '{text}'");

        GameObject dialogueInstance = Instantiate(dialoguePrefab, character.transform.position + Vector3.up * 1.5f, Quaternion.identity);
        dialogueInstance.transform.SetParent(character.transform);

        TMP_Text dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();
        if (dialogueText != null)
        {
            dialogueText.text = text;
            Debug.Log("Dialogue text set successfully.");
        }
        else
        {
            Debug.LogError("TMP_Text component not found in dialogue prefab.");
        }

        StartCoroutine(DestroyAfterTime(dialogueInstance, displayDuration));
    }

    private IEnumerator DestroyAfterTime(GameObject instance, float time)
    {
        Debug.Log("Starting DestroyAfterTime coroutine.");
        yield return new WaitForSeconds(time);
        Destroy(instance);
        Debug.Log("Dialogue UI destroyed.");
    }
}
