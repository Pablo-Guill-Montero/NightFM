using UnityEngine;
using System; // Necesario para Action

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Transform[] movementPoints;
    private Transform _currentPoint;
    private int _currentPointIndex = 4; // Empezamos en el punto central (índice 4)

    private int _filaActual = 1;
    private int _columnaActual = 1;
    // Animacion
    private Animator _animator;

    [Header("Efectos de Partículas")]
    [SerializeField] private ParticleSystem particulasAcierto;
    [SerializeField] private ParticleSystem particulasFallo;

    public static event Action<int> OnPlayerMove; // Evento para que el juez sepa cuándo se ha movido el jugador y pueda evaluar si es correcto o no
    // public static event Action<int> OnPlayerEnterBeat; // Evento para saber la posición del player cada vez que entra un nuevo rango de beat

    private void OnEnable()
    {
        // Empezamos a escuchar la señal
        PlayerInput.OnInputPressed += ResponderAlPlayerInput;
        Referee.AddScoreEvent += HandleAddScore; 
        Referee.SubtractScoreEvent += HandleSubtractScore; 
        Referee.UpdateComboEvent += HandleUpdateCombo; 
    }

    private void OnDisable()
    {
        // Dejamos de escuchar para evitar errores de memoria
        PlayerInput.OnInputPressed -= ResponderAlPlayerInput;
        Referee.AddScoreEvent -= HandleAddScore;
        Referee.SubtractScoreEvent -= HandleSubtractScore;
        Referee.UpdateComboEvent -= HandleUpdateCombo;
    }

    // Cambiamos las animaciones del player según el combo
    private void HandleUpdateCombo(int combo)
    {
        _animator.SetInteger("dance", combo);
    }

    private void HandleSubtractScore(int newScore)
    {
        particulasFallo.Play();
        _animator.SetInteger("dance", 0);
    }

    private void HandleAddScore(int newScore)
    {
        particulasAcierto.Play();
    }

    // Recoge la señal de PlayerInput y responde a ella según la acción recibida
    void ResponderAlPlayerInput(string accion)
    {
        switch (accion)
        {
            case "Up":
                if (_filaActual > 0)
                {
                    _filaActual--;
                    UpdateCurrentPoint();
                }
                break;
            case "Down":
                if (_filaActual < 2)
                {
                    _filaActual++;
                    UpdateCurrentPoint();
                }
                break;
            case "Left":
                if (_columnaActual > 0)
                {
                    _columnaActual--;
                    UpdateCurrentPoint();
                }
                break;
            case "Right":
                if (_columnaActual < 2)
                {
                    _columnaActual++;
                    UpdateCurrentPoint();
                }
                break;
            case "Escape":
                UnityEngine.Object.FindFirstObjectByType<MenusController_Gameplay>().AbrirMenuPausa();
                break;
            case "Fin":
                // UnityEngine.Object.FindFirstObjectByType<MenusController_Gameplay>().AbrirFin();
                MusicPlayer musicPlayer = UnityEngine.Object.FindFirstObjectByType<MusicPlayer>();
                musicPlayer.stopMusic();
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (movementPoints.Length != 9)
        {
            Debug.LogError("¡Asegúrate de asignar exactamente 9 puntos de movimiento en el inspector!");
            return;
        }

        _currentPoint = movementPoints[_currentPointIndex];
        transform.position = _currentPoint.position; // Posicionamos al jugador en el punto inicial
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // No necesitamos revisar el input aquí, ya que lo manejamos a través del evento
    }

    void UpdateCurrentPoint()
    {
        _currentPointIndex = _filaActual * 3 + _columnaActual; // Calculamos el índice basado en la fila y columna
        _currentPoint = movementPoints[_currentPointIndex];
        transform.position = _currentPoint.position; // Movemos al jugador al nuevo punto
        // Lanzo el evento de que el jugador se ha movido
        OnPlayerMove?.Invoke(_currentPointIndex);
    }
}
