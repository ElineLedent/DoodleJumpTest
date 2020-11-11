using UnityEngine;
using UnityEngine.UI;

public class OutroUI : MonoBehaviour, IUIPanel
{
    [SerializeField]
    private Canvas _canvas = default;

    [SerializeField]
    private GameObject _endScorePanel = default;

    [SerializeField]
    private GameObject _newHighScorePanel = default;

    [SerializeField]
    private Text _endScoreText = default;

    [SerializeField]
    private Text _highScoreText = default;

    [SerializeField]
    private Text _newHighScoreText = default;

    public void ShowEndScore(int score, int highScore)
    {
        _endScoreText.text = score.ToString();
        _highScoreText.text = highScore.ToString();

        _endScorePanel.SetActive(true);
    }

    public void ShowNewHighScore(int highScore)
    {
        _newHighScoreText.text = highScore.ToString();

        _newHighScorePanel.SetActive(true);
    }

    public void SetVisibility(bool isVisible)
    {
        _canvas.gameObject.SetActive(isVisible);
    }

    public void Reset()
    {
        _endScorePanel.SetActive(false);
        _newHighScorePanel.SetActive(false);
    }

    private void Awake()
    {
        Debug.Assert(_canvas != null, "Missing reference!");
        Debug.Assert(_endScoreText != null, "Missing reference!");
        Debug.Assert(_highScoreText != null, "Missing reference!");
        Debug.Assert(_newHighScoreText != null, "Missing reference!");

        _endScorePanel.SetActive(false);
        _newHighScorePanel.SetActive(false);
    }
}