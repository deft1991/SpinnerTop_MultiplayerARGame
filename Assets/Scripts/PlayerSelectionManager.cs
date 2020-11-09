using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; 
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;
    public GameObject[] spinnerTopModels;

   

    public int playerSelectionNumber;

    [Header("UI")] 
    public TextMeshProUGUI playerModelTypeText;

    public GameObject uiSelection;
    public GameObject uiAfterSelection;
    public Button nextButton;
    public Button previousButton;
    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        playerSelectionNumber = 0;

        ActivateUiSelection();
    }

    private void ActivateUiSelection()
    {
        uiSelection.SetActive(true);
        uiAfterSelection.SetActive(false);
    }    
    
    private void ActivateUiAfterSelection()
    {
        uiSelection.SetActive(false);
        uiAfterSelection.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    #endregion

    #region UI Callback Methods

    public void OnClickNextPlayer()
    {
        playerSelectionNumber++;
        if (playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }
        
        Debug.Log("PlayerSelectionNumber: " + playerSelectionNumber);
        SetEnableToButtons(false);
        
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1f));

        SetPlayerModelTypeText();
    }

    public void OnClickPreviousPlayer()
    {
        playerSelectionNumber--; 
        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }

        Debug.Log("PlayerSelectionNumber: " + playerSelectionNumber);
        
        SetEnableToButtons(false);
        
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1f));

        SetPlayerModelTypeText();
    }
    
    public void OnClickSelectPlayer()
    {
        ActivateUiAfterSelection();
        
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable
        {
            {MultiplayerArSpinnerTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnClickReSelect()
    {
        ActivateUiSelection();
    }

    public void OnClickBattle()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnClickBack()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    private void SetPlayerModelTypeText()
    {
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            // This means the player model type is ATTACK
            playerModelTypeText.text = "Attack";
        }
        else
        {
            // This means the player model type is DEFEND
            playerModelTypeText.text = "Defend";
        }
    }

    private void SetEnableToButtons(bool isEnable)
    {
        nextButton.enabled = isEnable;
        previousButton.enabled = isEnable;
    }

    #endregion

    #region Private Methods

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // Time.deltaTime - is a time passed from the last frame
            yield return null;
        }

        transformToRotate.rotation = finalRotation;
        SetEnableToButtons(true);
    }

    #endregion
}