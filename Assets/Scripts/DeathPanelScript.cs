using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region UI Callback Methods

    public void OnClickNextOpponentButton()
    {
        Debug.Log("OnClickNextOpponentButton");
        
        SpinningTopsGameManager gameManager = FindObjectOfType<SpinningTopsGameManager>();
        
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }

    public void OnClickRevengeButton()
    {
        // SendMessage("TryToRevenge", SendMessageOptions.RequireReceiver);
        Debug.Log("OnClickRevengeButton");
        BattleScript[] battleScripts = FindObjectsOfType<BattleScript>();

        foreach (BattleScript battle in battleScripts)
        {
            if (!battle.isBot && battle.photonView.IsMine)
            {
                battle.StopReSpawnCountDown();
            }
        }
    }

    #endregion
}