using UnityEngine;

[CreateAssetMenu(fileName = "NewMusicStore", menuName = "Data/MusicStore")]
public class MusicDataStore : ScriptableObject
{
    // La variable donde se guarda el dato
    private int _currentTimelinePosition;
    private int _activeBeat;

    // Función para escribir (Set)
    public void SetTimelinePosition(int position)
    {
        _currentTimelinePosition = position;
    }

    public void SetActiveBeat(int beat)
    {
        _activeBeat = beat;
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
}