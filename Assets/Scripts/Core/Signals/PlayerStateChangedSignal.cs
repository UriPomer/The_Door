// Assets/Scripts/Signals/PlayerStateChangedSignal.cs
public class PlayerStateChangedSignal
{
    /// <summary>新的玩家状态</summary>
    public PlayerState NewState { get; }

    public PlayerStateChangedSignal(PlayerState newState)
    {
        NewState = newState;
    }
}