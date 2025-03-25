using System;
using UnityEngine;

public enum GameState
{
    FreeRoam,
    Interaction,
    MenuOpened,
    Inventory,
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    
    public GameState CurrentState { get; private set; } = GameState.FreeRoam;
    
    // Evento notificante il cambiamento di stato
    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Se vuoi mantenere il manager tra le scene
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
            Debug.Log("Nuovo stato: " + newState);
        }
    }
}
