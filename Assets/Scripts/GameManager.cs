using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SceneManager ScenesManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void LoadOverworldScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadBattleScene()
    {
        SceneManager.LoadScene(1);
    }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

}
