using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game Config")]
public class GameConfig : ScriptableObject
{
    public DifficultyLevel difficultyLevel;
    public float roundTime;
    public int scoreToWin;
}

public enum DifficultyLevel
{
    Hard,
    Middle,
    Easy
}