using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Camera Camera { get; private set; }
    public float WorldWidth { get; set; } = 10;
    public float WorldBoundsTop { get { return Camera.ViewportToWorldPoint(new Vector3(0.5f, 1f, transform.position.z * -1)).y; } }
    public float WorldBoundsBottom { get { return Camera.ViewportToWorldPoint(new Vector3(0.5f, 0f, transform.position.z * -1)).y; } }

    private Vector3 _startPosition;
    private float _cameraPlayerOffset;

    public void UpdateCamera(Player player)
    {
        if (player != null && player.transform.position.y > transform.position.y + _cameraPlayerOffset)
        {
            float newHeight = (player.transform.position.y - _cameraPlayerOffset);
            transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
        }
    }

    public void Reset()
    {
        transform.position = _startPosition;
    }

    public void Initialize(Player player)
    {
        _cameraPlayerOffset = player.transform.position.y - transform.position.y;
    }

    private void Awake()
    {
        Camera = GetComponent<Camera>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        Camera.orthographicSize = (WorldWidth / 2f) / Camera.aspect;
    }
}