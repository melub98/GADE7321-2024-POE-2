using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isOccupied = false; // Flag to determine if a ship is occupying the tile
    public bool isHit = false; // Flag to determine if the tile has been hit

    private BattleSGameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<BattleSGameManager>();
    }
// Method to mark the tile as occupied by a ship
    public void OccupyTile()
    {
        isOccupied = true;
    }
// Method to mark the tile as hit
    public void MarkAsHit()
    {
        isHit = true;
        gameObject.GetComponent<Renderer>().material.color = Color.red; // Change the color of the tile to red when hit
    }
// Method to check if the tile is occupied by a ship
    public bool IsOccupied()
    {
        return isOccupied;
    }
// Method to check if the tile has been hit
    public bool IsHit()
    {
        return isHit;
    }
}
