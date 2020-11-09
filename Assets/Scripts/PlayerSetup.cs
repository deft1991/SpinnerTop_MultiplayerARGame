using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine) // if it`s me
        {
            // The player is local player
            SetEnableToMovementController(true);
        }
        else
        {
            // The player is remote player
            SetEnableToMovementController(false);
        }

        SetPlayerName();
    }

    private void SetEnableToMovementController(bool isEnabled)
    {
        transform.GetComponent<MovementController>().enabled = isEnabled;
        transform.GetComponent<MovementController>().joystick.gameObject.SetActive(isEnabled);
    }


    void SetPlayerName()
    {
        if (playerNameText != null)
        {
            if (photonView.IsMine)
            {
                playerNameText.text = "YOU";
                playerNameText.color = Color.green;
                
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
                playerNameText.color = Color.red;
            }
        }
    }
}