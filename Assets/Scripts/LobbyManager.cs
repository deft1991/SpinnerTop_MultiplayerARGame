using System;
using GooglePlayGames;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")] public InputField playerNameInputField;
    public GameObject uiLoginGameObject;

    [Header("Lobby UI")] public GameObject uiLobbyGameObject;
    public GameObject ui3DGameObject; // spinner object that will be visible after success connection to Photon servers
    public TextMeshProUGUI playerNameTMP;

    [Header("Profile UI")] public GameObject uiProfileGameObject;
    public GameObject ui3DGameObjectForProfileView; // spinner object that will be visible after click on profile

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
    }


    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            TryPhotonLoginWithGooglePlayName(playerNameTMP.text);
        }

        if (showConnectionStatus)
        {
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;

            // todo if PhotonNetwork.NetworkClientState (disconnected) show err and throw on lobby scene 
        }
        
        playerNameTMP.text = PlayGamesPlatform.Instance.localUser.userName;
    }

    #endregion

    #region UI Callback Methods

    public void OnClickEnterGameButton()
    {
        playerNameTMP.text = playerNameInputField.text;
        // if player name not empty or null let`s connect to Photon servers
        TryPhotonLogin(playerNameTMP.text);
    }

    public void OnClickQuickMatchButton()
    {
        // SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }

    public void OnClickProfileButton()
    {
        // Scene_Profile
        // SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_Profile");
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

    #region PrivateMethods

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

    private void TryPhotonLoginWithGooglePlayName(String playerName)
    {
        // if player name not empty or null let`s connect to Photon servers
        TryPhotonLogin(playerName);
    }

    private void TryPhotonLogin(string playerName)
    {
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

    #endregion
}