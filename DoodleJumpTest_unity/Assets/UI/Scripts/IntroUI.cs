using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour, IUIPanel
{
    [SerializeField]
    private Canvas _canvas = default;

    [SerializeField]
    private Text _countdownText = default;

    public void SetVisibility(bool isVisible)
    {
        _canvas.gameObject.SetActive(isVisible);
    }

    public void UpdateCountdown(int counter)
    {
        _countdownText.text = counter.ToString();
    }

    public void Reset()
    {
        _countdownText.text = "";
    }

    private void Awake()
    {
        Debug.Assert(_canvas != null, "Missing reference!");
        Debug.Assert(_countdownText != null, "Missing reference!");
    }
}