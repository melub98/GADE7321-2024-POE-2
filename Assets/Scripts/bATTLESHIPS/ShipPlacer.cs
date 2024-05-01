using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipPlacer : MonoBehaviour
{
    public GameObject[] shipPrefabs;
    public Transform boardTransform;
    public float cellSize;

    private List<GameObject> placedShips = new List<GameObject>();
    private GameObject currentShip;
    private static Vector2 startDragPosition;
    

    void Start()
    {
        // Create ship prefabs
        for (int i = 0; i < shipPrefabs.Length; i++)
        {
            GameObject ship = Instantiate(shipPrefabs[i]);
            ship.SetActive(false);
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Board")
                {
                    // Start dragging a new ship
                    currentShip = GetAvailableShip();
                    if (currentShip != null)
                    {
                        currentShip.SetActive(true);
                        currentShip.transform.position = hit.point;
                        startDragPosition = Input.mousePosition;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && currentShip != null)
        {
            // End dragging the ship
            currentShip.SetActive(false);
            PlaceShip();
        }
        else if (Input.GetMouseButton(0) && currentShip != null)
        {
            // Update ship position while dragging
            Vector2 mouseDelta = Input.mousePosition - (startDragPosition);
            currentShip.transform.position += new Vector3(mouseDelta.x, mouseDelta.y, 0);
        }
    }


    private GameObject GetAvailableShip()
    {
        foreach (GameObject ship in placedShips)
        {
            if (!ship.activeSelf)
            {
                return ship;
            }
        }
        return null;
    }

    private void PlaceShip()
    {
        // Check if the ship can be placed in its current position
        if (IsPositionValid(currentShip.transform.position))
        {
            // Snap the ship to the grid
            Vector2 gridPosition = GetGridPosition(currentShip.transform.position);
            currentShip.transform.position = gridPosition;

            // Add the ship to the placedShips list
            placedShips.Add(currentShip);

            // Check if all ships have been placed
            if (placedShips.Count == shipPrefabs.Length)
            {
                // Start the game
                // ...
            }
        }
        else
        {
            // Invalid position, reset the ship
            currentShip.transform.position = Vector3.zero;
            currentShip.SetActive(false);
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        // Check if the ship is within the bounds of the board
        if (position.x < 0 || position.x >= boardTransform.localScale.x * cellSize ||
            position.y < 0 || position.y >= boardTransform.localScale.y * cellSize)
        {
            return false;
        }

        // Check if the ship overlaps with any other placed ships
        foreach (GameObject placedShip in placedShips)
        {
            if (placedShip.activeSelf && Vector3.Distance(position, placedShip.transform.position) < cellSize)
            {
                return false;
            }
        }

        return true;
    }

    private Vector2 GetGridPosition(Vector3 position)
    {
        // Get the grid position in cell coordinates
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);

        // Snap the position to the center of the cell
        return new Vector2(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2);
    }

    
}

