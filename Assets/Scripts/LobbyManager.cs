using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")] public InputField playerNameInputField;
    public GameObject uiLoginGameObject;


    [Header("Lobby UI")] public GameObject uiLobbyGameObject;
    public GameObject ui3DGameObject; // spinner object that will be visible after success connection to Photon servers

    [Header("Connection status UI")] public GameObject uiConnectionStatusGameObject;
    public Text connectionStatusText;
    public bool showConnectionStatus = false;

    [Header("Google Play Service")] public GameObject googlePlayService;
    
    #region UNITY Methods

    // Start is called before the first frame update
    void Start()
    {
        var gpgsScript = googlePlayService.GetComponent<GpgsScript>();
        gpgsScript.LogIn();

        if (PhotonNetwork.IsConnected)
        {
            // Activate only Lobby UI
            ShowLobbyUI();
        }
        else
        {
            ShowLoginUI();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (showConnectionStatus)
        {
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
            
            // todo if PhotonNetwork.NetworkClientState (disconnected) show err and throw on lobby scene 
        }
    }

    private void ShowLoginUI()
    {
        uiLobbyGameObject.SetActive(false);
        ui3DGameObject.SetActive(false);
        uiConnectionStatusGameObject.SetActive(false);

        uiLoginGameObject.SetActive(true);
    }

    private void ShowConnectionStatusUI()
    {
        uiLobbyGameObject.SetActive(false);
        ui3DGameObject.SetActive(false);
        uiLoginGameObject.SetActive(false);

        uiConnectionStatusGameObject.SetActive(true);
    }

    private void ShowLobbyUI()
    {
        uiConnectionStatusGameObject.SetActive(false);
        uiLoginGameObject.SetActive(false);

        uiLobbyGameObject.SetActive(true);
        ui3DGameObject.SetActive(true);
    }

    #endregion

    #region UI Callback Methods

    public void OnClickEnterGameButton()
    {
        string playerName = playerNameInputField.text;
        // if player name not empty or null let`s connect to Photon servers
        if (!string.IsNullOrEmpty(playerName))
        {
            showConnectionStatus = true;
            ShowConnectionStatusUI();
            // check that we already to Photon or not
            if (!PhotonNetwork.IsConnected)
            {
                // set player name to Photon
                PhotonNetwork.LocalPlayer.NickName = playerName;
                // connect to photon
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid or empty! ");
        }
    }

    public void OnClickQuickMatchButton()
    {
        // SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }

    #endregion

    #region PHOTON Callback Methods

    public override void OnConnected()
    {
        Debug.Log("We connected to Internet!");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server!");
        ShowLobbyUI();
    }

    #endregion
}