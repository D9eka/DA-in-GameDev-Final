using System;
using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelDifficulty[] _difficulties;
    [SerializeField] private bool _forceStart;

    private int _currentDifficultyIndex;

    public EventHandler<LevelDifficulty> OnChangeDifficulty;

    public float LeftEdge { get; private set; }
    public float RightEdge { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }

    public bool ForceStart => _forceStart;

    public static LevelController Instance { get; private set; }

    public LevelDifficulty GetInitialValues()
    {
        return _difficulties[0];
    }    

    private void Awake()
    {
        Instance = this;

        Vector2 cameraBottomLeftAngle = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector2 cameraTopRightAngle = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        LeftEdge = cameraBottomLeftAngle.x;
        RightEdge = cameraTopRightAngle.x;
        Width = RightEdge - LeftEdge;
        Height = cameraTopRightAngle.y - cameraBottomLeftAngle.y;
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