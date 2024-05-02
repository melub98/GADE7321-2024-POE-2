using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSGameManager : MonoBehaviour
{
    
    public GameObject player1Board;
    public GameObject player2Board;

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
        Debug.Log("Game Initialized. Players take turns to place ships on the board.");
        MoveCameraToBoard(player1Board);
    }

    private void Update()
    {
        if (!gameEnded)
        {
            if (player1Turn)
            {
                MoveCameraToBoard(player1Board);
                PlayerTurn(player1Board, player2Board, player1Ships);
            }
            else
            {
                MoveCameraToBoard(player2Board);
                PlayerTurn(player2Board, player1Board, player2Ships);
            }
        }
    }

    private void MoveCameraToBoard(GameObject board)
    {
        Vector3 boardCenter = board.transform.position + new Vector3(4.5f, 0, 4.5f);
        Vector3 cameraPosition = new Vector3(boardCenter.x, 10, boardCenter.z - 5);
        Camera.main.transform.position = cameraPosition;
    }

    private void PlayerTurn(GameObject attackingBoard, GameObject defendingBoard, GameObject defendingShips)
    {
        if (!gameEnded)
        {
            HandleInput(attackingBoard, defendingShips, defendingBoard);
        }
    }

    private void HandleInput(GameObject attackingBoard, GameObject defendingShips, GameObject defendingBoard)
    {
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

private void DisplayBoard(GameObject board)
{
    foreach (Transform child in board.transform)
    {
        Debug.Log("Ship at position: " + child.position);
    }
}

private bool CheckWinCondition(GameObject ships)
{
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
    if (player1Won)
    {
        Debug.Log("Player 1 wins!");
    }
    else
    {
        Debug.Log("Player 2 wins!");
    }
}

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
