﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    public SceneField gameScene;

    public GameObject mainMenu;

    public GameObject findOrCreateGameMenu;
    public TMP_InputField playerNameInputField;

    public GameObject createGameMenu;
    public TMP_InputField createGameNameInputField;

    public GameObject gameLobbyMenu;
    public GameObject gameLobbyMenuList;
    public GameObject gameLobbyMenuItem;

    public GameObject roomLobbyMenu;
    public GameObject roomLobbyMenuList;
    public GameObject roomLobbyMenuItem;

    private Dictionary<Player, bool> isReadyInRoom;

    #region Monobehaviour Methods
    private void OnEnable()
    {
        InitializeNickname();
    }
    #endregion

    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        ConnectToMasterServerLobby();
    }

    public override void OnJoinedLobby()
    {
        ShowFindOrCreateGameMenu();
    }

    public override void OnJoinedRoom()
    {
        ShowRoomLobbyMenu();
        isReadyInRoom = new Dictionary<Player, bool>();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        isReadyInRoom.Add(newPlayer, false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        isReadyInRoom.Remove(otherPlayer);
    }
    #endregion

    #region Public Methods
    public void ConnectToGame()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToPhotonMasterServer();
        }
        else
        {
            ConnectToMasterServerLobby();
        }
    }

    public void ShowCreateGameMenu()
    {
        HideAllMenus();
        createGameMenu.SetActive(true);
    }

    public void CreateGame()
    {
        string roomName = createGameNameInputField.text;
        // Create a random game name if no string is entered. Random name is always "Game #XXXX".
        PhotonNetwork.CreateRoom(string.IsNullOrEmpty(roomName) ? "Game #" + Random.Range(1000, 9999) : roomName);
    }

    public void ShowGameLobbyMenu()
    {
        HideAllMenus();
        gameLobbyMenu.SetActive(true);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    #endregion

    #region Private Methods
    private void ConnectToPhotonMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void ConnectToMasterServerLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    private void ShowFindOrCreateGameMenu()
    {
        HideAllMenus();
        findOrCreateGameMenu.SetActive(true);
    }

    private string playerNamePrefKey = "PlayerName";

    private void InitializeNickname()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            string storedName = PlayerPrefs.GetString(playerNamePrefKey);
            playerNameInputField.text = storedName;
            PhotonNetwork.NickName = storedName;
        }

        playerNameInputField.onValueChanged.AddListener(UpdatePlayerNickname);
    }

    private void UpdatePlayerNickname(string nickname)
    {
        PlayerPrefs.SetString(playerNamePrefKey, nickname);
        PhotonNetwork.NickName = nickname;
    }

    private void ShowRoomLobbyMenu()
    {
        HideAllMenus();
        roomLobbyMenu.SetActive(true);
    }

    // Used to avoid keeping track of what menu is open before opening another.
    // This is more robust, too.
    private void HideAllMenus()
    {
        mainMenu.SetActive(false);
        findOrCreateGameMenu.SetActive(false);
        createGameMenu.SetActive(false);
        gameLobbyMenu.SetActive(false);
        roomLobbyMenu.SetActive(false);
    }

    private void CheckReadyStatus()
    {
        // Check if all players are ready, return if not.
        foreach(bool readyStatus in isReadyInRoom.Values)
        {
            if(readyStatus == false)
            {
                return;
            }
        }

        // All Players Ready - Start Game
        LaunchGame();
    }

    private void LaunchGame()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        PhotonNetwork.LoadLevel(gameScene.SceneName);
    }
    #endregion

    #region RPCs
    [PunRPC]
    public void SetReadyStatus(bool isReady, PhotonMessageInfo info)
    {
        isReadyInRoom[info.Sender] = isReady;
        if(PhotonNetwork.IsMasterClient)
        {
            CheckReadyStatus();
        }
    }
    #endregion
}
