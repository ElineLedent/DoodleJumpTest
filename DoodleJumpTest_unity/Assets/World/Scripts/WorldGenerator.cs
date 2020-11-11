using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenerator", menuName = "Scriptable Objects/World Generator")]
public class WorldGenerator : ScriptableObject
{
    [SerializeField]
    private float _worldWidth = 11;

    [Header("Segments")]
    [SerializeField]
    private int _maxSegmentsWithoutPlatform = 4;

    [Header("Platforms")]
    [SerializeField]
    private PlatformSpawner _platformSpawner = default;

    [SerializeField]
    private float _platformWidth = 2f;

    [SerializeField]
    private float _horizontalPlatformSpacing = 0.2f;

    [SerializeField]
    private float _platformMaxHeightOffset = 0.2f;

    [SerializeField]
    private int _verticalMovementRange = 4;

    [Range(0f, 1f)]
    [SerializeField]
    private float[] _platformSpawnProbabilities = new float[] { 0.3f, 0.4f, 0.1f, 0.1f };

    [Range(0f, 1f)]
    [SerializeField]
    private float _staticPlatformProbability = 0.9f;

    [Range(0f, 1f)]
    [SerializeField]
    private float _horizontalMovingPlatformProbability = 0.05f;

    [Range(0f, 1f)]
    [SerializeField]
    private float _verticalMovingPlatformProbability = 0.05f;

    [Range(0f, 1f)]
    [SerializeField]
    private float _isDestructibleProbability = 0.1f;

    [Header("Items")]
    [SerializeField]
    private ItemSpawner _itemSpawner = default;

    [Range(0f, 1f)]
    [SerializeField]
    private float _itemProbability = 0.1f;

    [Range(0f, 1f)]
    [SerializeField]
    private float _boosterProbability = 0.15f;

    [Range(0f, 1f)]
    [SerializeField]
    private float _springProbability = 0.85f;

    private Vector3 _itemOffset = new Vector3(0.5f, 0f, 0f);

    private float MinWorldBoundsX { get { return -(_worldWidth / 2f); } }
    private float MaxWorldBoundsX { get { return _worldWidth / 2f; } }
    public float WorldWidth { get { return _worldWidth; } }

    public void CreateStartingPlatforms(List<Platform> platforms, Transform platformParent, ref int currentSegmentIndex, ref int lastSegmentIndexWithPlatform)
    {
        const int startingSegmentPlatformCount = 5;
        const int startingOffset = 4;

        int startingSegmentHeight = -startingOffset;

        float spaceNeededForPlatform = _platformWidth + _horizontalPlatformSpacing;
        float placementStartX = -spaceNeededForPlatform * (startingSegmentPlatformCount - 1) / 2;

        for (int i = 0; i < startingSegmentPlatformCount; ++i)
        {
            float platformPositionX = placementStartX + i * spaceNeededForPlatform;
            float platformPositionY = CalculateRandomPlatformPositionY(startingSegmentHeight);

            Platform newPlatform = _platformSpawner.CreatePlatformOfType(PlatformType.Static, new Vector3(platformPositionX, platformPositionY, 0f), platformParent);
            newPlatform.SetIsDestructible(false);
            platforms.Add(newPlatform);
        }

        lastSegmentIndexWithPlatform = startingSegmentHeight;
        currentSegmentIndex = startingSegmentHeight + _maxSegmentsWithoutPlatform + 1;
    }

    public void CreatePlatformsForSegment(List<Platform> platforms, Transform platformParent, ref int currentSegmentIndex, ref int lastSegmentIndexWithPlatform)
    {
        int filledSegmentsCount = 1;

        PlatformType segmentPlatformType = GenerateRandomPlatformType();
        int segmentPlatformCount = GenerateRandomPlatformCountForSegment(segmentPlatformType, currentSegmentIndex, lastSegmentIndexWithPlatform);

        if (segmentPlatformCount > 0)
        {
            float platformMargin = _horizontalPlatformSpacing + _platformWidth / 2;

            if (segmentPlatformType == PlatformType.MovingHorizontal)
            {
                filledSegmentsCount = CreateHorizontalMovingPlatform(platforms, platformParent, currentSegmentIndex, platformMargin);
            }
            else
            {
                filledSegmentsCount = CreatePlatforms(platforms, segmentPlatformCount, segmentPlatformType, platformParent, currentSegmentIndex, platformMargin);
            }

            lastSegmentIndexWithPlatform = currentSegmentIndex;
        }

        currentSegmentIndex += filledSegmentsCount;
    }

    private int CreatePlatforms(List<Platform> platforms, int platformCount, PlatformType platformType, Transform platformParent, float segmentHeight, float platformMargin)
    {
        bool isItemCreated = false;
        float minPlatformPositionX = MinWorldBoundsX + platformMargin;

        for (int i = 0; i < platformCount; ++i)
        {
            float platformPositionX = CalculateRandomPlatformPositionX(minPlatformPositionX, MaxWorldBoundsX, platformCount - i, _platformWidth, _horizontalPlatformSpacing);
            float platformPositionY = CalculateRandomPlatformPositionY(segmentHeight);

            Vector3 platformPosition = new Vector3(platformPositionX, platformPositionY, 0);

            Platform newPlatform = _platformSpawner.CreatePlatformOfType(platformType, platformPosition, platformParent);
            if (newPlatform != null)
            {
                TryMakePlatformDestructible(newPlatform);
                isItemCreated |= TryCreateItemForPlatform(newPlatform);

                if (platformType == PlatformType.MovingVertical)
                {
                    MovingPlatform movingPlatform = newPlatform.GetComponent<MovingPlatform>();
                    if (movingPlatform != null)
                    {
                        movingPlatform.Initialize(_verticalMovementRange);
                    }
                }

                minPlatformPositionX = newPlatform.transform.position.x + (_horizontalPlatformSpacing + _platformWidth);

                platforms.Add(newPlatform);
            }
        }

        int filledSegmentsCount = 1;

        if (platformCount > 0 && platformType == PlatformType.MovingVertical)
        {
            filledSegmentsCount += _verticalMovementRange;
        }

        if (isItemCreated)
        {
            ++filledSegmentsCount;
        }

        return filledSegmentsCount;
    }

