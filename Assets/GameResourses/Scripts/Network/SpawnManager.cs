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
            Vector3 spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            GameObject playerGO = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

            _container.InjectGameObject(playerGO);
        }
    }
}
