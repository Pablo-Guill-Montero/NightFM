using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using FMODUnity;
using FMOD.Studio;

public class MenusController_Gameplay : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject _confirmExitCanvas; 
    [SerializeField] private GameObject _pauseCanvas; 
    [SerializeField] private GameObject _finPartidaCanvas; 
    private CanvasGroup _pauseCanvasGroup;
    private CanvasGroup _ConfirmExitCanvasGroup; 
    private CanvasGroup _FinPartidaCanvasGroup;
    private FinPartidaCanvasController _finPartidaController;


    // Arrastra el Snapshot desde la ventana de FMOD Event Browser al Inspector
    public EventReference radioSnapshot; 
    public EventReference normalSnapshot;
    public EventReference pauseSnapshot;
    private EventInstance radioSnapshotInstance;
    private EventInstance normalSnapshotInstance;
    private EventInstance pauseSnapshotInstance;

    void Start()
    {
        // Aseguramos que el cuadro de confirmación esté oculto al iniciar
        _confirmExitCanvas.SetActive(false);
        _pauseCanvas.SetActive(false);
        _finPartidaCanvas.SetActive(false);
        _ConfirmExitCanvasGroup = _confirmExitCanvas.GetComponent<CanvasGroup>();
        _pauseCanvasGroup = _pauseCanvas.GetComponent<CanvasGroup>();
        _FinPartidaCanvasGroup = _finPartidaCanvas.GetComponent<CanvasGroup>();

        _finPartidaController = _finPartidaCanvas.GetComponent<FinPartidaCanvasController>();

        // Creamos la instancia del snapshot para poder controlarlo
        radioSnapshotInstance = RuntimeManager.CreateInstance(radioSnapshot);
        normalSnapshotInstance = RuntimeManager.CreateInstance(normalSnapshot);
        pauseSnapshotInstance = RuntimeManager.CreateInstance(pauseSnapshot);
    }

    public void AbrirFin()
    {
        _finPartidaCanvas.gameObject.SetActive(true);
        _FinPartidaCanvasGroup.alpha = 1f;
        _FinPartidaCanvasGroup.interactable = true;
        _FinPartidaCanvasGroup.blocksRaycasts = true;
        // Mostramos el canvas de fin de partida y pausamos el juego
        _finPartidaController.MostrarFinPartida();

        pauseSnapshotInstance.start(); // Enchufar la snapshot de pausa al abrir el canvas de fin de partida
    }

    public void AbrirMenuPausa()
    {
        _pauseCanvas.gameObject.SetActive(true);
        _pauseCanvasGroup.alpha = 1f;
        _pauseCanvasGroup.interactable = true;
        _pauseCanvasGroup.blocksRaycasts = true;
        // Aquí podrías agregar lógica adicional para pausar el juego, como detener el tiempo
        Time.timeScale = 0f; // Pausa el juego

        pauseSnapshotInstance.start(); // Enchufar la snapshot de pausa al abrir el menú de pausa
    }

    public void CerrarMenuPausa()
    {
        _pauseCanvas.gameObject.SetActive(false);
        _pauseCanvasGroup.alpha = 0f;
        _pauseCanvasGroup.interactable = false;
        _pauseCanvasGroup.blocksRaycasts = false;

        // Aquí podrías agregar lógica adicional para reanudar el juego, como reanudar el tiempo
        Time.timeScale = 1f; // Reanuda el juego

        normalSnapshotInstance.start(); // Vuelve al snapshot normal al cerrar el menú de pausa
    }

    // Esta función activa la confirmación y bloquea el fondo
    public void AbrirConfirmacion()
    {
        // 1. Mostramos el cuadro de confirmación
        _confirmExitCanvas.SetActive(true);
        _ConfirmExitCanvasGroup.alpha = 1f; // Aseguramos que el cuadro de confirmación sea visible
        _ConfirmExitCanvasGroup.interactable = true; // Permitimos la interacción con el cuadro

        // 2. Bloqueamos la interacción del fondo
        // 'interactable' falso evita clicks en botones
        _pauseCanvasGroup.interactable = false;
        // 'blocksRaycasts' falso hace que el ratón "atraviese" el fondo si fuera necesario
        _pauseCanvasGroup.blocksRaycasts = false;
    }

    // Esta función cierra la confirmación y desbloquea el fondo
    public void CerrarConfirmacion()
    {
        // 1. Ocultamos el cuadro de confirmación
        _confirmExitCanvas.SetActive(false);
        _ConfirmExitCanvasGroup.alpha = 0f; // Aseguramos que el cuadro de confirmación sea invisible
        _ConfirmExitCanvasGroup.interactable = false; // Desactivamos la interacción con el

        // 2. Devolvemos la interacción al fondo
        _pauseCanvasGroup.interactable = true;
        _pauseCanvasGroup.blocksRaycasts = true;
    }

    public void SalirAlMenuPrincipal()
    {
        Debug.Log("Saliendo al menú principal...");
        Time.timeScale = 1f;
        radioSnapshotInstance.start();
        SceneLoader.Instance.LoadScene("Menu_Main");
    }

}
