using System;
using UnityEngine;

public class MenuUI : MonoBehaviour, IUIPanel
{
    public Action<GameDifficultyMode> StartButtonPressed;

    [SerializeField]
    private Canvas _canvas = default;

    public void SetVisibility(bool isVisible)
    {
        _canvas.gameObject.SetActive(isVisible);
    }

    public void OnEasyButtonPressed()
    {
        StartButtonPressed?.Invoke(GameDifficultyMode.Easy);
    }

    public void OnDefaultButtonPressed()
    {
        StartButtonPressed?.Invoke(GameDifficultyMode.Default);
    }

    public void OnHardButtonPressed()
    {
        StartButtonPressed?.Invoke(GameDifficultyMode.Hard);
    }

    private void OnStartButtonPressed(GameDifficultyMode difficultyMode)
    {
        StartButtonPressed?.Invoke(difficultyMode);
    }

    private void Awake()
    {
        Debug.Assert(_canvas != null, "Missing reference!");
    }
}