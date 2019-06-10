using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI scoreDisplay;
    GameSession gameSession;
    
    void Start()
    {
        scoreDisplay = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {      
        scoreDisplay.text = gameSession.getScore().ToString();
    }
}
