using System;
using UniRx;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

public class SessionController : MonoBehaviour
{

    [Inject] private GameSceneUIController _UIController;

    [Inject(Id = DifficultyLevel.Easy)] private GameConfig _gameConfigEasy;
    [Inject(Id = DifficultyLevel.Middle)] private GameConfig _gameConfigMiddle;
    [Inject(Id = DifficultyLevel.Hard)] private GameConfig _gameConfigHard;

    private GameConfig _gameConfig;
    public GameConfig GameConfig { get => _gameConfig; }
    public bool isGameStart { set; get; }

    public Action OnGameFinish { set; get; }

    private IDisposable timerStream;

    float sessionTimer;
    private void Start()
    {
        _gameConfig = _gameConfigEasy;

        sessionTimer = _gameConfig.roundTime*60;

        timerStream = Observable.EveryUpdate()
        .Where(_ => isGameStart)
        .Subscribe(_ => {
            sessionTimer -= Time.deltaTime;
            _UIController.SessionTimer(sessionTimer);
            if(sessionTimer <= 0)
            {
                OnGameFinish?.Invoke();
                timerStream?.Dispose();
            }
        });

        OnGameFinish += _UIController.OpenTableOfRecords;
        OnGameFinish += FinishGame;
    }
    private void OnDestroy()
    {
        timerStream?.Dispose();
        OnGameFinish -= _UIController.OpenTableOfRecords;
        OnGameFinish -= FinishGame;
    }

    private void FinishGame()
    {
        isGameStart = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
