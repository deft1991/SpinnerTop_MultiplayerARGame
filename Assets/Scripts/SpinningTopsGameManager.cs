using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpinningTopsGameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")] public GameObject uiInformPanelGameObject;
    public TextMeshProUGUI uiInformText;
    public GameObject searchForGamesButtonObject;
    public GameObject adjustButton;
    public GameObject raycastCenterImage;
    public SpawnManager spawnManager;
    public SpawnBoosterManager spawnBoosterManager;

    private float _timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        uiInformPanelGameObject.SetActive(true);
        spawnBoosterManager.AddOnBoosterSpawnEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnBoosterManager.enabled)
        {
            _timer += Time.deltaTime;
        
            if (_timer > 15)
            {
                StartCoroutine(spawnBoosterManager.SpawnBoosterAfterSeconds(3f));
                _timer = 0;
            }
        }
    }

    #region UI Callback Methods

    public void OnClickJoinRandomRoom()
    {
        uiInformText.text = "Searching for available rooms...";

        PhotonNetwork.JoinRandomRoom();

        searchForGamesButtonObject.SetActive(false);
    }

    public void OnClickQuitMatch()
    {
        // should leave room only if we are in room
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }

        // call back to OnLeftRoom
    }

    #endregion

    #region PHOTON Callback Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        uiInformText.text = message;

        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        adjustButton.SetActive(false);
        raycastCenterImage.SetActive(false);

        // fix our time if we 1st player in the room
        // after 5 seconds - add bot
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uiInformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name
                                             + ". Waiting for other players...";
            // todo return bot spawning
            // StartCoroutine(SpawnBotAfterSeconds(15f));
        }
        else
        {
            uiInformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(uiInformPanelGameObject, 2f));
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            spawnBoosterManager.enabled = true;
            
        }

        Debug.Log(PhotonNetwork.NickName + " joined to room " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        var informMsgAboutPlayerEnteredRoom = PhotonNetwork.NickName + " joined to room "
                                                                     + PhotonNetwork.CurrentRoom.Name +
                                                                     ". Player count "
                                                                     + PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(informMsgAboutPlayerEnteredRoom);

        uiInformText.text = informMsgAboutPlayerEnteredRoom;

        StartCoroutine(DeactivateAfterSeconds(uiInformPanelGameObject, 2));
        
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            spawnBoosterManager.enabled = true;
        }
    }

    public override void OnLeftRoom()
    {
        // todo узнать что будет если не обрабатывать этот метод
        // SceneLoader.Instance.LoadScene("Scene_Lobby");
        if (!PhotonNetwork.IsConnected)
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
    }

    #endregion

    #region Private methods

    public void CreateAndJoinRoom()
    {
        string randomRoomName = "Room " + Random.Range(0, 100000);
        RoomOptions roomOptions = new RoomOptions {IsOpen = true, IsVisible = true, MaxPlayers = 2};

        // Creating Room 
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions); // creates and join to room
    }

    IEnumerator DeactivateAfterSeconds(GameObject gameObjectForDeactivate, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObjectForDeactivate.SetActive(false);
    }


    IEnumerator SpawnBotAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(DeactivateAfterSeconds(uiInformPanelGameObject, 2));
        // if we wait seconds in the room create bot
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            // set bot in the room
            spawnManager.SpawnBot();
        }
    }

    #endregion
}