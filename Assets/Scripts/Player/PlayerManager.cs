using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    private Player player;
    //[SerializeField] private InputHandler input;

    public Player GetPlayerModel(){
        return player;
    }

    public void SetPlayerModel(Player loadedPlayer)
    {
       player=loadedPlayer;
    }
}
