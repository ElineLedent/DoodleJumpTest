using UnityEngine;

[CreateAssetMenu(fileName = "PlatformSpawner", menuName = "Scriptable Objects/Platform Spawner")]
public class PlatformSpawner : ScriptableObject
{
    [SerializeField]
    private Platform _staticPlatformPrefab = default;

    [SerializeField]
    private Platform _movingPlatformHorizontalPrefab = default;

    [SerializeField]
    private Platform _movingPlatformVerticalPrefab = default;

    public Platform CreatePlatformOfType(PlatformType type, Vector3 position, Transform parent)
    {
        Platform newPlatform = null;

        switch (type)
        {
            case PlatformType.Static:
                newPlatform = Instantiate(_staticPlatformPrefab, position, Quaternion.identity, parent);
                break;

            case PlatformType.MovingHorizontal:
                newPlatform = Instantiate(_movingPlatformHorizontalPrefab, position, Quaternion.identity, parent);
                break;

            case PlatformType.MovingVertical:
                newPlatform = Instantiate(_movingPlatformVerticalPrefab, position, Quaternion.identity, parent);
                break;
        }

        Debug.Assert(newPlatform != null, "Failed to create platform of type: " + type.ToString());

        return newPlatform;
    } 
}