using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerNameInput : MonoBehaviour
{
    private InputField mInputField;
    const string playerNamePrefKey = "PlayerName";

    // Start is called before the first frame update
    void Start()
    {
        mInputField = this.GetComponent<InputField>();

        //defaultName was changed to playerName as playerName is more fitting
        string playerName = string.Empty;
        if (mInputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                playerName = PlayerPrefs.GetString(playerNamePrefKey);
                mInputField.text = playerName;
            }
        }
        PhotonNetwork.NickName = playerName;
    }

    public void SetPlayerName()
    {
        string value = mInputField.text;
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);

        Debug.Log("Nickname entered: " + value);
    }

}
