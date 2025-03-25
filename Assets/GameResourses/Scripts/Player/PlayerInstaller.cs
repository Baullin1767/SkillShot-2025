using Photon.Pun;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var photonView = GetComponent<PhotonView>();
#if UNITY_STANDALONE || UNITY_EDITOR
        if (photonView.IsMine)
            Container.Bind<IPlayerInput>().To<PlayerInputPC>().AsSingle();
#elif UNITY_ANDROID || UNITY_IOS
        if (photonView.IsMine)
        Container.Bind<IPlayerInput>().To<MobilePlayerInput>().AsSingle();
#else
        if (photonView.IsMine)
            Container.Bind<IPlayerInput>().To<PlayerInputPC>().AsSingle();
#endif
    }
}
