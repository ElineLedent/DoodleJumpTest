using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Vector3 _movementDirection;

    [SerializeField]
    private float _movementSpeed = 3f;

    private Vector3 _startPosition;

    private float _movementRange;

    private float _previousDistance;
    private bool _isMovingAwayFromStartPosition;

    private void Update()
    {
        float currentDistance = Vector3.Distance(_startPosition, transform.position);

        if (_isMovingAwayFromStartPosition)
        {
            if (currentDistance > _movementRange)
            {
                _isMovingAwayFromStartPosition = false;
            }
        }
        else
        {
            if (currentDistance > _previousDistance)
            {
                _isMovingAwayFromStartPosition = true;
            }
        }

        int directionMultiplier = _isMovingAwayFromStartPosition ? 1 : -1;
        transform.Translate(_movementDirection.normalized * _movementSpeed * Time.deltaTime * directionMultiplier);
        _previousDistance = currentDistance;
    }

    public void Initialize(float range)
    {
        _movementRange = range;
        _startPosition = transform.position;
        _isMovingAwayFromStartPosition = true;
        _previousDistance = 0;

        float startOffset = Random.Range(0, _movementRange);
        transform.Translate(startOffset * _movementDirection);
    }
}