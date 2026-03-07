using UnityEngine;
using System; // Necesario para Action

// <summary>
// Recoge cuando el jugador pulsa una tecla, y lanza un evento
// Teclas: 
//  - Movimientos: W, A, S, D ( o flechas arriba, izquierda, abajo, derecha)
//  - UI: Escape, Enter o Space y flechas arriba, izquierda, abajo, derecha o WASD
// </summary>
public class PlayerInput : MonoBehaviour
{
    // Definimos el evento (la señal de radio de tu imagen)
    // El string indicará QUÉ botón se presionó
    public static event Action<string> OnInputPressed;

    void Update()
    {
        // Gameplay y UI
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Emitimos el evento
            OnInputPressed?.Invoke("Up");
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnInputPressed?.Invoke("Down");
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnInputPressed?.Invoke("Left");
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnInputPressed?.Invoke("Right");
        }

        // UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnInputPressed?.Invoke("Escape");
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnInputPressed?.Invoke("Submit");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnInputPressed?.Invoke("Fin");
        }

    }
}