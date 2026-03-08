using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using FMODUnity;
using FMOD.Studio;
// using System; // Para usar Action

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
    // public EventReference radioSnapshot; 
    // public EventReference normalSnapshot;
    // public EventReference pauseSnapshot;
    // private EventInstance radioSnapshotInstance;
    // private EventInstance normalSnapshotInstance;
    // private EventInstance pauseSnapshotInstance;

    public MusicPlayer musicPlayer; // Arrastra aquí tu MusicPlayer para poder pausar la música desde este script

    private int _menuState = 0; // 0 = Sin menú, 1 = Menú de pausa, 2 = Confirmación de salida, 3 = Fin de partida

    private void OnEnable()
    {
        // Empezamos a escuchar la señal
        PlayerInput.OnInputPressed += ResponderAlPlayerInput;
        Referee.GameEndEvent += AbrirFin; // Nos suscribimos al evento de fin de juego para abrir el canvas de fin de partida
    }

    private void OnDisable()
    {
        // Dejamos de escuchar para evitar errores de memoria
        PlayerInput.OnInputPressed -= ResponderAlPlayerInput;
        Referee.GameEndEvent -= AbrirFin; // Nos desuscribimos del evento de fin de juego al desactivar el script
    }

    // private void OnDestroy()
    // {
    //     // Liberamos la memoria de FMOD al destruir el script
    //     radioSnapshotInstance.release();
    //     normalSnapshotInstance.release();
    //     pauseSnapshotInstance.release();
    // }

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
        // radioSnapshotInstance = RuntimeManager.CreateInstance(radioSnapshot);
        // normalSnapshotInstance = RuntimeManager.CreateInstance(normalSnapshot);
        // pauseSnapshotInstance = RuntimeManager.CreateInstance(pauseSnapshot);

        // normalSnapshotInstance.start(); // Empezamos con el snapshot normal al iniciar el juego
    }

    void ResponderAlPlayerInput(string accion)
    {
        switch (accion)
        {
            case "Escape":
                if (_menuState == 0)
                {
                    AbrirMenuPausa();
                }
                // else 
                // if (_menuState == 1)
                // {
                //     CerrarMenuPausa();
                // }
                else if (_menuState == 2)
                {
                    CerrarConfirmacion();
                }
                break;
        }
    }

    // // Método auxiliar para limpiar snapshots antes de poner una nueva
    // private void LimpiarSnapshots()
    // {
    //     radioSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     normalSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     pauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    // }

    public void AbrirFin(bool playerWon)
    {
        _finPartidaCanvas.gameObject.SetActive(true);
        _FinPartidaCanvasGroup.alpha = 1f;
        _FinPartidaCanvasGroup.interactable = true;
        _FinPartidaCanvasGroup.blocksRaycasts = true;
        // Mostramos el canvas de fin de partida y pausamos el juego
        _finPartidaController.MostrarFinPartida();

        // LimpiarSnapshots();
        // pauseSnapshotInstance.start(); // Enchufar la snapshot de pausa al abrir el canvas de fin de partida

    }


    // Menús


    public void AbrirMenuPausa()
    {
        _pauseCanvas.gameObject.SetActive(true);
        _pauseCanvasGroup.alpha = 1f;
        _pauseCanvasGroup.interactable = true;
        _pauseCanvasGroup.blocksRaycasts = true;
        // Aquí podrías agregar lógica adicional para pausar el juego, como detener el tiempo
        Time.timeScale = 0f; // Pausa el juego
        
        // LimpiarSnapshots();
        // pauseSnapshotInstance.start(); // Enchufar la snapshot de pausa al abrir el menú de pausa

        musicPlayer.pauseMusic(); // Pausamos la música al abrir el menú de pausa

        _menuState = 1;
    }

    public void CerrarMenuPausa()
    {
        _pauseCanvas.gameObject.SetActive(false);
        _pauseCanvasGroup.alpha = 0f;
        _pauseCanvasGroup.interactable = false;
        _pauseCanvasGroup.blocksRaycasts = false;

        // Aquí podrías agregar lógica adicional para reanudar el juego, como reanudar el tiempo
        Time.timeScale = 1f; // Reanuda el juego

        // LimpiarSnapshots();
        // normalSnapshotInstance.start(); // Vuelve al snapshot normal al cerrar el menú de pausa

        musicPlayer.unPauseMusic(); // Reanudamos la música al cerrar el menú de pausa  

        _menuState = 0;
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

        _menuState = 2;
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

        _menuState = 1;
    }

    public void SalirAlMenuPrincipal()
    {
        Debug.Log("Saliendo al menú principal...");
        Time.timeScale = 1f;
        // LimpiarSnapshots();
        // radioSnapshotInstance.start();
        SceneLoader.Instance.LoadScene("Menu_Main");
        _menuState = 0;
    }

}
