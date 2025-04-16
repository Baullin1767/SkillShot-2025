using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

public class ScoreCounter
{

    [Inject] SessionController sessionController;
    private readonly GameSceneUIController _uiController;
    private int _score = 0;

    [Inject]
    public ScoreCounter(GameSceneUIController uiController)
    {
        _uiController = uiController;
    }

    public void AddPoints(int points)
    {
        _score += points;
        _uiController.ScoreAnim(_score);
        SetScore(_score);
        if(_score >= sessionController.GameConfig.scoreToWin)
        {
            sessionController.OnGameFinish?.Invoke();
        }
    }

    public static void SetScore(int value)
    {
        Hashtable props = new Hashtable
        {
            { "Score", value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public static int GetScore(Player p)
    {
        return p.CustomProperties.TryGetValue("Score", out var val) ? (int)val : 0;
    }

}
