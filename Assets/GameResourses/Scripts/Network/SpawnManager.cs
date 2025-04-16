using Photon.Pun;
using UnityEngine;
using Zenject;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPoints;

    [Inject] private DiContainer _container;

    void Awake()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber % spawnPoints.Length;
            Vector3 spawnPos = spawnPoints[playerIndex].transform.position;
            GameObject playerGO = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

            _container.InjectGameObject(playerGO);
        }
    }
}
