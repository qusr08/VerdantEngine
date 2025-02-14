using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager playerManager;

    public int money;
    [SerializeField] private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = this;
        DontDestroyOnLoad(playerManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