    private int CreateHorizontalMovingPlatform(List<Platform> platforms, Transform platformParent, float segmentHeight, float platformMargin)
    {
        int filledSegmentsCount = 1;

        float platformPositionX = MinWorldBoundsX + platformMargin;
        Vector3 platformPosition = new Vector3(platformPositionX, segmentHeight, 0f);

        Platform newPlatform = _platformSpawner.CreatePlatformOfType(PlatformType.MovingHorizontal, platformPosition, platformParent);
        if (newPlatform != null)
        {
            TryMakePlatformDestructible(newPlatform);

            if (TryCreateItemForPlatform(newPlatform))
            {
                ++filledSegmentsCount;
            }

            MovingPlatform movingPlatform = newPlatform.GetComponent<MovingPlatform>();
            if (movingPlatform != null)
            {
                float horizontalMovementRange = (MaxWorldBoundsX - platformMargin) - (MinWorldBoundsX + platformMargin);
                movingPlatform.Initialize(horizontalMovementRange);
            }

            platforms.Add(newPlatform);
        }

        return filledSegmentsCount;
    }

    private bool TryMakePlatformDestructible(Platform platform)
    {
        bool isDestructible = Random.value < _isDestructibleProbability;
        platform.SetIsDestructible(isDestructible);

        return isDestructible;
    }

    private bool TryCreateItemForPlatform(Platform platform)
    {
        bool isItemCreated = false;

        if (platform.IsDestructible == false)
        {
            float randomValue = Random.value;

            if (randomValue < _itemProbability)
            {
                ItemType itemType = GenerateRandomItemType();

                GameObject item = _itemSpawner.SpawnItem(itemType, platform.transform);
                item.transform.position = CalculateItemPosition(platform.transform.position, _itemOffset);

                isItemCreated = true;
            }
        }

        return isItemCreated;
    }

    private static Vector3 CalculateItemPosition(Vector3 platformPosition, Vector3 itemOffset)
    {
        float randomValue = Random.value;

        int offsetMultiplier = randomValue < 0.5f ? 1 : -1;

        return platformPosition + itemOffset * offsetMultiplier;
    }

    private int GenerateRandomPlatformCountForSegment(PlatformType platformType, int currentSegmentIndex, int lastSegmentWithPlatformIndex)
    {
        const int maxHorizontalMovingPlatformsPerSegment = 1;
        int maxPlatformCount = platformType == PlatformType.MovingHorizontal ? maxHorizontalMovingPlatformsPerSegment : _platformSpawnProbabilities.Length;

        int platformCount = (currentSegmentIndex - lastSegmentWithPlatformIndex) > _maxSegmentsWithoutPlatform ? 1 : 0;

        for (int i = platformCount; i < maxPlatformCount; ++i)
        {
            float randomValue = Random.value;

            if (randomValue < _platformSpawnProbabilities[i])
            {
                ++platformCount;
            }
            else
            {
                break;
            }
        }

        return platformCount;
    }

    private float CalculateRandomPlatformPositionX(float minPositionX, float maxPositionX, int remainingPlatformCount, float platformWidth, float platformSpacing)
    {
        float platformMaxPositionX = maxPositionX - (platformWidth * (remainingPlatformCount - 1) + platformSpacing * remainingPlatformCount + platformWidth / 2);

        return Random.Range(minPositionX, platformMaxPositionX);
    }

    private float CalculateRandomPlatformPositionY(float segmentHeight)
    {
        float randomHeightOffset = Random.Range(-_platformMaxHeightOffset, _platformMaxHeightOffset);
        return segmentHeight + randomHeightOffset;
    }

    private PlatformType GenerateRandomPlatformType()
    {
        float totalProbability = _staticPlatformProbability + _horizontalMovingPlatformProbability + _verticalMovingPlatformProbability;
        float randomValue = Random.value * totalProbability;

        if (randomValue < _horizontalMovingPlatformProbability)
        {
            return PlatformType.MovingHorizontal;
        }
        else if (randomValue < _horizontalMovingPlatformProbability + _verticalMovingPlatformProbability)
        {
            return PlatformType.MovingVertical;
        }

        return PlatformType.Static;
    }

    private ItemType GenerateRandomItemType()
    {
        float totalProbability = _springProbability + _boosterProbability;
        float randomValue = Random.value * totalProbability;

        if (randomValue < _boosterProbability)
        {
            return ItemType.Booster;
        }

        return ItemType.Spring;
    }

    private void Awake()
    {
        Debug.Assert(_platformSpawner != null, "Missing reference!");
        Debug.Assert(_itemSpawner != null, "Missing reference!");
    }
}