using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class FinPartidaCanvasController : MonoBehaviour
{
    // [SerializeField] private Text _tituloText;
    // [SerializeField] private String _textoVictoria = "Puntuación";
    [SerializeField] Sprite _estrellaLlena;
    [SerializeField] Sprite _estrellaVacia;
    [SerializeField] private Image[] estrellaImage; // Array de imágenes para mostrar las estrellas
    [SerializeField] private GameObject _canvas; 
    private CanvasGroup _canvasGroup; 

    private int _estrellas = 0;


    public void SetEstrellas(int cantidad)
    {
        _estrellas = cantidad;
        for (int i = 0; i < 3; i++)
        {
            if (i < _estrellas)
            {
                estrellaImage[i].sprite = _estrellaLlena;
            }
            else
            {
                estrellaImage[i].sprite = _estrellaVacia;
            }
        }
    }

    public void MostrarFinPartida()
    {
        _canvasGroup = _canvas.GetComponent<CanvasGroup>(); // Obtiene el CanvasGroup del canvas

        gameObject.SetActive(true); // Muestra el canvas
        _canvasGroup.alpha = 1f; // Asegura que el canvas sea visible
        _canvasGroup.interactable = true; // Permite la interacción con el canvas
        _canvasGroup.blocksRaycasts = true; // Permite que el canvas bloquee los raycasts
        Time.timeScale = 0f; // Pausa el juego
    }

    public void SalirAlMenuPrincipal()
    {
        Debug.Log("Saliendo al menú principal...");
        Time.timeScale = 1f;
        DesactivarCanvas();
        SceneLoader.Instance.LoadScene("Menu_Main");
    }

    public void ReiniciarNivel()
    {
        Debug.Log("Reiniciando nivel...");
        Time.timeScale = 1f;
        DesactivarCanvas();
        SceneLoader.Instance.LoadScene("Lvl_01_Gameplay");
    }

    private void DesactivarCanvas()
    {
        _canvasGroup.alpha = 0f; // Hace el canvas invisible
        _canvasGroup.interactable = false; // Desactiva la interacción
        _canvasGroup.blocksRaycasts = false; // Desactiva el bloqueo de raycasts
        gameObject.SetActive(false); // Desactiva el GameObject del canvas
    }
}
