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

    // Start is called before the first frame update
    void Start()
    {
        uiInformPanelGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
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
            StartCoroutine(SpawnBotAfterSeconds(5f));
        }
        else
        {
            uiInformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(uiInformPanelGameObject, 2f));
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
    }

    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region Private methods

    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room " + Random.Range(0, 100000);
        RoomOptions roomOptions = new RoomOptions {IsOpen = true, IsVisible = true, MaxPlayers = 3};

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