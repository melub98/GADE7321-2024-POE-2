using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GManager : MonoBehaviour
{
    public GameObject player1Board;
    public GameObject player2Board;
    public List<GameObject> player1Ships;
    public List<GameObject> player2Ships;
    private int currentPlayerIndex;

    private int[,] player1BoardState;
    private int[,] player2BoardState;
    private bool isPlayer1Turn;
    

    void Start()
    {
        isPlayer1Turn = true;
        player1BoardState = new int[10, 10];
        player2BoardState = new int[10, 10];
        InitializeBoards(player1BoardState);
        InitializeBoards(player2BoardState);
        PlaceShips(player1BoardState, player1Ships);
        PlaceShips(player2BoardState, player2Ships);
        // ... (Initialize boards and ships)
        DisplayBoard(player1BoardState);
        DisplayBoard(player2BoardState, showShips: true);
    }

    void Update()
    {
        if (isPlayer1Turn)
        {
            currentPlayerIndex = 0;
            // Get input from Player 1
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player2Board")
                {
                    int x = (int)hit.point.x;
                    int y = (int)hit.point.z;

                    if (player2BoardState[x, y] == 0)
                    {
                        // Miss
                        player2BoardState[x, y] = -1;
                        isPlayer1Turn = false;
                    }
                    else if (player2BoardState[x, y] == 1)
                    {
                        // Hit
                        player2BoardState[x, y] = 2;
                        CheckForSunkShip(player2BoardState, x, y);
                        if (CheckForGameOver(player2BoardState))
                        {
                            Debug.Log("Player 1 Wins!");
                        }
                        else
                        {
                            // Player 1 gets another turn
                        }
                    }
                }
            }
        }
        else
        {
            currentPlayerIndex = 1;
            // Get input from Player 2
            // (similar to Player 1)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player1Board")
                {
                    int x = (int)hit.point.x;
                    int y = (int)hit.point.z;

                    if (player1BoardState[x, y] == 0)
                    {
                        // Miss
                        player1BoardState[x, y] = -1;
                        isPlayer1Turn = false;
                    }
                    else if (player1BoardState[x, y] == 1)
                    {
                        // Hit
                        player1BoardState[x, y] = 2;
                        CheckForSunkShip(player1BoardState, x, y);
                        if (CheckForGameOver(player1BoardState))
                        {
                            Debug.Log("Player 2 Wins!");
                        }
                        else
                        {
                            // Player 2 gets another turn
                        }
                    }
                }
            }
            isPlayer1Turn = true;
        }
        if (CheckForGameOver(player2BoardState))
        {
            Debug.Log("Player 1 Wins!");
        }
        else if (CheckForGameOver(player1BoardState))
        {
            Debug.Log("Player 2 Wins!");
        }
    }
    public class Ship
    {
        public string Type { get; set; }
        public string Orientation { get; set; }
        public (int, int) StartingCoordinates { get; set; }
        public int Length { get; set; }
        public List<(int, int)> Segments { get; set; }
        public bool Sunk { get; set; }

        public Ship(string type, string orientation, (int, int) startingCoordinates, int length)
        {
            Type = type;
            Orientation = orientation;
            StartingCoordinates = startingCoordinates;
            Length = length;
            Segments = GenerateSegments();
            Sunk = false;
        }

        private List<(int, int)> GenerateSegments()
        {
            List<(int, int)> segments = new List<(int, int)>();
            for (int i = 0; i < Length; i++)
            {
                int x = StartingCoordinates.Item1;
                int y = StartingCoordinates.Item2;
                if (Orientation == "horizontal")
                {
                    x += i;
                }
                else if (Orientation == "vertical")
                {
                    y += i;
                }
                segments.Add((x, y));
            }
            return segments;
        }
    }

    private void InitializeBoards(int[,] boardState)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                boardState[i, j] = 0;
            }
        }
    }

    private void PlaceShips(int[,] boardState, List<GameObject> ships)
    {
        foreach (GameObject ship in ships)
        {
            int shipLength = ship.GetComponent<Ship>().Length;
            bool isHorizontal = Random.Range(0, 2) == 0;
            int x, y;

            do
            {
                x = Random.Range(0, 10 - shipLength);
                y = Random.Range(0, 10 - (isHorizontal ? 0 : shipLength - 1));
            } while (!CanPlaceShip(boardState, x, y, shipLength, isHorizontal));

            for (int i = 0; i < shipLength; i++)
            {
                boardState[x + (isHorizontal ? i : 0), y + (isHorizontal ? 0 : i)] = 1;
            }
        }
    }

    private bool CanPlaceShip(int[,] boardState, int x, int y, int shipLength, bool isHorizontal)
    {
        for (int i = 0; i < shipLength; i++)
        {
            int xPos = x + (isHorizontal ? i : 0);
            int yPos = y + (isHorizontal ? 0 : i);

            if (xPos < 0 || xPos >= 10 || yPos < 0 || yPos >= 10)
            {
                return false;
            }

            if (boardState[xPos, yPos] != 0)
            {
                return false;
            }
        }

        return true;
    }
    
    public void DisplayBoard(int[,] boardState, bool showShips = false)
    {
        for (int y = 9; y >= 0; y--)
        {
            for (int x = 0; x < 10; x++)
            {
                if (boardState[x, y] == 0)
                {
                    Console.Write("~");
                }
                else if (boardState[x, y] == -1)
                {
                    Console.Write("X");
                }
                else if (boardState[x, y] == 2)
                {
                    Console.Write("H");
                }
                else if (showShips && boardState[x,y] == 1)
                {
                    Console.Write("S");
                }
                else
                {
                    Console.Write("?");
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        
    }
    
    public List<Vector2Int> CheckSurroundingSquares(int[,] board, Vector2Int hitPosition)
    {
        List<Vector2Int> adjacentSquares = new List<Vector2Int>();

        // Check up
        if (hitPosition.y > 0 && board[hitPosition.y - 1, hitPosition.x] == 1)
        {
            adjacentSquares.Add(new Vector2Int(hitPosition.x, hitPosition.y - 1));
        }

        // Check down
        if (hitPosition.y < board.GetLength(1) - 1 && board[hitPosition.y + 1, hitPosition.x] == 1)
        {
            adjacentSquares.Add(new Vector2Int(hitPosition.x, hitPosition.y + 1));
        }

        // Check left
        if (hitPosition.x > 0 && board[hitPosition.y, hitPosition.x - 1] == 1)
        {
            adjacentSquares.Add(new Vector2Int(hitPosition.x - 1, hitPosition.y));
        }

        // Check right
        if (hitPosition.x < board.GetLength(0) - 1 && board[hitPosition.y, hitPosition.x + 1] == 1)
        {
            adjacentSquares.Add(new Vector2Int(hitPosition.x + 1, hitPosition.y));
        }

        return adjacentSquares;
    }

    private void CheckForSunkShip(int[,] boardState, int x, int y)
    {
        // Check if the surrounding squares are also hits
        // If they are, recursively call this function to check for more sunk ships
        // If a ship is sunk, mark all its squares as 3 and remove the ship from the list
        // If all ships are sunk, end the game
    }


    private bool CheckForGameOver(int[,] boardState)
    {
        // Check if there are any remaining ships on the board
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (boardState[i, j] == 1)
                {
                    return false;// un-sunk ship segment found
                }
            }
        }

        // If no ships remain, the game is over
        return true;
    }
}
