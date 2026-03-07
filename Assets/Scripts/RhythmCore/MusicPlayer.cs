using UnityEngine;
using System;
using FMODUnity;
using FMOD.Studio;

// <summary>
// Le damos un track y lo reproduce
// También es encargado de la música dinámica y music cues (fragmentos cortos musicales o respuestas de sonido cortos)
// - Encargado de añadir nuevas capas de instrumentos, efectos de audio, cambiar de track...
// - Es nuestra interfaz con FMOD o Wwise, es caso de usarlos.
// - Indica en cada frame, Update(), la posición en Ms o más preciso y escribe en el Store para quien lo necesite leer.
// </summary>
public class MusicPlayer : MonoBehaviour
{
    [Header("Store de música")]
    public MusicDataStore musicStore; // Arrastrar el archivo del MusicDataStore que he creado en el editor aquí. "GlobalMusicStore"

    [Header("Configuración de FMOD")]
    [SerializeField] private EventReference musicEvent; // Arrastra aquí tu evento de música
    private FMOD.Studio.EventInstance _musicInstance;
    // Posición en MS del track, que se actualizará cada frame
    private int _currentMusicPosition;

    // Snapshot Gameplay
    [Header("Snapshot Gameplay")]
    [SerializeField] private EventReference gameplaySnapshot;
    private FMOD.Studio.EventInstance _gameplaySnapshotInstance;

    // Eventos
    public static event Action OnMusicStart;
    public static event Action OnMusicEnd;

    void Start()
    {
        _gameplaySnapshotInstance = RuntimeManager.CreateInstance(gameplaySnapshot);
        _gameplaySnapshotInstance.start();
        _gameplaySnapshotInstance.release();

        _musicInstance = RuntimeManager.CreateInstance(musicEvent);
        _musicInstance.start();
        _musicInstance.release();

        // Lanzamos el evento de que la música ha comenzado
        OnMusicStart?.Invoke();

    }

    private void Update()
    {
        // Actualizamos la posición de la música cada frame
        _musicInstance.getTimelinePosition(out int position);
        _currentMusicPosition = position;
        // Escribimos la posición en un Store:
        musicStore.SetTimelinePosition(position);

        // Si termina la música, lanzamos el evento de que ha terminado
        _musicInstance.getPlaybackState(out PLAYBACK_STATE playbackState);
        if(playbackState == PLAYBACK_STATE.STOPPED)
        {
            OnMusicEnd?.Invoke();
        }
    }

    void OnDestroy()
    {
        // 4. DETENER la música si el objeto se destruye
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _gameplaySnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void pauseMusic()
    {
        _musicInstance.setPaused(true);
    }

    public void unPauseMusic()
    {
        _musicInstance.setPaused(false);
    }
}