using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlatformPiece : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _renderer = default;

    private IEnumerator DestroyPlatformPiece()
    {
        // Wait one frame to make sure platform piece is rendered once by camera
        yield return null;

        while (_renderer.isVisible == true)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        Debug.Assert(_renderer != null, "Missing reference!");
    }

    private void Start()
    {
        StartCoroutine(DestroyPlatformPiece());
    }
}