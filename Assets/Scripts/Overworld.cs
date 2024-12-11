using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Overworld : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player.
    private Vector2 movement;    // Store player movement input
    private Rigidbody2D rb;      // Reference to the Rigidbody2D component.

    public static bool EnemyDead;

    public UnityEvent EnemyTrigger;
    public UnityEvent EnemyisDead;
    public UnityEvent CharacterGet;
    public UnityEvent ForestTrigger;
    public UnityEvent EndTrigger;

    public GameObject player;

    private bool Player2 = true;
    private bool Player3 = true;
    private bool Player4 = false;

    public static Vector3 PlayerPosition;

    private GameObject currentMember; // Stores current member for interaction

    void Start()
    {
        // Restore player position on scene start
        player.transform.position = PlayerPosition;

        // Initialize Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the Player!");
        }

        // Handle post-battle logic
        if (EnemyDead)
        {
            EnemyisDead.Invoke();
        }
    }

    void Update()
    {
        // Get input from the player (WASD / Arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize to prevent diagonal movement from being faster
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Move the player by adjusting the Rigidbody2D position
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.gameObject.name);

        switch (other.gameObject.tag)
        {
            case "Slime":
                BattleSystem.slime = true;
                PlayerPosition = player.transform.position;
                EnemyTrigger.Invoke();
                break;

            case "Plant":
                BattleSystem.plant = true;
                PlayerPosition = player.transform.position;
                EnemyTrigger.Invoke();
                break;

            case "Forest":
                ForestTrigger.Invoke();
                break;

            case "End":
                EndTrigger.Invoke();
                break;

            case "Member":
                currentMember = other.gameObject; // Save reference for interaction
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Member")
        {
            currentMember = null; // Clear reference to member
        }
    }
}
