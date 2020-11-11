using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField]
    private WorldGenerator _worldGeneratorEasy = default;

    [SerializeField]
    private WorldGenerator _worldGeneratorDefault = default;

    [SerializeField]
    private WorldGenerator _worldGeneratorHard = default;

    private List<Platform> _platforms = new List<Platform>();
    private WorldGenerator _worldGenerator;

    private int _currentSegmentIndex = 0;
    private int _lastSegmentWithPlatformIndex = 0;

    public void UpdateWorld(CameraController cameraController)
    {
        RemoveOutOfRangePlatforms(_platforms, cameraController.WorldBoundsBottom);

        while (cameraController.WorldBoundsTop + 1 >= _currentSegmentIndex)
        {
            _worldGenerator.CreatePlatformsForSegment(_platforms, transform, ref _currentSegmentIndex, ref _lastSegmentWithPlatformIndex);
        }
    }

    public void Reset(CameraController cameraController)
    {
        RemoveAllWorldObjects();

        _currentSegmentIndex = 0;
        _lastSegmentWithPlatformIndex = _currentSegmentIndex;

        _worldGenerator.CreateStartingPlatforms(_platforms, transform, ref _currentSegmentIndex, ref _lastSegmentWithPlatformIndex);
        UpdateWorld(cameraController);
    }

    private WorldGenerator GetWorldGenerator(GameDifficultyMode difficultyMode)
    {
        WorldGenerator worldGenerator = null;

        switch (difficultyMode)
        {
            case GameDifficultyMode.Easy:
                worldGenerator = _worldGeneratorEasy;
                break;

            case GameDifficultyMode.Default:
                worldGenerator = _worldGeneratorDefault;
                break;

            case GameDifficultyMode.Hard:
                worldGenerator = _worldGeneratorHard;
                break;
        }

        Debug.Assert(worldGenerator != null, "Failed to get world generator");

        return worldGenerator;
    }

    private void RemoveOutOfRangePlatforms(List<Platform> spawnedPlatforms, float worldBoundsBottom)
    {
        List<Platform> platformsToBeRemoved = new List<Platform>();

        foreach (Platform platform in _platforms)
        {
            if (platform.transform.position.y + platform.MovementBounds.y < worldBoundsBottom)
            {
                platformsToBeRemoved.Add(platform);
            }
        }

        foreach (Platform platform in platformsToBeRemoved)
        {
            spawnedPlatforms.Remove(platform);
            Destroy(platform.gameObject);
        }
    }

    private void RemoveAllWorldObjects()
    {
        foreach (Platform platform in _platforms)
        {
            Destroy(platform.gameObject);
        }

        _platforms.Clear();
    }

    public void Initialize(CameraController cameraController, GameDifficultyMode difficultyMode)
    {
        _worldGenerator = GetWorldGenerator(difficultyMode);
        if (_worldGenerator != null)
        {
            cameraController.WorldWidth = _worldGenerator.WorldWidth;
        }
    }

    private void Awake()
    {
        Debug.Assert(_worldGeneratorEasy != null, "Missing reference!");
        Debug.Assert(_worldGeneratorDefault != null, "Missing reference!");
        Debug.Assert(_worldGeneratorHard != null, "Missing reference!");
    }
}