using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private NetworkRunner runner;
    SessionListUIHandler sessionListUIHandler;

    private void Awake()
    {
        NetworkRunner networkRunner = FindObjectOfType<NetworkRunner>();
        sessionListUIHandler = FindObjectOfType<SessionListUIHandler>(true);
        if (networkRunner != null)
            runner = networkRunner;
    }

    private void Start()
    {
        if(runner == null)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            runner = gameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;

            if(SceneManager.GetActiveScene().name != "Menu")
                StartGame(GameMode.AutoHostOrClient, "TestSession");
        }
    }

    async Task StartGame(GameMode mode, string sessionName)
    {
        
        // Create the NetworkSceneInfo from the current scene
        //var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex + 1);
        //var sceneInfo = new NetworkSceneInfo();
        //if (scene.IsValid)
        //{
        //    sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        //}

        // Start or join (depends on gamemode) a session with a specific name
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionName,
            CustomLobbyName = "OurLobbyId",
            PlayerCount = Utils.GetMaxPlayers(),
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex + 1),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnJoinLobby()
    {
        var clientTask = JoinLobby();
    }

    private async Task JoinLobby()
    {
        string lobbyId = "OurLobbyId";
        var result = await runner.JoinSessionLobby(SessionLobby.Custom, lobbyId);

        if (!result.Ok)
            Debug.LogError($"Unable to join lobby {lobbyId}");
        else
            Debug.Log("Join Lobby OK");
    }

    public void CreateGame(string sessionName)
    {
        var clientTask = StartGame(GameMode.Host, sessionName);
    }
    
    public void JoinGame(SessionInfo sessionInfo)
    {
        var clientTask = StartGame(GameMode.Client, sessionInfo.Name);
    }

    //private void OnGUI()
    //{
    //    if (_runner == null)
    //    {
    //        if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
    //        {
    //            StartGame(GameMode.Host);
    //        }
    //        if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
    //        {
    //            StartGame(GameMode.Client);
    //        }
    //    }
    //}

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        SceneManager.LoadScene(0);
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        SceneManager.LoadScene(0);
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        data.direction = new Vector2(0, Input.GetAxisRaw("Vertical"));
        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    int index = 0;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            //Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Utils.GetSpawnPoint(index++), Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }

        //if (_spawnedCharacters.Count == Utils.GetMaxPlayers())
            FindObjectOfType<PingPongManager>().SetReady();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
            SceneManager.LoadScene(0);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (sessionList == null) return;

        if (sessionList.Count == 0)
            sessionListUIHandler.OnNoSessionsFound();
        else
        {
            sessionListUIHandler.ClearList();
            foreach(SessionInfo sessionInfo in sessionList)
            {
                sessionListUIHandler.AddToList(sessionInfo);
                Debug.Log($"Found session {sessionInfo.Name} player count : {sessionInfo.PlayerCount}");
            }
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene(0);
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
}
