using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionInfoListUIItem : MonoBehaviour
{
    public TMP_Text sessionNameTxt;
    public TMP_Text playerCountTxt;
    public Button joinBtn;

    SessionInfo sessionInfo;
    public event Action<SessionInfo> OnJoinSession;

    public void SetInformation(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;

        sessionNameTxt.text = sessionInfo.Name;
        playerCountTxt.text = $"{sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";

        bool isJoinButtonActive = true;

        if (sessionInfo.PlayerCount >= sessionInfo.MaxPlayers)
            isJoinButtonActive = false;

        joinBtn.gameObject.SetActive(isJoinButtonActive);
    }

    public void OnClick()
    {
        OnJoinSession?.Invoke(sessionInfo);
    }
}
