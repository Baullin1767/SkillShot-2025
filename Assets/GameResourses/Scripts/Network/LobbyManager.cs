using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TMP_InputField playerName;
    [SerializeField]
    TMP_InputField roomName;
    [SerializeField]
    TextMeshProUGUI statusText;
    [SerializeField]
    Button StartGame;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        StartGame.onClick.AddListener(JoinRoom);
        StartGame.interactable = false;
    }
    public override void OnConnectedToMaster()
    {
        StartGame.interactable = true;
    }
    private void JoinRoom()
    {
        if (string.IsNullOrEmpty(playerName.text))
        {
            statusText.text = "Enter nickname!";
            return;
        }

        if (string.IsNullOrEmpty(roomName.text))
        {
            statusText.text = "Enter room name!";
            return;
        }
        PhotonNetwork.NickName = playerName.text;
        PhotonNetwork.JoinOrCreateRoom(roomName.text,
            new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
        PhotonNetwork.LoadLevel(1);

    }
}
