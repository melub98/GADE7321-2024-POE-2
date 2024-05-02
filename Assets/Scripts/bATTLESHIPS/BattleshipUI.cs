using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleshipUI : MonoBehaviour
{
    public Text turnText;
    public Text resultText;

    private BattleSGameManager game;

    private void Start()
    {
        game = FindObjectOfType<BattleSGameManager>();
        UpdateUIText();
    }

    private void Update()
    {
        if (game.gameEnded)
        {
            UpdateResultText();
        }
    }

    private void UpdateUIText()
    {
        if (game.player1Turn)
        {
            turnText.text = "Player 1's Turn";
        }
        else
        {
            turnText.text = "Player 2's Turn";
        }
    }

    private void UpdateResultText()
    {
        if (game.player1Won)
        {
            resultText.text = "Player 1 wins!";
        }
        else
        {
            resultText.text = "Player 2 wins!";
        }
    }
}