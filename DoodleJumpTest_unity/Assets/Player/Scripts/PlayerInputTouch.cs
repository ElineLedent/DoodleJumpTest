using UnityEngine;

public class PlayerInputTouch : MonoBehaviour, IPlayerInput
{
    private Vector3 _previousTouchPosition;

    public float ProcessInput(CameraController cameraController)
    {
        float touchDelta = 0;

        foreach (Touch touch in Input.touches)
        {
            Vector3 touchPosition = cameraController.Camera.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                _previousTouchPosition = touchPosition;
            }

            touchDelta = touchPosition.x - _previousTouchPosition.x;
            _previousTouchPosition = touchPosition;
        }

        return touchDelta;
    }

    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }
}