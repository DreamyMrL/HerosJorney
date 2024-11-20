using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;

public enum BattleState { START, PLAYERTURN, PLAYERTURN2, PLAYERTURN3, PLAYERTURN4, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public TMP_Text dialogueText;
    public TMP_Text abilityText;
    public TMP_Text abilityText2;
    public TMP_Text abilityText3;
    public TMP_Text abilityText4;
    public BattleState state;

    public UnityEvent ReturntoGame;
    public UnityEvent SwaptoP1Buttons;
    public UnityEvent SwaptoP2Buttons;
    public UnityEvent SwaptoP3Buttons;
    public UnityEvent SwaptoP4Buttons;

    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;
    public GameObject enemyPrefab;

    public Transform playerposition;
    public Transform playerposition2;
    public Transform playerposition3;
    public Transform playerposition4;
    public Transform enemyposition;

    Unit playerUnit;
    Unit playerUnit2;
    Unit playerUnit3;
    Unit playerUnit4;
    Unit enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD playerHUD2;
    public BattleHUD playerHUD3;
    public BattleHUD playerHUD4;
    public BattleHUD enemyHUD;

    private bool evade = false;
    private int target;
    private int poisonchance;
    private bool ice = true;
    private bool fire = false;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject PlayerGO = Instantiate(playerPrefab, playerposition);
        playerUnit = PlayerGO.GetComponent<Unit>();
        GameObject Player2GO = Instantiate(playerPrefab2, playerposition2);
        playerUnit2 = Player2GO.GetComponent<Unit>();
        GameObject Player3GO = Instantiate(playerPrefab3, playerposition3);
        playerUnit3 = Player3GO.GetComponent<Unit>();
        GameObject Player4GO = Instantiate(playerPrefab4, playerposition4);
        playerUnit4 = Player4GO.GetComponent<Unit>();
        GameObject EnemyGO = Instantiate(enemyPrefab, enemyposition);
        enemyUnit = EnemyGO.GetComponent<Unit>();

        dialogueText.text = "A " + enemyUnit.unitName + " encounters Pandora's squad!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        playerHUD2.SetHUD(playerUnit2);
        playerHUD3.SetHUD(playerUnit3);
        playerHUD4.SetHUD(playerUnit4);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool IsDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " attacks for " + playerUnit.damage + " damage!";
        state = BattleState.PLAYERTURN2;
        yield return new WaitForSeconds(2f);

