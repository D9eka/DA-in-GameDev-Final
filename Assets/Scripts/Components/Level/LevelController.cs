using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelDifficulty[] _difficulties;

    private int _currentDifficultyIndex;

    public EventHandler<LevelDifficulty> OnChangeDifficulty;

    public static LevelController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BirdController.Instance.OnIncreaseScore += BirdComponent_OnIncreaseScore;
    }

    private void BirdComponent_OnIncreaseScore(object sender, int e)
    {
        if (_currentDifficultyIndex != _difficulties.Length - 1 &&
           _difficulties[_currentDifficultyIndex + 1].ScoreToActivate == e)
            ChangeDifficulty(_currentDifficultyIndex + 1);
    }

    private void ChangeDifficulty(int difficultyIndex)
    {
        OnChangeDifficulty?.Invoke(this, _difficulties[difficultyIndex]);
        _currentDifficultyIndex = difficultyIndex;
    }
}

[Serializable]
public struct LevelDifficulty
{
    public int ScoreToActivate;
    public float Speed;

    public float PipeSpawnDelay;
    public float SpaceBetweenPipes;
}