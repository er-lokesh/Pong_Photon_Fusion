using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    public GameObject playerDetailsPanel;
    public GameObject sessionBrowserPanel;
    public GameObject createSessionPanel;
    public GameObject statusPanel;

    private string playerNickName = "PlayerNickName";
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_InputField sessionNameInputField;
    BasicSpawner basicSpawner;

    // Start is called before the first frame update
    void Start()
    {
        basicSpawner = FindObjectOfType<BasicSpawner>();
        if (PlayerPrefs.HasKey(playerNickName))
            playerNameInputField.text = PlayerPrefs.GetString(playerNickName);
    }

    void HideAllPanels()
    {
        playerDetailsPanel.SetActive(false);
        sessionBrowserPanel.SetActive(false);
        statusPanel.SetActive(false);
        createSessionPanel.SetActive(false);
    }

    public void OnFindGameClicked()
    {
        PlayerPrefs.SetString(playerNickName, playerNameInputField.text);
        PlayerPrefs.Save();

        basicSpawner.OnJoinLobby();
        HideAllPanels();

        sessionBrowserPanel.gameObject.SetActive(true);
        FindObjectOfType<SessionListUIHandler>(true).OnLookingForGameSession();
    }

    internal void OnJoiningServer()
    {
        HideAllPanels();
        statusPanel.SetActive(true);
    }

    public void OnCreateNewGameClicked()
    {
        HideAllPanels();
        createSessionPanel.SetActive(true);
    }

    public void OnStartNewSessionClicked()
    {
        basicSpawner.CreateGame(sessionNameInputField.text);
        HideAllPanels();
        statusPanel.SetActive(true);
    }
}
