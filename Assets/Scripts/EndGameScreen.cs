using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    public event Action RestartGame;
    public event Action ExitGame;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    public void EnableScreen(float score)
    {
        _screenVisabilityHandler.EnableScreen();

        if (_scoreText != null)
            _scoreText.text = score.ToString("F2");
    }
    
    public void EnableScreen(string text)
    {
        _screenVisabilityHandler.EnableScreen();

        if (_scoreText != null)
            _scoreText.text = text;
    }
    
    public void DisableScreen()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void OnRestartGame()
    {
        RestartGame?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    public void OnExitGame()
    {
        ExitGame?.Invoke();
    }
}