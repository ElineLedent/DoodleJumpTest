using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInputTouch))]
[RequireComponent(typeof(PlayerInputKeyboard))]
public class Player : MonoBehaviour
{
    public Action<int> PlatformsJumpedCountChanged;

    public bool IsJumpingUp { get; private set; } = true;

    [SerializeField]
    private MeshRenderer _renderer = default;

    [SerializeField]
    private GameObject _viewOffset = default;

    [SerializeField]
    private GameObject _boosterSlot = default;

    [SerializeField]
    private AudioSource _audioSource = default;

    [SerializeField]
    private float _jumpStartVelocity = 30f;

    [SerializeField]
    private float _gravityAcceleration = 0.025f;

    [SerializeField]
    private float _playerRotation = 45f;

    private int _jumpCount = 0;

    private Coroutine _jumpingRoutine;
    private Booster _lastAttachedBooster;

    private IPlayerInput[] _playerInputs;

    public bool CheckIfRendererIsVisible(Camera camera)
    {
        return _renderer.IsVisibleFrom(camera);
    }

    public void UpdatePlayerInput(CameraController cameraController)
    {
        float movementDelta = 0;

        foreach (IPlayerInput input in _playerInputs)
        {
            movementDelta += input.ProcessInput(cameraController);
        }

        if (movementDelta > 0)
        {
            _viewOffset.transform.localRotation = Quaternion.Euler(0f, -_playerRotation, 0f);
        }
        else if (movementDelta < 0)
        {
            _viewOffset.transform.localRotation = Quaternion.Euler(0f, _playerRotation, 0f);
        }

        transform.Translate(movementDelta, 0f, 0f);
    }

    public void InitialJump()
    {
        float initialVelocityModifier = 0f;
        float initialAccelerationDelay = 0f;

        _jumpingRoutine = StartCoroutine(Jump(initialVelocityModifier, initialAccelerationDelay));
    }

    public void StartNewJump(float startVelocityModifier = 1f, float accelerationDelay = 0f)
    {
        if (_jumpingRoutine != null)
        {
            StopCoroutine(_jumpingRoutine);
            _jumpingRoutine = null;
        }

        _jumpingRoutine = StartCoroutine(Jump(startVelocityModifier, accelerationDelay));

        _audioSource.Play();
    }

    public void IncrementJumpCount()
    {
        ++_jumpCount;

        PlatformsJumpedCountChanged?.Invoke(_jumpCount);
    }

    public void AttachBooster(Booster booster, float startVelocityModifier, float accelerationDelay)
    {
        _lastAttachedBooster = booster;

        booster.transform.SetParent(_boosterSlot.transform, false);
        booster.transform.localPosition = Vector3.zero;

        StartNewJump(startVelocityModifier, accelerationDelay);

        StartCoroutine(DetachBooster(booster));
    }

    private IEnumerator DetachBooster(Booster booster)
    {
        while (IsJumpingUp == true && _lastAttachedBooster == booster)
        {
            yield return null;
        }

        booster.transform.SetParent(null);

        booster.Detach();
    }

    private IEnumerator Jump(float startVelocityModifier, float accelerationDelay)
    {
        IsJumpingUp = true;

        float jumpVelocity = _jumpStartVelocity * startVelocityModifier;

        while (true)
        {
            if (accelerationDelay <= 0)
            {
                jumpVelocity -= _gravityAcceleration * Time.deltaTime;
            }
            else
            {
                accelerationDelay -= Time.deltaTime;
            }

            transform.Translate(0f, jumpVelocity * Time.deltaTime, 0f);

            if (jumpVelocity < 0)
            {
                IsJumpingUp = false;
            }

            yield return null;
        }
    }

    private void Awake()
    {
        Debug.Assert(_renderer != null, "Missing reference");
        Debug.Assert(_viewOffset != null, "Missing reference");
        Debug.Assert(_boosterSlot != null, "Missing reference");
        Debug.Assert(_audioSource != null, "Missing reference");
        Debug.Assert(_audioSource.clip != null, "Missing reference");

        _playerInputs = GetComponents<IPlayerInput>();
    }
}