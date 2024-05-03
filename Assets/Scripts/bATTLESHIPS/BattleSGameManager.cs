using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSGameManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public GameObject player1Board;
    public GameObject player2Board;

    public GameObject player1Ships;
    public GameObject player2Ships;

    public bool player1Turn = true;
  
    public bool gameEnded = false;
    public bool player1Won = false;
  
    private int player1TurnCount = 0;//variable to ensure player only has 10 turns
    private int player2TurnCount = 0;
    private int maxTurns = 10;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Debug.Log("Game Initialized. Players take turns to place ships on the board.");
        MoveCameraToBoard(player1Board);
    }

    private void Update()
    { //checking if game is over and if not letting the turns run through
        if (!gameEnded)
        {
            if (player1Turn && player1TurnCount < maxTurns)
            {
                MoveCameraToBoard(player1Board);
                PlayerTurn(player1Board, player2Board, player1Ships);
                player1TurnCount++;
            }
            else if (!player1Turn && player2TurnCount < maxTurns)
            {
                MoveCameraToBoard(player2Board);
                PlayerTurn(player2Board, player1Board, player2Ships);
                player2TurnCount++;
            }
            else
            {
                gameEnded = true;
                DisplayGameResults(player1Ships);
            }
        }
    }
// method for moving camera to the side of the board of the playing player so that they cant see opposing board
    private void MoveCameraToBoard(GameObject board)
    {
        Vector3 boardCenter = board.transform.position + new Vector3(4.5f, 0, 4.5f);
        Vector3 cameraPosition = new Vector3(boardCenter.x, 10, boardCenter.z - 5);
        
        mainCamera.transform.position = cameraPosition;
    }

    private void PlayerTurn(GameObject attackingBoard, GameObject defendingBoard, GameObject defendingShips)
    {
        
        if (!gameEnded)
        {
            HandleInput(attackingBoard, defendingShips, defendingBoard);
        }
    }

    // Code to handle player turn, getting input, checking hits, marking results, checking for sunk ships, and ending game
    private void HandleInput(GameObject attackingBoard, GameObject defendingShips, GameObject defendingBoard)
    {
        //code for getting players input and maths for board placement
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.FloorToInt(clickPosition.x);
            int z = Mathf.FloorToInt(clickPosition.z);

            if (x >= 0 && x < 10 && z >= 0 && z < 10)
            {
                if (CheckHit(defendingBoard, x, z))
                {
                    Debug.Log("Hit at position: " + x + ", " + z);
                    if (CheckSunkShip(defendingShips, defendingBoard, x, z))
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
                    Debug.Log("Miss at position: " + x + ", " + z);
                    player1Turn = !player1Turn;
                }
            }
        }
    }
    
//method for placement of ship on board
    private void PlaceShip(GameObject ships, GameObject board, int x, int z)
    {
        if (CheckEmpty(board, x, z))
        {
            GameObject newShip = Instantiate(ships, new Vector3(x, 0, z), Quaternion.identity);
            newShip.transform.parent = board.transform;
            if (player1Turn)
            {
                player1Turn = false;
            }
            else
            {
                player1Turn = true;
            }
        }
    }

    //method to check if square on board is empty to verify placement
    private bool CheckEmpty(GameObject board, int x, int z)
    {
        foreach (Transform child in board.transform)
        {
            if (child.position == new Vector3(x, 0, z))
            {
                return false;
            }
        }

        return true;
    }
    
    
// boolean to check if enemy ship was hit on turn
private bool CheckHit(GameObject board, int x, int z)
{
    foreach (Transform child in board.transform)
    {
        if (child.position == new Vector3(x, 0, z))
        {
            return true;
        }
    }
    return false;
}


private bool CheckSunkShip(GameObject ships, GameObject board, int x, int z)
{
    // Code to check if a ship is sunk after a hit
    foreach (Transform child in ships.transform)
    {
        if (child.position == new Vector3(x, 0, z))
        {
            Ship health = child.GetComponent<Ship>();
            health.Hit();
            return health.IsSunk;
        }
    }
    return false;
}

//code to display the player board 
private void DisplayBoard(GameObject board)
{
    foreach (Transform child in board.transform)
    {
        Debug.Log("Ship at position: " + child.position);
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
    //code for displaying the game results
    if (player1Won)
    {
        Debug.Log("Player 1 wins!");
    }
    else
    {
        Debug.Log("Player 2 wins!");
    }
}

//declaring of ship class
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
