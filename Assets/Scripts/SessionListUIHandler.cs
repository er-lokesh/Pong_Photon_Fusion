using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListUIHandler : MonoBehaviour
{
    public TMP_Text statusText;
    public GameObject sessionItemListPrefab;
    public VerticalLayoutGroup verticalLayoutGroup;

    private void Awake()
    {
        ClearList();
    }

    public void ClearList()
    {
        foreach (Transform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
        statusText.gameObject.SetActive(false);
    }

    public void AddToList(SessionInfo sessionInfo)
    {
        SessionInfoListUIItem uiItem = Instantiate(sessionItemListPrefab, verticalLayoutGroup.transform).GetComponent<SessionInfoListUIItem>();
        uiItem.SetInformation(sessionInfo);
        uiItem.OnJoinSession += AddSessionInfo_OnJoinSession;
    }

    private void AddSessionInfo_OnJoinSession(SessionInfo sessionInfo)
    {
        BasicSpawner basicSpawner = FindObjectOfType<BasicSpawner>();
        basicSpawner.JoinGame(sessionInfo);

        MainMenuUIHandler mainMenuHandler = FindObjectOfType<MainMenuUIHandler>();
        mainMenuHandler.OnJoiningServer();
    }

    public void OnNoSessionsFound()
    {
        ClearList();
        statusText.text = "No game session found";
        statusText.gameObject.SetActive(true);
    }

    public void OnLookingForGameSession()
    {
        ClearList();
        statusText.text = "Looking for game session";
        statusText.gameObject.SetActive(true);
    }
}
