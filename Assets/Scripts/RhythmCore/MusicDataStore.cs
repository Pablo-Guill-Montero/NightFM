using UnityEngine;

[CreateAssetMenu(fileName = "NewMusicStore", menuName = "Data/MusicStore")]
public class MusicDataStore : ScriptableObject
{
    // La variable donde se guarda el dato
    private int _currentTimelinePosition;
    private int _activeBeat;
    private bool _gameEnded = false;
    private int _lastBeat = -1;
    private int _maxScore = 100;

    // Función para escribir (Set)
    public void SetTimelinePosition(int position)
    {
        _currentTimelinePosition = position;
    }

    public void SetActiveBeat(int beat)
    {
        _activeBeat = beat;
    }

    public void SetGameEnded(bool ended)
    {
        _gameEnded = ended;
    }

    public void SetLastBeat(int beat)
    {
        _lastBeat = beat;
    }
    public void SetMaxScore(int maxScore)
    {
        _maxScore = maxScore;
    }

    // Función para obtener (Get)
    public int GetTimelinePosition()
    {
        return _currentTimelinePosition;
    }

    public int GetActiveBeat()
    {
        return _activeBeat;
    }

    public bool GetGameEnded()
    {
        return _gameEnded;
    }
    public int GetLastBeat()
    {
        return _lastBeat;
    }
    public int GetMaxScore()
    {
        return _maxScore;
    }

    // Opcional: Una propiedad directa para leer/escribir más rápido
    public int CurrentPosition 
    { 
        get => _currentTimelinePosition; 
        set => _currentTimelinePosition = value; 
    }

    public int ActiveBeat
    {
        get => _activeBeat;
        set => _activeBeat = value;
    }

    public bool GameEnded
    {
        get => _gameEnded;
        set => _gameEnded = value;
    }

    public int LastBeat
    {
        get => _lastBeat;
        set => _lastBeat = value;
    }

    public int MaxScore
    {
        get => _maxScore;
        set => _maxScore = value;
    }
}