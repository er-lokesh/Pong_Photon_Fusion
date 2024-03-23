using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PingPongManager : NetworkBehaviour
{
    private NetworkObject spawnedBall;
    public NetworkPrefabRef ballPrefab;

    public TMP_Text teamAScoreTxt;
    public TMP_Text teamBScoreTxt;

    public int teamAPoint;
    public int teamBPoint;
    private static PingPongManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static PingPongManager GetInstance()
    {
        if (!instance)
            instance = FindObjectOfType<PingPongManager>();
        return instance;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AddPoints(TargtPlayer target)
    {
        switch (target)
        {
            case TargtPlayer.TeamA:
                teamAPoint++;
                break;
            case TargtPlayer.TeamB:
                teamBPoint++;
                break;
        }
        UpdateScore();
    }

    public void AddTeamAPoints()
    {
        RPC_AddPoints(TargtPlayer.TeamA);
    }

    public void AddTeamBPoints()
    {
        RPC_AddPoints(TargtPlayer.TeamB);
    }

    private void UpdateScore()
    {
        teamAScoreTxt.text = teamAPoint.ToString();
        teamBScoreTxt.text = teamBPoint.ToString();
    }

    public void SetReady()
    {
        RPC_SetReady();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetReady(RpcInfo info = default)
    {
        if (Runner.IsServer)
        {
            spawnedBall = Runner.Spawn(ballPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}

public enum TargtPlayer
{
    TeamA, TeamB
}
