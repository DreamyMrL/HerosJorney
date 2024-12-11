using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterSlot
    {
        public GameObject characterPrefab;
        public Transform position;
        public BattleHUD hud;
        public string attackName;
        public string abilityName;
        public Unit unit;
    }

    public static SlotManager Instance;

    [Header("Slot Settings")]
    public int maxSlots = 3; // Maximum number of slots
    public List<Transform> predefinedSlotPositions; // Slot positions in the scene
    public List<CharacterSlot> characterSlots = new List<CharacterSlot>(); // Active characters in slots

    private void Awake()
    {
        // Singleton pattern to ensure only one SlotManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist SlotManager across scenes
    }

    /// <summary>
    /// Adds a character to the next available slot.
    /// </summary>
    public bool AddCharacter(GameObject characterPrefab, Transform slotPosition, string attackName, string abilityName)
    {
        if (characterSlots.Count < maxSlots)
        {
            CharacterSlot newSlot = new CharacterSlot
            {
                characterPrefab = characterPrefab,
                position = slotPosition,
                attackName = attackName,
                abilityName = abilityName,
                unit = characterPrefab.GetComponent<Unit>(),
                hud = slotPosition.GetComponentInChildren<BattleHUD>() // Assuming HUD is a child of the slot
            };

            characterSlots.Add(newSlot);
            UpdateSlotPositions();
            return true; // Character added successfully
        }

        Debug.LogWarning("No available slots!");
        return false; // Slots are full
    }

    /// <summary>
    /// Removes a character from a specific slot.
    /// </summary>
    public void RemoveCharacter(GameObject characterPrefab)
    {
        CharacterSlot slotToRemove = characterSlots.Find(slot => slot.characterPrefab == characterPrefab);
        if (slotToRemove != null)
        {
            characterSlots.Remove(slotToRemove);
            UpdateSlotPositions();
        }
    }

    /// <summary>
    /// Updates the positions of all characters in slots.
    /// </summary>
    private void UpdateSlotPositions()
    {
        foreach (var slot in characterSlots)
        {
            slot.characterPrefab.transform.position = slot.position.position;
        }
    }

    /// <summary>
    /// Gets the next available slot position.
    /// </summary>
    public Transform GetNextAvailableSlotPosition()
    {
        if (characterSlots.Count < predefinedSlotPositions.Count)
        {
            return predefinedSlotPositions[characterSlots.Count];
        }

        return null; // No available slot
    }
}


