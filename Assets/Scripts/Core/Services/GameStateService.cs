using System;
using Zenject;

public interface IGameStateService
{
    GameState State { get; }
    void SetState(GameState state);
    event Action<GameState> OnGameStateChanged;
}

public class GameStateService : IGameStateService
{
    readonly SignalBus _signals;
    GameState _state = GameState.Init;
    public event Action<GameState> OnGameStateChanged;

    [Inject]
    public GameStateService(SignalBus signals)
    {
        _signals = signals;
    }

    public GameState State => _state;

    public void SetState(GameState state)
    {
        if (_state == state) return;
        _state = state;
        OnGameStateChanged?.Invoke(state);
        _signals.Fire(new GameStateChangedSignal(state));
    }
}