using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    // Enemy health
    [SerializeField] float health, maxHealth = 2f;

    public GameObject player;
    public float speed;

    private float distance;
    private Animator animator;

    // New variable to track if the enemy is following the player
    private bool isFollowingPlayer = true;

    private void Start()
    {
        // Health system
        health = maxHealth;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isFollowingPlayer)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();

            // Calculate the movement vector
            Vector2 moveDirection = direction * speed * Time.deltaTime;

            // Move the Fireball towards the player
            transform.Translate(moveDirection);

            // Set the isMoving parameter in the animator based on the magnitude of moveDirection
            animator.SetBool("isMoving", moveDirection.magnitude > 0f);

            // Update the blend tree parameter for horizontal movement
            animator.SetFloat("horizontalMovement", moveDirection.x);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount; // 3 -> 2 -> 1 -> 0 = Enemy has died

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Stop following the player when the enemy dies
        isFollowingPlayer = false;

        // Debug
        Debug.Log("Enemy Fireball died!");

        // Disable the BoxCollider component to prevent further collisions
        GetComponent<BoxCollider2D>().enabled = false;

        // Play the death animation by setting the "isDead" parameter to true
        animator.SetBool("isDead", true);

        // Destroy the enemy GameObject after some time (adjust the delay as needed)
        float deathAnimationDuration = 1.2f; // Replace with the actual duration of the death animation
        Destroy(gameObject, deathAnimationDuration);
    }
}