using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{

    // Player variables
    public float speed = 5f;
    public float jumpForce = 10f;
    public Rigidbody rb;
    public Animator anim;

    // Ragdoll variables
    public GameObject ragdollPrefab;
    private bool isRagdoll = false;
    private float ragdollTimer = 0f;

    void Update() {
        // Check if the player is in ragdoll mode
        if (isRagdoll) {
            ragdollTimer += Time.deltaTime;
            if (ragdollTimer >= 3f) {
                EndRagdoll();
            }
            return;
        }

        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput);
        movementDirection.Normalize();
        rb.velocity = movementDirection * speed;

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Animation
        anim.SetFloat("Speed", movementDirection.magnitude);
        anim.SetBool("IsGrounded", IsGrounded());
    }

    void OnTriggerEnter(Collider other) 
    {
        // Check if the player is hit by a shot
        if (other.CompareTag("Bullet") && !isRagdoll) {
            // Check if the player is on their last turn and has the headshot powerup
            if (currentTurn % 2 == 0 && player1.GetComponent<Shooting>().hasHeadshotPowerup) {
                // Deal instant headshot damage
                player2Health -= 10;
                player2HealthText.text = "Player 2 Health: " + player2Health;
                StartRagdoll();
            } else if (currentTurn % 2 == 1 && player2.GetComponent<Shooting>().hasHeadshotPowerup) {
                // Deal instant headshot damage
                player1Health -= 10;
                player1HealthText.text = "Player 1 Health: " + player1Health;
                StartRagdoll();
            } else {
                // Deal regular damage based on body part
                TakeDamage(other.GetComponent<Bullet>().damage, other.GetComponent<RaycastHit>());
            }
        }
    }

    void StartRagdoll() {
        // Instantiate ragdoll prefab
        GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        ragdoll.transform.parent = null;

        // Disable player movement and animation
        rb.isKinematic = true;
        anim.enabled = false;

        // Set isRagdoll to true and start timer
        isRagdoll = true;
        ragdollTimer = 0f;
    }

    void EndRagdoll() {
        // Destroy ragdoll
        Destroy(gameObject);

        // Re-enable player movement and animation
        rb.isKinematic = false;
        anim.enabled = true;

        // Set isRagdoll to false
        isRagdoll = false;
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
    
    
}
