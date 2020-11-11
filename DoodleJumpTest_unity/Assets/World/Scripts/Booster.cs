using System.Collections;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer = default;

    [SerializeField]
    private Collider _collider = default;

    [SerializeField]
    private Rigidbody _rigidBody = default;

    [SerializeField]
    private AudioSource _audioSource = default;

    [SerializeField]
    private float _boosterStartVelocityModifier = 1.5f;

    [SerializeField]
    private float _boosterAccelerationDelay = 1f;

    [SerializeField]
    private Vector3 _detachForce = new Vector3(0f, 5f, -5f);

    public void Detach()
    {
        _rigidBody.useGravity = true;
        _rigidBody.isKinematic = false;

        Vector3 localForce = transform.TransformDirection(_detachForce);
        _rigidBody.AddForce(localForce, ForceMode.Impulse);

        _audioSource.Stop();

        StartCoroutine(DestroyBooster());
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            _collider.enabled = false;
            player.AttachBooster(this, _boosterStartVelocityModifier, _boosterAccelerationDelay);
            _audioSource.Play();
        }
    }

    private IEnumerator DestroyBooster()
    {
        while (_renderer.isVisible == true)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        Debug.Assert(_renderer != null, "Missing reference");
        Debug.Assert(_collider != null, "Missing reference");

        Debug.Assert(_rigidBody != null, "Missing reference");
        _rigidBody.useGravity = false;
        _rigidBody.isKinematic = true;

        Debug.Assert(_audioSource != null, "Missing reference");
        Debug.Assert(_audioSource.clip != null, "Missing reference");
    }
}