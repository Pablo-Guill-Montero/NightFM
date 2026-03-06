using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform[] movementPoints;

    private Transform _currentPoint;
    private int _currentPointIndex = 4; // Empezamos en el punto central (índice 4)

    private int _filaActual = 1;
    private int _columnaActual = 1;

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
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        // Movimiento del jugador basado en las teclas W, A, S, D o las flechas del teclado
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _filaActual > 0)
        {
            _filaActual--;
            UpdateCurrentPoint();
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && _filaActual < 2 )
        {
            _filaActual++;
            UpdateCurrentPoint();
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && _columnaActual > 0 )
        {
            _columnaActual--;
            UpdateCurrentPoint();
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && _columnaActual < 2 )
        {
            _columnaActual++;
            UpdateCurrentPoint();
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Object.FindFirstObjectByType<MenusController_Gameplay>().AbrirMenuPausa();
        }

        else if (Input.GetKeyDown(KeyCode.V))
        {
            Object.FindFirstObjectByType<MenusController_Gameplay>().AbrirFin();
        }
    }

    void UpdateCurrentPoint()
    {
        _currentPointIndex = _filaActual * 3 + _columnaActual; // Calculamos el índice basado en la fila y columna
        _currentPoint = movementPoints[_currentPointIndex];
        transform.position = _currentPoint.position; // Movemos al jugador al nuevo punto
    }
}
