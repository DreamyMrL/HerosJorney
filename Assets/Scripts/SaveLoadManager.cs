using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public string PlayerName;

    [ContextMenu("Save")]
    private void Save()
    {
        PlayerPrefs.SetString("PlayerName", PlayerName);
    }

    [ContextMenu("Load")]
    private void Load()
    {
        PlayerName = PlayerPrefs.GetString("PlayerName");
    }
}
