using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Camera ARCamera;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float currentScore = 0;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateScore(float points)
    {
        currentScore += points;
        scoreText.text = $"Score: {currentScore}";
    }
}
