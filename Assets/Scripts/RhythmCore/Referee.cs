using UnityEngine;
using System;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;

// <summary>
// Misión: Pone todo en conjunto y aplica las reglas del juego.

// El único que conoce todas las reglas y las aplica para hacer que el juego funcione. es el GameManager.
// Deja que el resto haga sus cosas y los coordina.

// Es el único que depende del resto: va observando a todos para dar instrucciones.

// 1. Iniciación
//      Le da el track al Music Player
//      Le da los BPM al metrónomo
//      Le da los datos del nivel al Compositor
//      Emite un evento a todos. ¡Ha empezado el juego!
// 2. Game Loop
//      Anotación. Escucha al juez constantemente para anotar los resultados
//      Progreso. Aplica las reglas que definamos según esos resultados (vidas, estamina, combos, progreso en el combate...)
// 3. Fin del nivel. ¿Cuándo termina el nivel? Condición de victoria o derrota. (O empate)
//      ¿Al terminar la canción? ¿al llegar a una parte conccreta?
//      ¿Ah, es un loop? ¿Entonces al llegar a X compases?
//      ¿Objetivos especiales o concretos?

// Emite: ¡Juego terminado! y para a todos los componentes.
// </summary>
public class Referee : MonoBehaviour
{
    [Header("Store de música")]
    public MusicDataStore musicStore; // Arrastrar el archivo del MusicDataStore que he creado en el editor aquí. "GlobalMusicStore"
    // [Header("Configuración inicial")]
    // [SerializeField] private bool _useInitConfig = false; // Para usar la configuración que tengo aquí en el inspector, o usar la que me den por parámetros (ej: desde un menú de selección de nivel)
    // [Header("Configuración de la canción")]
    // [SerializeField] private EventReference musicEvent; // Arrastra aquí tu evento de música
    // [SerializeField] private int BPM = 128; // BPM de la canción
    // [Header("Métrica de la canción")]
    // [SerializeField] private int _numerador = 4; // Número de figuras por
    // [SerializeField] private int _denominador = 4; // Figura que representa el beat (4 para negra, 8 para corchea...)
    // [SerializeField] private int totalSegundosCancion = 3*60 + 20; // Para calcular el número total de beats, aunque también podríamos calcularlo a partir de la duración del track que nos da el Music Player
    // private int _totalBeats; // Calculado a partir de los segundos de la canción, los BPM y la métrica

    // [Header("Datos del nivel")]
    // [SerializeField] private List<BeatStep> score = new List<BeatStep>();
    // [SerializeField] private string jsonFileName = "MusicScore.json"; // Nombre del archivo JSON con la partitura, que debe estar en la carpeta Resources
    [SerializeField] private int _maxScore = 320;


    public static event Action<bool> GameStartEvent;
    public static event Action<int> MaxScoreSettedEvent;
    public static event Action<bool> GameEndEvent;
    public static event Action<int> AddScoreEvent;
    public static event Action<int> SubtractScoreEvent;
    public static event Action<int> UpdateComboEvent;
    public static event Action<int> UpdateComboSliderMAxEvent;

    public static event Action<int> UpdateScoreEvent;
    public static event Action<int> StarAchievedEvent;

    // Datos para las reglas del juego
    [Header("Ajustes de reglas")]
    private int _totalScore = 0;
    [SerializeField] private int _addScore = 1;
    [SerializeField] private int _subScore = 3;
    [Header("Ajustes de combo. Multiplicador de Add Score")]
    [SerializeField] private int _combo = 1;
    [SerializeField] private int _maxCombo = 4;
    [SerializeField] private int _rachaFor2 = 2; // Racha necesaria para llegar a combo x2
    [SerializeField] private int _rachaFor3 = 5; // Racha necesaria
    [SerializeField] private int _rachaFor4 = 10; // Racha necesaria para llegar a combo x4
    [Header("Ajustes de Estrellas")]
    [SerializeField] private int _scoreFor1Star = 75;
    [SerializeField] private int _scoreFor2Stars = 150;
    [SerializeField] private int _scoreFor3Stars = 300;
    private bool _oneStarAchieved = false;
    private bool _twoStarsAchieved = false;
    private bool _threeStarsAchieved = false;
    private bool _gameEnded = false; // Para evitar que se sigan sumando puntos o aumentando combos después de que el juego ha terminado

    private void OnEnable()
    {
        // Nos suscribimos a los eventos del Music Player y el Juez
        MusicPlayer.OnMusicStart += HandleMusicStart;
        MusicPlayer.OnMusicEnd += HandleMusicEnd;
        Judge.OnAcierto += HandleAcierto;
        Judge.OnRachaAumentada += HandleRachaAumentada;
        Judge.OnFallo += HandleFallo;
    }

    private void OnDisable()
    {
        MusicPlayer.OnMusicStart -= HandleMusicStart;
        MusicPlayer.OnMusicEnd -= HandleMusicEnd;
        Judge.OnAcierto -= HandleAcierto;
        Judge.OnFallo -= HandleFallo;
    }

