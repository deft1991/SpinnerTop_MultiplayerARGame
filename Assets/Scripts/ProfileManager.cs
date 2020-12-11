using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using TMPro;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    
    [Header("Google Play Service")] 
    public TextMeshProUGUI playerNameTMP;
    public GpgsScript gpgsScript;
    
    // Start is called before the first frame update
    void Start()
    {
        playerNameTMP.text = PlayGamesPlatform.Instance.localUser.userName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region UI Callback Methods

    public void OnClickBack()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    #endregion
}
