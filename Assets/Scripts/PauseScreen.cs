using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public event Action RestartGame;
    public event Action ContinueGame;
    public event Action ExitGame;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    public void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void DisableScreen()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void OnRestartGame()
    {
        RestartGame?.Invoke();
        DisableScreen();
    }

    public void OnExitGame()
    {
        ExitGame?.Invoke();
    }

    public void OnContinueGame()
    {
        ContinueGame?.Invoke();
        DisableScreen();
    }
}
