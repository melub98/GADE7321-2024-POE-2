
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
        // Player variables
        public GameObject player1;
        public GameObject player2;
        public int player1Health = 100;
        public int player2Health = 100;
    
        // Turn variables
        public int currentTurn = 1;
        public int maxTurns = 10;
    
        // UI variables
        public Text player1HealthText;
        public Text player2HealthText;
        public Text turnText;
    
        void Start() {
            // Set initial health and turn text
            player1HealthText.text = "Player 1 Health: " + player1Health;
            player2HealthText.text = "Player 2 Health: " + player2Health;
            turnText.text = "Turn: " + currentTurn;
        }
    
        void Update() {
            // Check if the game is over
            if (player1Health <= 0 || player2Health <= 0) {
                GameOver();
                return;
            }
    
            // Check if the current turn is over
            if (Input.GetKeyDown(KeyCode.Space)) {
                EndTurn();
            }
        }
    
        void EndTurn() {
            // Switch turns
            currentTurn++;
            if (currentTurn > maxTurns) {
                GameOver();
                return;
            }
    
            // Update turn text
            turnText.text = "Turn: " + currentTurn;
    
            // Switch active player
            if (currentTurn % 2 == 0) {
                player1.SetActive(true);
                player2.SetActive(false);
            } else {
                player1.SetActive(false);
                player2.SetActive(true);
            }
        }
    
        void GameOver() {
            // Determine the winner
            string winner = player1Health > player2Health ? "Player 1 Wins!" : "Player 2 Wins!";
    
            // Display the game over message
            Debug.Log(winner);
        }
    
        public void TakeDamage(int damage, RaycastHit hit) {
            // Determine the body part hit
            BodyPart bodyPart = hit.collider.GetComponent<BodyPart>();
    
            // Apply damage based on body part
            switch (bodyPart) {
                case BodyPart.Head:
                    player1Health -= 10;
                    break;
                case BodyPart.Torso:
                    player1Health -= 5;
                    break;
                case BodyPart.Feet:
                    player1Health -= 1;
                    break;
            }
    
            // Update health text
            player1HealthText.text = "Player 1 Health: " + player1Health;
        }
    }
    
    // Body part enum
    public enum BodyPart {
        Head,
        Torso,
        Feet
    }

