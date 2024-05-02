using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleshipGame : MonoBehaviour
{
    public GameObject player1Board;
    public GameObject player2Board;
    public GameObject defendingBoard; // Declaring the defendingBoard variable

    public GameObject player1Ships;
    public GameObject player2Ships;

    public bool player1Turn = true;
    public bool gameEnded = false;
    public bool player1Won = false;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Code to create boards and place ships for player 1
        // Code to create boards and place ships for player 2
        PlaceShipsRandomly(player1Ships, player1Board);
        PlaceShipsRandomly(player2Ships, player2Board);
      Debug.Log("Game Initialized. Player 1 starts.");
    }

    private void PlaceShipsRandomly(GameObject ships, GameObject board)
    {
        // Code to randomly place ships on the board
      int shipCount = 0;
      while (shipCount < 3)
      {
          int randomX = Random.Range(0, 10);
          int randomY = Random.Range(0, 10);
          if (CheckEmpty(board, randomX, randomY))
          {
              GameObject newShip = Instantiate(ships, new Vector3(randomX, 0, randomY), Quaternion.identity);
              newShip.transform.parent = board.transform;
              shipCount++;
          }
      }
    }

    private void Update()
    {
        if (!gameEnded)
        {
            if (player1Turn)
            {
                PlayerTurn(player1Board, player2Board, player1Ships);
            }
            else
            {
                PlayerTurn(player2Board, player1Board, player2Ships);
            }
        }
    }

  private bool CheckEmpty(GameObject board, int x, int y)
  {
      foreach (Transform child in board.transform)
      {
          if (child.position == new Vector3(x, 0, y))
          {
              return false;
          }
      }
      return true;
  }

    private void PlayerTurn(GameObject attackingBoard, GameObject defendingBoard, GameObject defendingShips)
    {
        // Code to handle player turn, getting input, checking hits, marking results, checking for sunk ships, and ending game
      {
          Debug.Log("Player takes a turn.");
          HandleInput(attackingBoard, defendingShips, defendingBoard);
      }
    }

    private bool CheckWinCondition(GameObject ships)
    {
        // Code to check if a player has won by sinking all opponent's ships
      int shipCount = 0;
      foreach (Transform child in ships.transform)
      {
          Ship health = child.GetComponent<Ship>();
          if (health.IsSunk)
          {
              shipCount++;
          }
      }
      return shipCount == 3;
    }

    private void DisplayGameResults(GameObject ships)
    {
        // Code to display game results, including the winner and sunk ships
      if (player1Won)
      {
          Debug.Log("Player 1 wins!");
      }
      else
      {
          Debug.Log("Player 2 wins!");
      }
    }

    // Additional methods for UI display, handling input, ship placement, different ships, and game rules

    private void DisplayBoard(GameObject board)
    {
        // Code to display the game board for a player
      foreach (Transform child in board.transform)
      {
          Debug.Log("Ship at position: " + child.position);
      }
    }

    private void HandleInput(GameObject attackingBoard, GameObject defendingShips, GameObject defendingBoard)
    {
        int randomX = Random.Range(0, 10);
        int randomY = Random.Range(0, 10);

        if (CheckHit(defendingBoard, randomX, randomY))
        {
            Debug.Log("Hit at position: " + randomX + ", " + randomY);
            if (CheckSunkShip(defendingShips, defendingBoard, randomX, randomY))
            {
                Debug.Log("Ship sunk!");
                if (CheckWinCondition(defendingShips))
                {
                    gameEnded = true;
                    player1Won = !player1Turn;
                    DisplayGameResults(defendingShips);
                }
            }
        }
        else
        {
            Debug.Log("Miss at position: " + randomX + ", " + randomY);
            player1Turn = !player1Turn;
        }
    }
    
    
  
    private bool CheckHit(GameObject board, int x, int y)
    {
        // Code to check if the attack hits a ship on the board
        foreach (Transform child in board.transform)
        {
            if (child.position == new Vector3(x, 0, y))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckSunkShip(GameObject ships, GameObject board, int x, int y)
    {
        // Code to check if a ship is sunk after a hit
        foreach (Transform child in ships.transform)
        {
            if (child.position == new Vector3(x, 0, y))
            {
                Ship health = child.GetComponent<Ship>();
                health.Hit();
                return health.IsSunk;
            }
        }
        return false;
    }

    private void RemoveSunkShip(GameObject ships, string shipType, float x, float y)
    {
        // Code to remove a sunk ship from the list of ships
      foreach (Transform child in ships.transform)
      {
          if (child.position == new Vector3(x, 0, y))
          {
              Destroy(child.gameObject);
              break;
          }
      }
    }

    // Define ship classes: Carrier, Battleship, Cruiser, Submarine, Destroyer

    public class Ship 
    {
        public string ShipType { get; set; }
        public int Size { get; set; }
        public int Hits { get; set; }
        public bool IsSunk => Hits == Size;

        public Ship(string shipType, int size)
        {
            ShipType = shipType;
            Size = size;
            Hits = 0;
        }

        public void Hit()
        {
            Hits++;
        }
    }
}
