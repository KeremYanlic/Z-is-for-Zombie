using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class GameManager : SingletonMonobehaviour<GameManager>
{
    public GameResources gameResources;
    private PlayerDetailsSO playerDetailsSO;

    private Player player;
    protected override void Awake()
    {
        base.Awake();

        playerDetailsSO = gameResources.playerDetailsSO;

        InstantiatePlayer();


    }

    public Player GetPlayer()
    {
        return player;
    }
    /// <summary>
    /// Create player in scene at position
    /// </summary>
    private void InstantiatePlayer()
    {
        // Instantiate player
        GameObject playerGameObject = Instantiate(playerDetailsSO.playerPrefab, Vector3.zero - new Vector3(50f, 0f, 0f), Quaternion.identity);

        player = playerGameObject.GetComponent<Player>();
        // Initialize Player
        player.Initialize(playerDetailsSO);

    }



}
