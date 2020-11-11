using System.Collections;
using UnityEngine;

public enum GameDifficultyMode { Easy, Default, Hard };

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CameraController _cameraController = default;

    [SerializeField]
    private WorldController _worldController = default;

    [SerializeField]
    private UIController _uiController = default;

    [SerializeField]
    private GlobalAudioManager _globalAudioManager = default;

    [SerializeField]
    private Player _playerPrefab = default;

    [SerializeField]
    private float _playerStartHeightOffset = 2f;

    private ScoreManager _scoreManager;
    private Player _player;

    public GameDifficultyMode _difficultyMode = GameDifficultyMode.Default;

    private int _countdownStartValue = 3;
    private float _countdownUpdateInterval = 1f;
    private float _gameOutroDuration = 5f;
    private bool _startButtonPressed = false;

    private void OnStartButtonPressed(GameDifficultyMode difficultyMode)
    {
        _difficultyMode = difficultyMode;
        _startButtonPressed = true;

        _worldController.Initialize(_cameraController, _difficultyMode);

        _globalAudioManager.PlayAudioUIConfirmation();
    }

    private void OnPlatformJumpedCountChanged(int platformsJumpedCount)
    {
        _scoreManager.SetCurrentScore(platformsJumpedCount);
        _uiController.UpdateGameUI(_scoreManager.CurrentScore);
    }

    private IEnumerator ShowMenu()
    {
        _startButtonPressed = false;
        _uiController.ResetUI();
        _uiController.SetVisibleUI(UITypes.Menu);

        _globalAudioManager.PlayAudioMenuMusic();

        while (_startButtonPressed == false)
        {
            yield return null;
        }

        StartNewGame();
    }

    private void StartNewGame()
    {
        InitializeNewGame();

        StartCoroutine(GameIntro());
    }

    private void InitializeNewGame()
    {
        CreateWorld();

        _player = SpawnPlayer();
        _cameraController.Initialize(_player);

        _player.PlatformsJumpedCountChanged += OnPlatformJumpedCountChanged;

        _scoreManager.Reset();
    }

    private void CreateWorld()
    {
        _cameraController.Reset();
        _worldController.Reset(_cameraController);
    }

    private Player SpawnPlayer()
    {
        return Instantiate(_playerPrefab, new Vector3(0f, -_playerStartHeightOffset, 0f), Quaternion.identity);
    }

    private IEnumerator GameIntro()
    {
        _globalAudioManager.PlayAudioGameBackground();
        _uiController.SetVisibleUI(UITypes.Intro);

        int counter = _countdownStartValue;

        yield return new WaitForSeconds(_countdownUpdateInterval);

        while (counter > 0)
        {
            _uiController.UpdateCountdown(counter);
            _globalAudioManager.PlayAudioCountdown();

            yield return new WaitForSeconds(_countdownUpdateInterval);

            --counter;
        }

        _player.InitialJump();
        _cameraController.UpdateCamera(_player);

        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        _uiController.SetVisibleUI(UITypes.Game);
        _globalAudioManager.PlayAudioGameStart();

        while (_player.CheckIfRendererIsVisible(_cameraController.Camera) == true)
        {
            _worldController.UpdateWorld(_cameraController);
            _player.UpdatePlayerInput(_cameraController);
            _cameraController.UpdateCamera(_player);

            yield return null;
        }

        StartCoroutine(GameOutro());
    }

    private IEnumerator GameOutro()
    {
        _uiController.SetVisibleUI(UITypes.Outro);
        _globalAudioManager.PlayAudioGameOver();

        _player.PlatformsJumpedCountChanged -= OnPlatformJumpedCountChanged;
        Destroy(_player.gameObject);

        if (_scoreManager.CheckForHighScore(_difficultyMode))
        {
            _scoreManager.SetHighScore(_scoreManager.CurrentScore, _difficultyMode);
            _uiController.ShowNewHighScore(_scoreManager.CurrentScore);
        }
        else
        {
            _uiController.ShowEndScore(_scoreManager.CurrentScore, _scoreManager.GetHighScore(_difficultyMode));
        }

        yield return new WaitForSeconds(_gameOutroDuration);

        StartCoroutine(ShowMenu());
    }

    private void Awake()
    {
        Debug.Assert(_cameraController != null, "Missing reference!");
        Debug.Assert(_worldController != null, "Missing reference!");
        Debug.Assert(_uiController != null, "Missing reference!");
        Debug.Assert(_playerPrefab != null, "Missing reference!");

        _scoreManager = new ScoreManager();
    }

    private void OnEnable()
    {
        _uiController.StartButtonPressed += OnStartButtonPressed;
    }

    private void OnDisable()
    {
        _uiController.StartButtonPressed -= OnStartButtonPressed;
    }

    private void Start()
    {
        StartCoroutine(ShowMenu());
    }
}