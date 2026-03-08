using UnityEngine;
using System;

// <summary>
// Misión: Valida las acciones del jugador.
//
// - Pregunta al Compositor: ¿Next 7?
// - Se guarda el objetivo actual -> Beat Pos.
// - Espera a recibir un input del jugador
// - Juzga: Mira la ventana del activeBeat del metrónomo.
//     - Si el player entra en esa posición en el mismo Beat: Acierto (En otro script suma puntos y aumenta la racha)
//     - Si se pasa el beat, y no ha entrado: Fallo (En otro script corta la racha y no puntúa)
// - No guarda Score, solo emite eventos tras juzgar:
//     - Fallo
//     - Acierto
// - Se repite volviendo a preguntar al compositor: ¿Next?
// 
// </summary>
public class Judge : MonoBehaviour
{
    [Header("Store de música")]
    public MusicDataStore musicStore; // Arrastrar el archivo del MusicDataStore que he creado en el editor aquí. "GlobalMusicStore"

    private int _currentTargetBeat; // El beat que el compositor ha dicho que es puntuable, y que el jugador debe intentar acertar
    private int _currentTargetCell; // La casilla que el compositor ha dicho que es el objetivo del golpe, y que el jugador debe intentar acertar

    private int _playerCurrentCell = 4; // La casilla en la que se encuentra el jugador actualmente, que se actualizará cada vez que el jugador se mueva o entre en un nuevo beat
    private bool _acertadoOpasado = false; // Para evitar que el jugador pueda acertar varias veces en el mismo beat si se mueve dentro de la ventana

    private int _fallos = 0;
    private int _aciertos = 0;
    private int _rachaActual = 0;

    // Eventos para comunicar a otros sistemas de gameplay los aciertos y fallos
    public static event Action<int> OnAcierto;
    public static event Action<int> OnFallo;
    public static event Action<int> OnRachaAumentada;

    private void OnEnable()
    {
        // Escuchamos compositor 
        Composer.OnPuntuableBeat += HandlePuntuableBeat; // escuchamos al compositor para saber cuándo es un beat puntuable y cuál es la casilla objetivo para ese beat
        PlayerController.OnPlayerMove += HandlePlayerMove; // Escuchamos al jugador para saber cuándo se ha movido y evaluar si es correcto o no
        Metronom.EnterBeatEvent += HandleEnterBeat; // Escuchamos cuándo se sale de un beat para resetear el estado de acierto/fallo
        Metronom.ExitBeatEvent += HandleExitBeat; // Escuchamos cuándo se sale de un beat para resetear el estado de acierto/fallo
    }

    private void OnDisable()
    {
        Composer.OnPuntuableBeat -= HandlePuntuableBeat;
        PlayerController.OnPlayerMove -= HandlePlayerMove;
        Metronom.EnterBeatEvent -= HandleEnterBeat;
        Metronom.ExitBeatEvent -= HandleExitBeat;
    }

    private void HandlePuntuableBeat(int beatNumber, int cellNumber)
    {
        if (musicStore.GetGameEnded()) return; // Si el juego ha terminado, no hacemos nada

        _currentTargetBeat = beatNumber;
        _currentTargetCell = cellNumber;
        Debug.Log($"Nuevo beat puntuable: Beat {_currentTargetBeat}, Casilla {_currentTargetCell}");
    }

    // actualia la posición del player y juzga
    private void HandlePlayerMove(int playerCellIndex)
    {
        if (musicStore.GetGameEnded()) return; // Si el juego ha terminado, no hacemos nada

        _playerCurrentCell = playerCellIndex;
        Juzgar();
    }

    // Al entrar en un nuevo beat resetea el booleano de acierto_o_pasado y juzga
    private void HandleEnterBeat(int beatNumber)
    {
        if (musicStore.GetGameEnded()) return; // Si el juego ha terminado, no hacemos nada

        // Cada vez que entramos en un nuevo beat, reseteamos el estado de acierto/fallo para permitir que el jugador pueda acertar de nuevo
        _acertadoOpasado = false;
        Juzgar(); // Evaluamos si el jugador ya estaba posicionado en esa casilla
    }

    // Al salir de un beat, si no ha acertado aún, se lanza el fallo y se marca como pasado para no permitir más aciertos en ese beat
    private void HandleExitBeat(int beatNumber)
    {
        if (musicStore.GetGameEnded()) return; // Si el juego ha terminado, no hacemos nada

        // Si salimos del beat y no se ha acertado, es un fallo
        JuzgarExitBeat(beatNumber);
        if (!_acertadoOpasado && beatNumber == _currentTargetBeat)
        {
            Debug.Log("¡Fallo! Posicion del jugador: " + _playerCurrentCell + ", Posición objetivo: " + _currentTargetCell);
            _acertadoOpasado = true; // Marcamos ha pasado el beat
            _fallos++; // Sumamos un fallo
            _rachaActual = 0; // Reseteamos la racha
            // Lanzamos el evento de fallo para que otros sistemas puedan reaccionar (cortar racha, etc)    
            OnFallo?.Invoke(_fallos);
        }

    }

    // Evalúa si el jugador ha acertado o no, comparando su posición actual con la posición objetivo del compositor y el beat actual del metrónomo
    private void Juzgar()
    {
        // si aún no ha acertado ni ha pasado el beat, y el jugador está en la casilla objetivo y el beat actual es el objetivo, entonces es un acierto
        if (!_acertadoOpasado && _playerCurrentCell == _currentTargetCell && musicStore.GetActiveBeat() == _currentTargetBeat)
        {
            Debug.Log("¡Acierto!");
            _acertadoOpasado = true; // Marcamos que se ha acertado para no permitir más aciertos en este beat
            _aciertos++; // Sumamos un acierto
            _rachaActual++; // Sumamos a la racha
            // Lanzamos el evento de acierto para que otros sistemas puedan reaccionar (sumar puntos, aumentar racha, etc)
            OnAcierto?.Invoke(_aciertos);
        }
        // else
        // {
        //     Debug.Log("¡Nada!");
        // }
    }

    private void JuzgarExitBeat(int exitBeatNumber)
    {
        // si aún no ha acertado ni ha pasado el beat, y el jugador está en la casilla objetivo y el beat actual es el objetivo, entonces es un acierto
        if (!_acertadoOpasado && _playerCurrentCell == _currentTargetCell && exitBeatNumber == _currentTargetBeat)
        {
            Debug.Log("¡Acierto!");
            _acertadoOpasado = true; // Marcamos que se ha acertado para no permitir más aciertos en este beat
            _aciertos++; // Sumamos un acierto
            _rachaActual++; // Sumamos a la racha
            // Lanzamos el evento de acierto para que otros sistemas puedan reaccionar (sumar puntos, aumentar racha, etc)
            OnAcierto?.Invoke(_aciertos);
            OnRachaAumentada?.Invoke(_rachaActual);
        }
        // else
        // {
        //     Debug.Log("¡Nada!");
        // }
    }
}