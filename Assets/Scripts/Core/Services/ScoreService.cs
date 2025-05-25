using System;
using UnityEngine;

public interface IScoreService
{
    int Score { get; set; }
    event Action<int> OnScoreChanged;
}

public class ScoreService : IScoreService
{
    int _score = 0;
    public event Action<int> OnScoreChanged;
    public int Score
    {
        get => _score;
        set
        {
            _score = Mathf.Max(0, value);
            OnScoreChanged?.Invoke(_score);
        }
    }
}