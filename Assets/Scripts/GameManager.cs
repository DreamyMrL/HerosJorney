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
    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadOverworldScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadForestScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadBattleScene()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadEndScene()
    {
        SceneManager.LoadScene(4);
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
