using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

public class GameSceneUIController : MonoBehaviour
{
    [Inject] SessionController sessionController;
    [SerializeField]
    UIDocument timerWindow;
    [SerializeField]
    UIDocument HUD;
    [SerializeField]
    UIDocument tableRecords;

    private TextElement _timer;
    private TextElement _sessionTimer;
    private TextElement _score;
    private ScrollView _playerList;
    public async UniTask TimerAnimation()
    {
        var root = timerWindow.rootVisualElement;
        _timer = root.Q<TextElement>("Timer");
        for (int i = 3; i >= 0; i--)
        {
            _timer.text = i==0 ? "GO" : i.ToString();
            await UIAnimations.ScaleUp(_timer);
            UIAnimations.HideElement(_timer);
        }
        timerWindow.gameObject.SetActive(false);
        HUD.gameObject.SetActive(true);
        Time.timeScale = 1;
        sessionController.isGameStart = true;
    }

    public void ScoreAnim(int score)
    {
        var root = HUD.rootVisualElement;
        _score = root.Q<TextElement>("Score");

        UIAnimations.Pulse(_score);
        _score.text = score.ToString();
    }
    public void SessionTimer(float totalSeconds)
    {
        var root = HUD.rootVisualElement;
        _sessionTimer = root.Q<TextElement>("SessionTimer");

        int minutes = (int)(totalSeconds / 60);
        int seconds = (int)(totalSeconds % 60);

        _sessionTimer.style.color = Color.white;
        if (seconds <= 10)
        {
            _sessionTimer.style.color = Color.red;
            UIAnimations.Pulse(_sessionTimer);
        }
        _sessionTimer.text = $"{minutes: 00}:{seconds: 00}";
    }

    public void OpenTableOfRecords()
    {
        timerWindow.gameObject.SetActive(false);
        HUD.gameObject.SetActive(false);
        tableRecords.gameObject.SetActive(true);
        UpdateTableRecords();
    }

    private void UpdateTableRecords()
    {
        var root = tableRecords.rootVisualElement;

        _playerList = root.Q<ScrollView>("PlayerList");
        _playerList.Clear();
        for (int i = 0; i < 4; i++)
        {
            try
            {
                var player = PhotonNetwork.PlayerList[i];

                var row = new VisualElement();
                row.AddToClassList("player-row");

                var nameLabel = new Label(player.NickName);
                nameLabel.AddToClassList("player-name");
                row.Add(nameLabel);

                int score = ScoreCounter.GetScore(player);
                var scoreLabel = new Label(score.ToString());
                nameLabel.AddToClassList("player-name");
                row.Add(scoreLabel);

                _playerList.Add(row);
            }
            catch (IndexOutOfRangeException)
            {
                var row = new VisualElement();
                row.AddToClassList("player-row");

                var nameLabel = new Label("Empty");
                nameLabel.AddToClassList("player-name");
                row.Add(nameLabel);

                _playerList.Add(row);
            }
        }
    }
}
