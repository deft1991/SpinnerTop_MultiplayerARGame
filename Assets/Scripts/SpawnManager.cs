using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPositions;
    public GameObject battleArena;

    private enum RaiseEvenCodes
    {
        PlayerSpawnEventCode = 0
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region PHOTON Callback Methods

    /**
     * Called only on remote clients to spawn our player in their own battle arena.
     * It will not be called in our local game since we are  who raise the event
     */
    public void OnEvent(EventData photonEvent)
    {
        if ((byte) RaiseEvenCodes.PlayerSpawnEventCode == photonEvent.Code)
        {
            object[] data = (object[]) photonEvent.CustomData; // get data from PhotonNetwork.RaiseEvent
            Vector3 receivedPosition = (Vector3) data[0];
            Quaternion receiveRotation = (Quaternion) data[1];
            int receivedPlayerSelectionData = (int) data[3];

            GameObject player = Instantiate(
                playerPrefabs[receivedPlayerSelectionData]
                , battleArena.transform.position +
                  receivedPosition // because we should place remote object on our own battle arena
                , receiveRotation);

            PhotonView photonView = player.GetComponent<PhotonView>();
            photonView.ViewID = (int) data[2];
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    #endregion

    #region PRIVATE Methods

    /**
     * See https://doc.photonengine.com/en-us/pun/current/gameplay/instantiation
     * for better understanding photon sync
     */
    private void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(
            MultiplayerArSpinnerTopGame.PLAYER_SELECTION_NUMBER, out var playerSelectionNumber))
        {
            Debug.Log("Player selection number is " + (int) playerSelectionNumber);
            int randomSpawnPosition = Random.Range(0, spawnPositions.Length - 1);
            Vector3 instantiatePosition = spawnPositions[randomSpawnPosition].position;

            // instantiate playerGameObject from prefab arr, position from spawn positions and Quaternion from prefab
            GameObject playerGameObject = Instantiate(playerPrefabs[(int) playerSelectionNumber], instantiatePosition,
                Quaternion.identity);

            // PhotonView attached to all game models
            PhotonView photonView = playerGameObject.GetComponent<PhotonView>();

            // PhotonNetwork.AllocateViewID - create and assign new viewId to displays photonView
            if (PhotonNetwork.AllocateViewID(photonView))
            {
                // extract our own position from battle arena
                // because other players dont know where we place battle arena
                // extract battleArena give us ONLY spawn point position
                var playerPositionIgnoreArenaPos = playerGameObject.transform.position - battleArena.transform.position;
                object[] data = new object[]
                {
                    playerPositionIgnoreArenaPos,
                    playerGameObject.transform.rotation,
                    photonView.ViewID,
                    playerSelectionNumber
                };


                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others, // send event to all players except me
                    CachingOption = EventCaching.AddToRoomCache // event kept in room catch, 
                };

                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true // if set to false it just overrides any current value
                };
                // Raise events!
                PhotonNetwork.RaiseEvent((byte) RaiseEvenCodes.PlayerSpawnEventCode, data, raiseEventOptions,
                    sendOptions);
            }
            else
            {
                // if we failed allocate viewId we will then destroy the object
                Debug.Log("Failed to allocate a viewId");
                Destroy(playerGameObject);
            }
        }
    }

    public void SpawnBot()
    {
        // create random selection bot prefab
        float botSelectionNumber = Random.Range(0, playerPrefabs.Length);
        Debug.Log("Bot selection number is " + (int) botSelectionNumber);
        int randomSpawnPosition = Random.Range(0, spawnPositions.Length - 1);
        Vector3 instantiatePosition = spawnPositions[randomSpawnPosition].position;

        // instantiate playerGameObject from prefab arr, position from spawn positions and Quaternion from prefab
        GameObject botGameObject = Instantiate(playerPrefabs[(int) botSelectionNumber], instantiatePosition,
            Quaternion.identity);
        botGameObject.GetComponent<BattleScript>().isBot = true;
        // PhotonView attached to all game models
        PhotonView photonView = botGameObject.GetComponent<PhotonView>();
        photonView.enabled = false;
        // PhotonNetwork.AllocateViewID - create and assign new viewId to displays photonView
        PhotonNetwork.AllocateViewID(photonView);
        botGameObject.GetComponent<BotScript>().enabled = true;
        
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    #endregion
}