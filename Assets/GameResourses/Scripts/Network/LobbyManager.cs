using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    UIDocument uIDocument;


    private TextField _playerNameField;
    private TextField _roomNameField;
    private Button _startGameButton;

    void Start()
    {
        uIDocument = FindAnyObjectByType<UIDocument>();

        PhotonNetwork.ConnectUsingSettings();
        var root = uIDocument.rootVisualElement;

        _playerNameField = root.Q<TextField>("PlayerNameField");
        _roomNameField = root.Q<TextField>("RoomNameField");
        _startGameButton = root.Q<Button>("StartGameButton");

        _startGameButton.enabledSelf = false; 
        PhotonNetwork.AutomaticallySyncScene = true;

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
