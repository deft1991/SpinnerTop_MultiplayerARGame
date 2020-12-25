﻿using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnBoosterManager : MonoBehaviourPunCallbacks
{
    public GameObject[] boosterPrefabs;
    public Transform[] spawnPositions;
    public GameObject battleArena;
    private float _timer = 0f;

    private enum RaiseEvenCodes
    {
        BoosterSpawnEventCode = 1
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // _timer += Time.deltaTime;
        //
        // if (_timer > 15)
        // {
        //     StartCoroutine(SpawnBoosterAfterSeconds(3f));
        //     _timer = 0;
        // }
    }

    #region PHOTON Callback Methods

    /**
     * Called only on all clients to spawn booster in battle arena.
     * It will called in our local game
     */
    public void OnEventBoosterSpawn(EventData photonEvent)
    {
        if ((byte) RaiseEvenCodes.BoosterSpawnEventCode == photonEvent.Code)
        {
            object[] data = (object[]) photonEvent.CustomData; // get data from PhotonNetwork.RaiseEvent
            Vector3 receivedPosition = (Vector3) data[0];
            Quaternion receiveRotation = (Quaternion) data[1];
            int receivedBoosterSelectionData = (int) data[3];

            GameObject booster = Instantiate(
                boosterPrefabs[receivedBoosterSelectionData]
                , battleArena.transform.position +
                  receivedPosition // because we should place remote object on our own battle arena
                , receiveRotation);

            PhotonView photonView = booster.GetComponent<PhotonView>();
            photonView.ViewID = (int) data[2];
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnBooster();
        }
    }

    #endregion

    #region PUBLIC

    public void AddOnBoosterSpawnEvent()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventBoosterSpawn;
    }

    /**
     * See https://doc.photonengine.com/en-us/pun/current/gameplay/instantiation
     * for better understanding photon sync
     */
    public void SpawnBooster()
    {
        int boosterNumber = Random.Range(0, boosterPrefabs.Length);
        Debug.Log("Booster number is " + (int) boosterNumber);
        int randomSpawnPosition = Random.Range(0, spawnPositions.Length);
        Vector3 instantiatePosition = spawnPositions[randomSpawnPosition].position;

        // instantiate playerGameObject from prefab arr, position from spawn positions and Quaternion from prefab
        GameObject boosterGameObject = Instantiate(boosterPrefabs[(int) boosterNumber], instantiatePosition,
            Quaternion.identity);

        // PhotonView attached to all game models
        PhotonView photonView = boosterGameObject.GetComponent<PhotonView>();

        // PhotonNetwork.AllocateViewID - create and assign new viewId to displays photonView
        if (PhotonNetwork.AllocateViewID(photonView))
        {
            // extract our own position from battle arena
            // because other players dont know where we place battle arena
            // extract battleArena give us ONLY spawn point position
            var playerPositionIgnoreArenaPos =
                boosterGameObject.transform.position - battleArena.transform.position;
            object[] data = new object[]
            {
                playerPositionIgnoreArenaPos,
                boosterGameObject.transform.rotation,
                photonView.ViewID,
                boosterNumber
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
            PhotonNetwork.RaiseEvent((byte) RaiseEvenCodes.BoosterSpawnEventCode, data, raiseEventOptions,
                sendOptions);
        }
        else
        {
            // if we failed allocate viewId we will then destroy the object
            Debug.Log("Failed to allocate a viewId");
            Destroy(boosterGameObject);
        }
    }

    #endregion
    
    //todo change spawn chest
    public IEnumerator SpawnBoosterAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // set booster in the room
        SpawnBooster();
    }
    
    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventBoosterSpawn;
    }

}