using UnityEngine;

public class PlayerInputKeyboard : MonoBehaviour, IPlayerInput
{
    [SerializeField]
    private float _sidewaysVelocity = 20f;

    public float ProcessInput(CameraController cameraController)
    {
        return Input.GetAxis("Horizontal") * _sidewaysVelocity * Time.deltaTime;
    }
}