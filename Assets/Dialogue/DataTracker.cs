using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDataManager : MonoBehaviour
{
    public static StateDataManager Instance;

    [SerializeField] private StateData stateData;
    private int currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentState(int newState)
    {
        currentState = newState;
        Debug.Log($"Current state set to {currentState}");
    }

    public int GetCurrentState()
    {
        return currentState;
    }

    public void UpdateCondition(int key, bool value)
    {
        stateData.SetCondition(key, value);
    }
}
