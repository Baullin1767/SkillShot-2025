using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using WebSocketSharp;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    UIDocument uIDocument;


    private TextField _playerNameField;
    private TextField _roomNameField;
    private UnityEngine.UIElements.Button _startGameButton;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        var root = uIDocument.rootVisualElement;

        _playerNameField = root.Q<TextField>("PlayerNameField");
        _roomNameField = root.Q<TextField>("RoomNameField");
        _startGameButton = root.Q<UnityEngine.UIElements.Button>("StartGameButton");

        _startGameButton.enabledSelf = false;

        _startGameButton.clicked += JoinRoom;
    }

    public override void OnConnectedToMaster()
    {
        _startGameButton.enabledSelf = true;
        Debug.Log("Connected To Master");
    }
    private void JoinRoom()
    {
        if (string.IsNullOrEmpty(_playerNameField.text))
        {
            return;
        }

        if (string.IsNullOrEmpty(_roomNameField.text))
        {
            return;
        }
        PhotonNetwork.NickName = _playerNameField.text;
        PhotonNetwork.JoinOrCreateRoom(_roomNameField.text,
            new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
