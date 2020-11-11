using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;

    [SerializeField]
    private float _springStartVelocityModifier = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null && player.IsJumpingUp == false)
        {
            Platform platform = GetComponentInParent<Platform>();

            if (platform != null)
            {
                platform.SetPlatformHasBeenJumpedOn(player);
            }

            player.StartNewJump(_springStartVelocityModifier);

            _audioSource.Play();
        }
    }

    private void Awake()
    {
        Debug.Assert(_audioSource != null, "Missing reference");
        Debug.Assert(_audioSource.clip != null, "Missing reference");
        _audioSource.playOnAwake = false;
    }
}