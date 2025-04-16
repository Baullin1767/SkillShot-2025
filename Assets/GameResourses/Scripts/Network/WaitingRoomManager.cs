using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Sprite readySprite;
    [SerializeField]
    private Sprite notReadySprite;
    private const string READY_PROP = "IsReady";
    private Button _actionButton;
    private ScrollView _playerList;

    void Start()
    {
        var root = FindAnyObjectByType<UIDocument>().rootVisualElement;
        _actionButton = root.Q<Button>("ButtonStart");
        _playerList = root.Q<ScrollView>("PlayerList");

        _actionButton.clicked += OnActionButtonClick;

        RefreshUI();
    }

    private void OnActionButtonClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (AllReadyOrAlone())
                PhotonNetwork.LoadLevel(2);
        }
        else
        {
            bool current = GetReady(PhotonNetwork.LocalPlayer);
            SetReady(!current);
        }
    }

    private void SetReady(bool value)
    {
        var props = new Hashtable { { READY_PROP, value } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    private bool GetReady(Player p)
    {
        return p.CustomProperties.TryGetValue(READY_PROP, out var val) && (bool)val;
    }

    private bool AllReadyOrAlone()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) return true;

        foreach (var p in PhotonNetwork.PlayerListOthers)
        {
            if (!GetReady(p)) return false;
        }
        return true;
    }

    private void RefreshUI()
    {
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

                if (i > 0)
                {
                    var icon = new Image();
                    icon.AddToClassList("ready-icon");
                    icon.sprite = GetReady(player) ? readySprite : notReadySprite;
                    row.Add(icon); 
                }

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

        if (PhotonNetwork.IsMasterClient)
        {
            _actionButton.text = AllReadyOrAlone() ? "Start" : "Wait...";
            _actionButton.SetEnabled(AllReadyOrAlone());
        }
        else
        {
            bool isReady = GetReady(PhotonNetwork.LocalPlayer);
            _actionButton.text = isReady ? "Not reary" : "Reary";
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(READY_PROP))
            RefreshUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RefreshUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RefreshUI();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        RefreshUI();
    }
}
