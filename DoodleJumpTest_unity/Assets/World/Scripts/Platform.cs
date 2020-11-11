using UnityEngine;

public enum PlatformType { Static, MovingHorizontal, MovingVertical };

public class Platform : MonoBehaviour
{
    public Vector3 MovementBounds { get; private set; } = Vector3.zero;

    public bool IsDestructible { get; private set; } = false;

    [SerializeField]
    private Renderer _indestructiblePlatformView = default;

    [SerializeField]
    private Renderer _destructiblePlatformView = default;

    [SerializeField]
    private PlatformPiece _platformPiecePrefab = default;

    [SerializeField]
    private AudioSource _audioSource = default;

    [SerializeField]
    private Vector3 _platformPieceOffset = new Vector3(0.1f, 0f, 0f);

    [SerializeField]
    private float _platformPieceAngle = 20f;

    private Renderer _activeRenderer;

    private bool _platformHasBeenJumpedOn = false;

    public void SetIsDestructible(bool isDestructible)
    {
        IsDestructible = isDestructible;

        if (IsDestructible)
        {
            _indestructiblePlatformView.gameObject.SetActive(false);
            _destructiblePlatformView.gameObject.SetActive(true);
            _activeRenderer = _destructiblePlatformView;
        }
        else
        {
            _destructiblePlatformView.gameObject.SetActive(false);
            _indestructiblePlatformView.gameObject.SetActive(true);
            _activeRenderer = _indestructiblePlatformView;
        }
    }

    public void SetPlatformHasBeenJumpedOn(Player player)
    {
        if (_platformHasBeenJumpedOn == false)
        {
            player.IncrementJumpCount();
            _platformHasBeenJumpedOn = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (_activeRenderer.isVisible == true)
        {
            if (player != null && player.IsJumpingUp == false)
            {
                player.StartNewJump();

                SetPlatformHasBeenJumpedOn(player);

                if (IsDestructible)
                {
                    DestroyPlatform();
                }
            }
        }
    }

    private void DestroyPlatform()
    {
        _destructiblePlatformView.gameObject.SetActive(false);

        _audioSource.Play();

        Instantiate(_platformPiecePrefab.gameObject, transform.position - _platformPieceOffset, Quaternion.Euler(0f, 0f, -_platformPieceAngle));
        Instantiate(_platformPiecePrefab.gameObject, transform.position + _platformPieceOffset, Quaternion.Euler(0f, 0f, _platformPieceAngle));
    }

    private void Awake()
    {
        Debug.Assert(_indestructiblePlatformView != null, "Missing reference!");
        Debug.Assert(_destructiblePlatformView != null, "Missing reference!");
        Debug.Assert(_platformPiecePrefab != null, "Missing reference!");

        Debug.Assert(_audioSource != null, "Missing reference!");
        Debug.Assert(_audioSource.clip != null, "Missing reference!");
    }
}