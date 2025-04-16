using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public GameConfig[] gameConfigs;
    public GameObject gameSceneUIController;
    public GameObject sessionController;
    public override void InstallBindings()
    {
        foreach (var item in gameConfigs)
        {
            Container.Bind<GameConfig>().WithId(item.difficultyLevel).FromScriptableObject(item).AsCached();
        }
        Container.Bind<GameSceneUIController>().FromComponentOn(gameSceneUIController).AsSingle();
        Container.Bind<SessionController>().FromComponentOn(sessionController).AsSingle();
        Container.Bind<ScoreCounter>().AsSingle();
    }
}
