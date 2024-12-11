using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
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

    public TMP_Text dialogueText;
    public TMP_Text abilityText;
    public BattleState state;

    public UnityEvent ReturntoGame;
    public UnityEvent[] SwapToButtons; // Array of UnityEvents for swapping button UI per character

    public List<CharacterSlot> playerSlots = new List<CharacterSlot>();
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public Transform enemyPosition;
    public BattleHUD enemyHUD;

    private Unit enemyUnit;
    private int currentPlayerIndex = 0; // Tracks whose turn it is
    private bool evade = false;
    private int target;
    private bool ice = true;
    private bool fire = false;

    public static bool slime = false;
    public static bool plant = false;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        // Instantiate player characters based on slots
        foreach (var slot in playerSlots)
        {
            GameObject characterGO = Instantiate(slot.characterPrefab, slot.position);
            slot.unit = characterGO.GetComponent<Unit>();
            slot.hud.SetHUD(slot.unit);
        }

        // Instantiate the enemy
        if (slime == true)
        {
            GameObject enemyGO = Instantiate(enemyPrefab1, enemyPosition);
            enemyUnit = enemyGO.GetComponent<Unit>();
        }
        else if (plant == true)
        {
            GameObject enemyGO = Instantiate(enemyPrefab2, enemyPosition);
            enemyUnit = enemyGO.GetComponent<Unit>();
        }

        dialogueText.text = "A " + enemyUnit.unitName + " encounters Pandora's squad!";
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        var currentSlot = playerSlots[currentPlayerIndex];
        dialogueText.text = $"What will {currentSlot.unit.unitName} do?";
        abilityText.text = currentSlot.abilityName;
    }

    IEnumerator PlayerAttack()
    {
        var currentSlot = playerSlots[currentPlayerIndex];
        bool isDead = enemyUnit.TakeDamage(currentSlot.unit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = $"{currentSlot.unit.unitName} attacks for {currentSlot.unit.damage} damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            AdvanceToNextPlayer();
        }
    }

    IEnumerator PlayerAbility()
    {
        var currentSlot = playerSlots[currentPlayerIndex];
        Debug.Log($"Current Player: {currentSlot.unit.unitName}, Ability: {currentSlot.abilityName}");
        bool hasMP = currentSlot.unit.UseMP(currentSlot.unit.amount);
        currentSlot.hud.SetMP(currentSlot.unit.currentMP);

        if (hasMP)
        {
            // Add logic for specific abilities (e.g., poison, evasion, etc.)
            if (currentSlot.abilityName == "Goodie Bag")
            {
                int bagDamage = Random.Range(1, 7);
                bool isDead = enemyUnit.TakeDamage(bagDamage);
                enemyHUD.SetHP(enemyUnit.currentHP);
                dialogueText.text = $"{currentSlot.unit.unitName} attacks for {bagDamage} damage using Goodie Bag!";

                yield return new WaitForSeconds(2f);

                if (isDead)
                {
                    state = BattleState.WON;
                    EndBattle();
                    yield break;
                }
            }
            else if (currentSlot.abilityName == "Evasive Stance")
            {
                evade = true;
                dialogueText.text = $"{currentSlot.unit.unitName} takes a defensive stance!";
            }
            else if (currentSlot.abilityName == "Toxic Vial")
            {
                enemyUnit.poison += 3;
                dialogueText.text = $"{currentSlot.unit.unitName} throws a toxic vial!";
            }
            else if (currentSlot.abilityName == "Swap Strike")
            {
                dialogueText.text = $"{currentSlot.unit.unitName} uses Swap Strike!";
                if (ice)
                {
                    currentSlot.unit.unitName = "Solis";
                    fire = true;
                    ice = false;
                }
                else
                {
                    currentSlot.unit.unitName = "Boreas";
                    fire = false;
                    ice = true;
                }
            }

            yield return new WaitForSeconds(2f);
            AdvanceToNextPlayer();
        }
        else
        {
            dialogueText.text = $"{currentSlot.unit.unitName} is out of MP!";
            yield return new WaitForSeconds(2f);
            StartPlayerTurn();
        }
    }

    void AdvanceToNextPlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= playerSlots.Count)
        {
            currentPlayerIndex = 0;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            StartPlayerTurn();
        }
    }

    IEnumerator EnemyTurn()
    {
        enemyUnit.TakeDamage(enemyUnit.poison);
        enemyHUD.SetHP(enemyUnit.currentHP);

        dialogueText.text = $"{enemyUnit.unitName} attacks!";
        yield return new WaitForSeconds(1f);

        target = Random.Range(0, playerSlots.Count);
        var targetSlot = playerSlots[target];
        bool isDead = false;

        if (evade == true)
        {
            dialogueText.text = $"{targetSlot.unit.unitName} evades the attack!";
            evade = false;
            targetSlot.unit.transform.DOMoveX(5.0f, .5f)
                .SetLoops(2, LoopType.Yoyo);
        }
        else
        {
            isDead = targetSlot.unit.TakeDamage(enemyUnit.damage);
            targetSlot.hud.SetHP(targetSlot.unit.currentHP);
        }

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            dialogueText.text = "Pandora's squad was defeated...";
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            StartPlayerTurn();
        }
    }

    public void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Pandora's squad won!";
            ReturntoGame.Invoke();
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Pandora's squad was defeated...";
            ReturntoGame.Invoke();
        }
    }

    public void OnAttackButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack());
        }
    }

    public void OnAbilityButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAbility());
        }
    }
}

