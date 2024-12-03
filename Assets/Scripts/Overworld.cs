using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Overworld : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player.
    private Vector2 movement;     // Store player movement input

    private Rigidbody2D rb;       // Reference to the Rigidbody2D component.

    public static bool EnemyDead;

    public UnityEvent EnemyTrigger;
    public UnityEvent EnemyisDead;
    public UnityEvent CharacterGet;

    public GameObject player;
    public GameObject Popup;


    private bool Player2 = true;
    private bool Player3 = true;
    private bool Player4 = false;

    public static Vector3 PlayerPosition;

    void Start()
    {
        if (EnemyDead == true)
        {
            EnemyisDead.Invoke();
        }
        player.transform.position = PlayerPosition;
        // Get the Rigidbody2D component attached to the player.
        rb = GetComponent<Rigidbody2D>();

        // Debug: Check if Rigidbody2D is missing.
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the Player!");
        }
    }
    void Update()
    {
        // Get input from the player (WASD / Arrow keys).
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize to prevent diagonal movement from being faster.
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Move the player by adjusting the Rigidbody2D position.
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) //anything that enters this trigger causes this to run
    {
        if(other.gameObject.tag == "Slime")
        {
            BattleSystem.slime = true;
            PlayerPosition = player.transform.position;
            EnemyTrigger.Invoke();
        }
        if (other.gameObject.tag == "Plant")
        {
            BattleSystem.plant = true;
            PlayerPosition = player.transform.position;
            EnemyTrigger.Invoke();
        }
        if (other.gameObject.tag == "Member")
        {
            Popup.SetActive(true);
            if(Input.GetKeyDown(KeyCode.E))
            {
                CharacterGet.Invoke();
                other.gameObject.SetActive(false);
                if(other.gameObject.name == "Dancer")
                {
                    if(Player2 == false)
                    {
                        Player2 = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) //anything that enters this trigger causes this to run
    {
        if (other.gameObject.tag == "Member")
        {
            Popup.SetActive(false);
        }
    }
}