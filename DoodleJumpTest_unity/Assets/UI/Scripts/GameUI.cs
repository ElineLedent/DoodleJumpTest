using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour, IUIPanel
{
    [SerializeField]
    private Canvas _canvas = default;

    [SerializeField]
    private Text _scoreText = default;

    public void SetVisibility(bool isVisible)
    {
        _canvas.gameObject.SetActive(isVisible);
    }

    public void UpdateGameUI(int currentScore)
    {
        _scoreText.text = currentScore.ToString();
    }

    public void Reset()
    {
        UpdateGameUI(0);
    }

    private void Awake()
    {
        Debug.Assert(_canvas != null, "Missing reference!");
        Debug.Assert(_scoreText != null, "Missing reference!");
    }
}