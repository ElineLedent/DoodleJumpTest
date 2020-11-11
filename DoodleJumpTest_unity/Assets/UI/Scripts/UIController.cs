using System;
using System.Collections.Generic;
using UnityEngine;

public enum UITypes { Menu, Intro, Game, Outro };

public class UIController : MonoBehaviour
{
    public Action<GameDifficultyMode> StartButtonPressed;

    [SerializeField]
    private MenuUI _menuUI = default;

    [SerializeField]
    private IntroUI _introUI = default;

    [SerializeField]
    private GameUI _gameUI = default;

    [SerializeField]
    private OutroUI _outroUI = default;

    private Dictionary<UITypes, IUIPanel> _uiPanels = new Dictionary<UITypes, IUIPanel>();

    public void SetVisibleUI(UITypes uiType)
    {
        foreach (KeyValuePair<UITypes, IUIPanel> uiPanel in _uiPanels)
        {
            uiPanel.Value.SetVisibility(uiPanel.Key == uiType);
        }
    }

    private void OnStartButtonPressed(GameDifficultyMode difficultyMode)
    {
        StartButtonPressed?.Invoke(difficultyMode);
    }

    public void UpdateCountdown(int counter)
    {
        _introUI.UpdateCountdown(counter);
    }

    public void UpdateGameUI(int currentScore)
    {
        _gameUI.UpdateGameUI(currentScore);
    }

    public void ShowEndScore(int score, int highScore)
    {
        _outroUI.ShowEndScore(score, highScore);
    }

    public void ShowNewHighScore(int highScore)
    {
        _outroUI.ShowNewHighScore(highScore);
    }

    public void ResetUI()
    {
        _introUI.Reset();
        _gameUI.Reset();
        _outroUI.Reset();
    }

    private void Awake()
    {
        Debug.Assert(_menuUI != null, "Missing reference");
        Debug.Assert(_introUI != null, "Missing reference");
        Debug.Assert(_gameUI != null, "Missing reference");
        Debug.Assert(_outroUI != null, "Missing reference");

        _uiPanels.Add(UITypes.Menu, _menuUI);
        _uiPanels.Add(UITypes.Intro, _introUI);
        _uiPanels.Add(UITypes.Game, _gameUI);
        _uiPanels.Add(UITypes.Outro, _outroUI);
    }

    private void OnEnable()
    {
        _menuUI.StartButtonPressed += OnStartButtonPressed;
    }

    private void OnDisable()
    {
        _menuUI.StartButtonPressed -= OnStartButtonPressed;
    }
}