using UnityEngine;
using Zenject;

public class GameSturtup : MonoBehaviour
{
    [Inject] GameSceneUIController UIController;
    async void Start()
    {
        Time.timeScale = 0;
        await UIController.TimerAnimation();
    }
}