    private void Start()
    {
        // if (_useInitConfig)
        // {
        //     Debug.Log("Usando configuración del Referee en el Inspector");
        // }
        // else
        // {
        //     Debug.Log("Usando configuración de las otras clases");
        // }
        musicStore.SetGameEnded(false); // Nos aseguramos de que el juego no esté marcado como terminado al iniciar, por si acaso
        musicStore.SetLastBeat(-1); // También inicializamos el último beat a -1
        musicStore.SetActiveBeat(-1); // Y el active beat a -1, para que otros sistemas puedan consultarlo desde el inicio
        musicStore.SetMaxScore(_maxScore); // Inicializamos el max score a 0, para que el Score Display pueda consultarlo y delimitar el slider desde el inicio. El Referee se encargará de calcularlo a medida que el Compositor le vaya dando los beats puntuables.
        MaxScoreSettedEvent?.Invoke(_maxScore); // Lanzamos el evento de max score para que el Score Display pueda inicializar el slider en cuanto el Referee lo calcule al inicio de la partida
    }


    private void HandleMusicStart()
    {
        Debug.Log("¡La música ha comenzado! El juego empieza.");
        // Lanzamos el evento de que el juego ha comenzado para que todos los sistemas se pongan en marcha
        GameStartEvent?.Invoke(true);
        _gameEnded = false; // Marcamos que el juego ha comenzado para que se puedan sumar puntos, aumentar combos, etc
        musicStore.SetGameEnded(false); // También lo marcamos en el MusicDataStore para
    }

    private void HandleMusicEnd()
    {
        Debug.Log("¡La música ha terminado! El juego termina.");
        // Lanzamos el evento de que el juego ha terminado para que todos los sistemas se detengan y muestren resultados, etc
        GameEndEvent?.Invoke(true);
        _gameEnded = true; // Marcamos que el juego ha terminado para que no se sigan sumando puntos o aumentando combos
        musicStore.SetGameEnded(true); // También lo marcamos en el MusicDataStore para que otros sistemas puedan consultarlo si lo necesitan
    }

    // Suma puntos:
    // +_addScore*_combo
    private void HandleAcierto(int totalAciertos)
    {
        if (_gameEnded) return; // Si el juego ha terminado, no hacemos nada

        _totalScore += _addScore * _combo;
        AddScoreEvent?.Invoke(_addScore * _combo);
        UpdateScoreEvent?.Invoke(_totalScore);
        // Comprobamos si hemos alcanzado el umbral para las estrellas
        if (!_oneStarAchieved && _totalScore >= _scoreFor1Star)
        {
            Debug.Log("¡Has conseguido 1 estrella!");
            _oneStarAchieved = true;
            StarAchievedEvent?.Invoke(1);
        }
        if (!_twoStarsAchieved && _totalScore >= _scoreFor2Stars)
        {
            Debug.Log("¡Has conseguido 2 estrellas!");
            _twoStarsAchieved = true;
            StarAchievedEvent?.Invoke(2);
        }
        if (!_threeStarsAchieved && _totalScore >= _scoreFor3Stars)
        {
            Debug.Log("¡Has conseguido 3 estrellas!");
            _threeStarsAchieved = true;
            StarAchievedEvent?.Invoke(3);
        }
    }

    // Si falla, resetea el combo a 1 y resta un poco
    private void HandleFallo(int totalFallos)
    {
        if (_gameEnded) return; // Si el juego ha terminado, no hacemos nada

        _combo = 1;
        HandleRachaAumentada(0); // Reseteamos la racha para que baje el combo
        _totalScore -= _subScore;
        if (_totalScore < 0) _totalScore = 0; // Evitamos que la puntuación sea negativa
        SubtractScoreEvent?.Invoke(_subScore);
        UpdateScoreEvent?.Invoke(_totalScore);
        UpdateComboSliderMAxEvent?.Invoke(_rachaFor2); 
    }

    private void HandleRachaAumentada(int rachaActual)
    {
        if (_gameEnded) return; // Si el juego ha terminado, no hacemos nada

        if (rachaActual < _rachaFor2)
        {
            _combo = 1;
            UpdateComboEvent?.Invoke(_combo);
            UpdateComboSliderMAxEvent?.Invoke(_rachaFor2); 
        }
        else if (rachaActual < _rachaFor3)
        {
            _combo = 2;
            UpdateComboEvent?.Invoke(_combo);
            UpdateComboSliderMAxEvent?.Invoke(_rachaFor3); 
        }
        else if (rachaActual < _rachaFor4)
        {
            _combo = 3;
            UpdateComboEvent?.Invoke(_combo);
            UpdateComboSliderMAxEvent?.Invoke(_rachaFor4); 
        }
        else
        {
            _combo = _maxCombo; // Combo máximo
            UpdateComboEvent?.Invoke(_combo);
        }
    }

    public int GetTotalScore()
    {
        return _totalScore;
    }

    public int GetCombo()
    {
        return _combo;
    }

    public int GetMaxCombo()
    {
        return _maxCombo;
    }

    public int GetRachaFor2()
    {
        return _rachaFor2;
    }

    public int GetRachaFor3()
    {
        return _rachaFor3;
    }

    public int GetRachaFor4()
    {
        return _rachaFor4;
    }

}