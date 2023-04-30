using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public Text p1ScoreText;
    public Text p2ScoreText;

    public void UpdateScore()
    {
        p1ScoreText.text = $"P1(B): {OthelloManager.instance.P1Score}";
        p2ScoreText.text = $"P2(W): {OthelloManager.instance.P2Score}";
    }
}