        if (IsDead)
        {
            dialogueText.text = "Pandora's squad won!";
            yield return new WaitForSeconds(5f);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            SwaptoP2Buttons.Invoke();
            PlayerTurn2();
        }
    }

    IEnumerator PlayerAttack2()
    {
        bool IsDead = enemyUnit.TakeDamage(playerUnit2.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit2.unitName + " attacks for " + playerUnit2.damage + " damage!";
        state = BattleState.PLAYERTURN3;
        yield return new WaitForSeconds(2f);

        if (IsDead)
        {
            dialogueText.text = "Pandora's squad won!";
            yield return new WaitForSeconds(5f);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            SwaptoP3Buttons.Invoke();
            PlayerTurn3();
        }
    }

    IEnumerator PlayerAttack3()
    {
        poisonchance = Random.Range(1, 11);
        bool IsDead = enemyUnit.TakeDamage(playerUnit3.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit3.unitName + " attacks for " + playerUnit3.damage + " damage!";
        if(poisonchance <= 2)
        {
            enemyUnit.poison += 1;
            dialogueText.text = playerUnit3.unitName + " inflicts a small poison!";
        }
        state = BattleState.PLAYERTURN4;
        yield return new WaitForSeconds(2f);

        if (IsDead)
        {
            dialogueText.text = "Pandora's squad won!";
            yield return new WaitForSeconds(5f);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            SwaptoP4Buttons.Invoke();
            PlayerTurn4();
        }
    }

    IEnumerator PlayerAttack4()
    {
        bool IsDead = false;
        if (ice = true)
        {
            enemyUnit.TakeDamage(playerUnit4.damage);
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = playerUnit4.unitName + " attacks for " + playerUnit4.damage + " ice damage!";
        }
        if (fire = true)
        {
            enemyUnit.TakeDamage(playerUnit4.damage);
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = playerUnit4.unitName + " attacks for " + playerUnit4.damage + " fire damage!";
        }
        state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(2f);

        if (IsDead)
        {
            dialogueText.text = "Pandora's squad won!";
            yield return new WaitForSeconds(5f);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            SwaptoP1Buttons.Invoke();
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator GoodieBag()
    {
        bool HasMP = playerUnit.UseMP(playerUnit.amount);
        playerHUD.SetMP(playerUnit.currentMP);
        if (HasMP)
        {
            playerUnit.bagdmg = Random.Range(1, 7);
            dialogueText.text = "Pandora opens her bag of goodies!";
            yield return new WaitForSeconds(2f);
            bool IsDead = enemyUnit.BagDamage(playerUnit.damage);
            enemyHUD.SetHP(enemyUnit.currentHP);
            if (playerUnit.bagdmg == 1)
            {
                dialogueText.text = playerUnit.unitName + " attacks for " + playerUnit.bagdmg + " damage...";
            }
            else
            {
                dialogueText.text = playerUnit.unitName + " attacks for " + playerUnit.bagdmg + " damage!";
            }
            state = BattleState.PLAYERTURN2;
            yield return new WaitForSeconds(2f);

            if (IsDead)
            {
                dialogueText.text = "Pandora's squad won!";
                yield return new WaitForSeconds(5f);
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                SwaptoP2Buttons.Invoke();
                PlayerTurn2();
            }
        }
        else
        {
            dialogueText.text = playerUnit.unitName + " is out of MP!";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "What will " + playerUnit.unitName + " do?";
        }
    }

    IEnumerator DanceEvasion()
    {
        bool HasMP = playerUnit2.UseMP(playerUnit2.amount);
        playerHUD2.SetMP(playerUnit2.currentMP);
        if (HasMP)
        {
            state = BattleState.PLAYERTURN3;
            evade = true;
            dialogueText.text = "The dancer prepares to avoid the enemy!";
            yield return new WaitForSeconds(2f);
            SwaptoP3Buttons.Invoke();
            PlayerTurn3();
        }
        else
        {
            dialogueText.text = playerUnit2.unitName + " is out of MP!";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "What will " + playerUnit2.unitName + " do?";
        }
    }

    IEnumerator ToxicVial()
    {
        bool HasMP = playerUnit3.UseMP(playerUnit3.amount);
        playerHUD3.SetMP(playerUnit3.currentMP);
        if (HasMP)
        {
            enemyUnit.poison += 3;
            dialogueText.text = "Ingrid throws a toxic vial!";
            yield return new WaitForSeconds(2f);
            SwaptoP4Buttons.Invoke();
            PlayerTurn4();
        }
        else
        {
            dialogueText.text = playerUnit3.unitName + " is out of MP!";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "What will " + playerUnit3.unitName + " do?";
        }
    }

    IEnumerator SwapStrike()
    {
        bool HasMP = playerUnit4.UseMP(playerUnit4.amount);
        playerHUD.SetMP(playerUnit4.currentMP);
        if (HasMP)
        {
            bool IsDead = enemyUnit.TakeDamage(playerUnit4.damage);
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = playerUnit4.unitName + " swaps!";
            if (ice == true)
            {
                playerUnit4.unitName = "Solis";
                fire = true;
            }
            else
            {
                playerUnit4.unitName = "Boreas";
                ice = true;
            }
            state = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(2f);
            if (IsDead)
            {
                dialogueText.text = "Pandora's squad won!";
                yield return new WaitForSeconds(5f);
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                SwaptoP1Buttons.Invoke();
                StartCoroutine(EnemyTurn());
            }
        }
        else
        {
            dialogueText.text = playerUnit4.unitName + " is out of MP!";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "What will " + playerUnit4.unitName + " do?";
        }
    }

    public void EndBattle()
    {
        if (state == BattleState.WON)
        {
            Overworld.EnemyDead = true;
            ReturntoGame.Invoke();
        }
        if(state == BattleState.LOST)
        {
            Overworld.EnemyDead = false;
            ReturntoGame.Invoke();
        }
    }

    IEnumerator EnemyTurn()
    {
        StartCoroutine(PoisonCheck());
        bool IsDead = false;
        dialogueText.text = enemyUnit.unitName + " attacks!";
        yield return new WaitForSeconds(1f);
        target = Random.Range(1, 5);
        if(target == 1)
        {
            IsDead = playerUnit.TakeDamage(enemyUnit.damage);
            playerHUD.SetHP(playerUnit.currentHP);
        }
        if(target == 2)
        {
            if(evade == true)
            {
                dialogueText.text = "The dancer evades!";
                evade = false;
                playerUnit2.transform.DOMoveX(5.0f, .5f)
                .SetLoops(2, LoopType.Yoyo);
            }
            else
            {
                IsDead = playerUnit2.TakeDamage(enemyUnit.damage);
                playerHUD2.SetHP(playerUnit2.currentHP);
            }
        }
        if (target == 3)
        {
            IsDead = playerUnit3.TakeDamage(enemyUnit.damage);
            playerHUD3.SetHP(playerUnit3.currentHP);
        }
        if (target == 4)
        {
            IsDead = playerUnit4.TakeDamage(enemyUnit.damage);
            playerHUD4.SetHP(playerUnit4.currentHP);
        }
        yield return new WaitForSeconds(1f);
        if (IsDead)
        {
            dialogueText.text = "Pandora's squad was defeated...";
            yield return new WaitForSeconds(5f);
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    IEnumerator PoisonCheck()
    {
        if(enemyUnit.poison >= 1)
        {
            bool IsDead = enemyUnit.PoisonDamage(enemyUnit.poison);
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "The enemy takes poison damage!";
            yield return new WaitForSeconds(2f);
            if (IsDead)
            {
                dialogueText.text = "Pandora's squad won!";
                yield return new WaitForSeconds(5f);
                state = BattleState.WON;
                EndBattle();
            }
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "What will " + playerUnit.unitName + " do?";
        abilityText.text = "Goodie Bag";
    }
    void PlayerTurn2()
    {
        dialogueText.text = "What will " + playerUnit2.unitName + " do?";
        abilityText2.text = "Evasive Stance";
    }
    void PlayerTurn3()
    {
        dialogueText.text = "What will " + playerUnit3.unitName + " do?";
        abilityText3.text = "Toxic Vial";
    }

    void PlayerTurn4()
    {
        dialogueText.text = "What will " + playerUnit4.unitName + " do?";
        abilityText4.text = "Swap Strike";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack());
        }
    }

    public void OnAttackButton2()
    {
        if (state != BattleState.PLAYERTURN2)
            return;
        if (state == BattleState.PLAYERTURN2)
        {
            StartCoroutine(PlayerAttack2());
        }
    }

    public void OnAttackButton3()
    {
        if (state != BattleState.PLAYERTURN3)
            return;
        if (state == BattleState.PLAYERTURN3)
        {
            StartCoroutine(PlayerAttack3());
        }
    }

    public void OnAttackButton4()
    {
        if (state != BattleState.PLAYERTURN4)
            return;
        if (state == BattleState.PLAYERTURN4)
        {
            StartCoroutine(PlayerAttack4());
        }
    }

    public void OnGoodieButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(GoodieBag());
        }
    }

    public void OnDanceButton()
    {
        if (state != BattleState.PLAYERTURN2)
            return;
        if (state == BattleState.PLAYERTURN2)
        {
            StartCoroutine(DanceEvasion());
        }
    }

    public void OnVialButton()
    {
        if (state != BattleState.PLAYERTURN3)
            return;
        if (state == BattleState.PLAYERTURN3)
        {
            StartCoroutine(ToxicVial());
        }
    }

    public void OnSwapButton()
    {
        if (state != BattleState.PLAYERTURN4)
            return;
        if (state == BattleState.PLAYERTURN4)
        {
            StartCoroutine(SwapStrike());
        }
    }
}
