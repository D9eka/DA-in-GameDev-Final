using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartButton;
    [Space]
    [SerializeField] private GameObject _scoreHandler;
    [SerializeField] private TextMeshProUGUI _scoreNum;

    [SerializeField] private GameObject _maxScoreHandler;
    [SerializeField] private TextMeshProUGUI _maxScoreNum;

    private const string SCORE_KEY = "Score";

    public EventHandler OnGameStart;

    public static UI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _startButton.gameObject.SetActive(true);
        _startButton.onClick.AddListener(() => OnStartButtonClick());
        _restartButton.gameObject.SetActive(false);

        _scoreHandler.SetActive(false);
        _scoreNum.text = 0.ToString();

        _maxScoreHandler.SetActive(true);
        _maxScoreNum.text = GetMaxScore().ToString();
    }

    private int GetMaxScore()
    {
        return PlayerPrefs.HasKey(SCORE_KEY) ? PlayerPrefs.GetInt(SCORE_KEY) : 0;
    }

    private void Start()
    {
        BirdController.Instance.OnIncreaseScore += BirdComponent_OnIncreaseScore;
        BirdController.Instance.OnDied += BirdComponent_OnDied;
        if(BirdController.Instance.TryGetComponent(out BirdAgent agent))
        {
            agent.OnEpisodeBeginEvent += BirdAgent_OnEpisodeBeginEvent;
        }

        if(LevelController.Instance.ForceStart)
        {
            _startButton.gameObject.SetActive(false);
            _scoreHandler.SetActive(true);
        }
    }

    private void BirdAgent_OnEpisodeBeginEvent(object sender, EventArgs e)
    {
        OnStartButtonClick();
    }

    private void OnStartButtonClick()
    {
        OnGameStart?.Invoke(this, EventArgs.Empty);

        _startButton.gameObject.SetActive(false);
        _scoreHandler.SetActive(true);
    }

    private void BirdComponent_OnIncreaseScore(object sender, int e)
    {
        _scoreNum.text = e.ToString();

        if(GetMaxScore() < e)
        {
            PlayerPrefs.SetInt(SCORE_KEY, e);
            _maxScoreNum.text = e.ToString();
        }
    }

    private void BirdComponent_OnDied(object sender, EventArgs e)
    {
        _restartButton.gameObject.SetActive(true);
    }
}
