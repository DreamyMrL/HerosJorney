using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStateData", menuName = "Dialogue System/State Data")]
public class StateData : ScriptableObject
{
    private Dictionary<int, bool> conditions = new Dictionary<int, bool>();

    public void SetCondition(int key, bool value)
    {
        conditions[key] = value;
    }

    public bool GetCondition(int key)
    {
        return conditions.ContainsKey(key) && conditions[key];
    }
}
