using UnityEngine;
using System;

// <summary>
// Lee del Storage la posición en Ms que escribe Music Player.
// Dependiendo de la estructura de la canción puede contar:
//
//  a) Beat: Tiempos dentro del compás
//
//  b) Bar: Compases enteros
//
//  c) Partes completas.
//
// Nos dice, Dónde estamos en la partitura.
// Su implementación se complica a medida que escala el juego.

// Guarda los BPM y duración en segundos o Ms de un beat, compás... lo que necesitemos.
// Depende de la métrica de la canción: (N/D)
// Figura en Segundos = (60/BPM) * (4/D)
// Compás en Segundos = (60/BPM) * (4/D) * N
//
// Cuando comienza la canción inicializa 2 valores:
// lastBeat = 0; // Beat global de la canción
// nextBeatPosition = beatDurationSec; // Posición del siguiente beat en la canción
//
// Si lo queremos en Ms es multiplicar nuestros resultados en segundos por 1000 en las fórmula.
// </summary>
public class Metronom : MonoBehaviour
{
    [Header("Store de música")]
    public MusicDataStore musicStore; // Arrastrar el archivo del MusicDataStore que he creado en el editor aquí. "GlobalMusicStore"

    [Header("Configuración de la canción")]
    [SerializeField] private int BPM = 128; // BPM de la canción
    [Header("Métrica de la canción")]
    [SerializeField] private int _numerador = 4; // Número de figuras por compás (4 para negra, 8 para corchea...)
    [SerializeField] private int _denominador = 4; // Figura que representa el beat (4 para negra, 8 para corchea...)

    private int _beatDurationMS; // Duración de un beat en Ms, calculada a partir de los BPM y la métrica

    private int _currentMusicPosition; // Posición actual de la música en Ms, que leeremos del MusicPlayer
    private int _nextBeatPosition;
    private int _lastBeat;
    private int _lastBeatInBar;

    // Cuando el time position de la música está dentro del margen de error, devuelve su nº en activeBeat; en otro caso -1 o null.
    // Funciona como una ventana que se abre y se cierra cuando se entra y sale en el rango de un beat con su margen de error.
    [Header("Ajustes de Ventana")]
    [SerializeField] private int margin = 100; // Margen de error en MS (ej: 100ms antes y después)

    private int _activeBeat = -1; // -1 significa que estamos fuera de cualquier ventana
    private bool _isInWindow = false;
    // Buffer
    private int _activeBeatStartPosition;
    private int _activeBeatEndPosition;

    // Eventos
    public static event Action<int> BeatEvent;
    public static event Action<int> EnterBeatEvent; 
    public static event Action<int> ExitBeatEvent; 
    public static event Action<int> BarEvent; // Compás entero


    void Start()
    {
        // Calculamos la duración de un beat en Ms
        _beatDurationMS = Mathf.RoundToInt((60f / BPM) * (4f / _denominador) * 1000f);
        // Inicializamos los valores de los beats
        _lastBeat = 0;
        _lastBeatInBar = 0;
        _nextBeatPosition = _beatDurationMS;
        _activeBeatStartPosition = _nextBeatPosition - margin;
        _activeBeatEndPosition = _nextBeatPosition + margin;
    }

    void Update()
    {
        _currentMusicPosition = musicStore.GetTimelinePosition();

        // 1. Lógica de ventanas Entrada y Salida
        // ¿Estamos dentro del rango de la ventana?
        if (_currentMusicPosition >= _activeBeatStartPosition && _currentMusicPosition <= _activeBeatEndPosition)
        {
            if (!_isInWindow) // Acabamos de entrar
            {
                _isInWindow = true;
                _activeBeat = _lastBeat + 1; // Es el beat que está por venir
                EnterBeatEvent?.Invoke(_activeBeat);
            }
        }
        else
        {
            if (_isInWindow) // Estábamos dentro y acabamos de salir
            {
                _isInWindow = false;
                ExitBeatEvent?.Invoke(_activeBeat);
                _activeBeat = -1; // Limpiamos el beat activo
            }
        }

        // 2. Comprobamos si hemos llegado al siguiente beat
        if(_currentMusicPosition >= _nextBeatPosition)
        {
            _lastBeat ++;
            // Emitimos el evento de que hemos entrado en un nuevo beat
            BeatEvent?.Invoke(_lastBeat);
            _nextBeatPosition += _beatDurationMS;
            _lastBeatInBar ++;
            if(_lastBeatInBar > _numerador)
            {
                _lastBeatInBar = 1;
                // Emitimos el evento de que hemos entrado en un nuevo compás
                BarEvent?.Invoke(_lastBeat);
            }
            // Calculamos los límites del próximo beat
            _activeBeatStartPosition = _nextBeatPosition - margin;
            _activeBeatEndPosition = _nextBeatPosition + margin;

            // Debug
            Debug.Log($"Beat: {_lastBeat}, Next Beat Position: {_nextBeatPosition}ms, Active Beat Window: [{_activeBeatStartPosition}ms - {_activeBeatEndPosition}ms]");
        }

    }
    
}