using UnityEngine;

public class ScoreManager
{
    public int CurrentScore { get; private set; } = 0;

    public void SetCurrentScore(int currentScore)
    {
        CurrentScore = currentScore;
    }

    public int GetHighScore(GameDifficultyMode difficultyMode)
    {
        return PlayerPrefs.GetInt("HighScore" + difficultyMode.ToString());
    }

    public void SetHighScore(int newHighScore, GameDifficultyMode difficultyMode)
    {
        PlayerPrefs.SetInt("HighScore" + difficultyMode.ToString(), newHighScore);
    }

    public bool CheckForHighScore(GameDifficultyMode difficultyMode)
    {
        return CurrentScore > GetHighScore(difficultyMode);
    }

    public void Reset()
    {
        CurrentScore = 0;
    }
}